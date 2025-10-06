using ChildFund.Features.CatalogContent.Services;
using ChildFund.Features.Checkout.Payments;
using ChildFund.Features.Checkout.Services;
using ChildFund.Features.Checkout.ViewModels;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Features.MyAccount.CreditCard;
using ChildFund.Features.MyOrganization.Organization;
using ChildFund.Features.Shared;
using ChildFund.Infrastructure.Cms;
using ChildFund.Infrastructure.Cms.Settings;
using ChildFund.Infrastructure.Commerce.Customer.Services;
using ChildFund.Infrastructure.Commerce.Extensions;
using ChildFund.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.Commerce.Order;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;

namespace ChildFund.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<CheckoutViewModelFactory>();
            //context.Services.AddSingleton<MultiShipmentViewModelFactory>();
            context.Services.AddSingleton<OrderSummaryViewModelFactory>();
            context.Services.AddSingleton<PaymentMethodViewModelFactory>();
            //context.Services.AddSingleton<CatalogEntryViewModelFactory>();
            //context.Services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            context.Services.AddSingleton<CartItemViewModelFactory>();
            context.Services.AddSingleton<CartViewModelFactory>();
            context.Services.AddSingleton<ShipmentViewModelFactory>();

            context.Services.AddTransient<ICustomerService, CustomerService>();
            context.Services.AddTransient<ICookieService, CookieService>();
            context.Services.AddTransient<ICurrencyService, CurrencyService>();
            context.Services.AddTransient<ICartService, CartService>();
            context.Services.AddTransient<IPricingService, PricingService>();
            context.Services.AddTransient<IPaymentService, PaymentService>();
            context.Services.AddTransient<IAddressBookService, AddressBookService>();
            context.Services.AddTransient<IMailService, MailService>();
            context.Services.AddTransient<IProductService, ProductService>();
            context.Services.AddTransient<IPromotionService, PromotionService>();
            context.Services.AddTransient<IShippingService, ShippingService>();
            context.Services.AddTransient<IOrganizationService, OrganizationService>();
            context.Services.AddTransient<IMailService, MailService>();
            context.Services.AddSingleton<IHtmlDownloader, HtmlDownloader>();
            context.Services.AddTransient<CheckoutService>();
            context.Services.AddSingleton<ISettingsService, SettingsService>();
            context.Services.AddSingleton<ICreditCardService, CreditCardService>();

            context.Services.AddTransient<IPaymentMethod, GenericCreditCardPaymentOption>();

            context.ConfigurationComplete += (o, e) =>
            {
                e.Services.Intercept<IUpdateCurrentLanguage>(
                    (locator, defaultImplementation) =>
                        new LanguageService(
                            locator.GetInstance<ICurrentMarket>(),
                            locator.GetInstance<ICookieService>(),
                            defaultImplementation));
            };
        }

        public void Initialize(InitializationEngine context)
        {
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            context.InitializeFoundationCommerce();
        }
        
        public void Uninitialize(InitializationEngine context)
        {
            
        }
    }
}
