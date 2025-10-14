using ChildFund.Web.Core.CustomRoutes.Error;
using ChildFund.Web.Features.CatalogContent.Services;
using ChildFund.Web.Features.Checkout.Payments;
using ChildFund.Web.Features.Checkout.Services;
using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Features.MyAccount.CreditCard;
using ChildFund.Web.Features.MyOrganization.Organization;
using ChildFund.Web.Infrastructure.Cms.Accessor;
using ChildFund.Web.Infrastructure.Cms.Helpers;
using ChildFund.Web.Infrastructure.Cms.ModelBinders;
using ChildFund.Web.Infrastructure.Cms.Services;
using ChildFund.Web.Infrastructure.Cms.Settings;
using ChildFund.Web.Infrastructure.Commerce.Customer.Services;
using ChildFund.Web.Infrastructure.Commerce.Extensions;
using ChildFund.Web.Infrastructure.Commerce.Markets;
using ChildFund.Web.Infrastructure.Commerce.Pricing;
using ChildFund.Web.Infrastructure.Display;
using ChildFund.Web.Infrastructure.Rendering;
using Episerver.Marketing.Connector.Framework.Services;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Globalization;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus.Configurator;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Constant = ChildFund.Web.Infrastructure.Commerce.Constant;

namespace ChildFund.Web.Infrastructure.Initialization
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        private IServiceCollection _services;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;

            context.Services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            context.Services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            context.Services.AddTransient<IsInEditModeAccessor>(locator =>
                () => locator.GetInstance<IContextModeResolver>().CurrentMode.EditOrPreview());

            //Commerce
            context.Services.AddTransient<CheckoutViewModelFactory>();
            //context.Services.AddSingleton<MultiShipmentViewModelFactory>();
            context.Services.AddSingleton<OrderSummaryViewModelFactory>();
            context.Services.AddSingleton<PaymentMethodViewModelFactory>();
            //context.Services.AddSingleton<CatalogEntryViewModelFactory>();
            //context.Services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            context.Services.AddSingleton<CartItemViewModelFactory>();
            context.Services.AddSingleton<CartViewModelFactory>();
            context.Services.AddSingleton<ShipmentViewModelFactory>();

            //Services
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
            context.Services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator =>
                locator.GetInstance<IContentRouteHelper>);
            context.Services.AddTransient<IModelBinderProvider, ModelBinderProvider>();
            context.Services.AddSingleton<IDataService, DataService>();
            context.Services.AddSingleton<ICacheService, CacheService>();
            context.Services.AddSingleton<IConfigurationService, ConfigurationService>();
            context.Services.AddSingleton<IEncryptionService, EncryptionService>();
            
            context.Services.Intercept<IPlacedPriceProcessor>((locator, defaultImplementation) =>
                new CustomPlacedPriceProcessor(defaultImplementation));

            // Custom Routes
            context.Services.AddTransient<NotFoundHandler>();
            context.Services.AddTransient<ErrorHandler>();

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
            context.InitComplete += OnInitComplete;
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= OnInitComplete;
        }

        private void OnInitComplete(object sender, EventArgs e)
        {
            _services.AddTransient<ContentAreaRenderer, CustomContentAreaRenderer>();

            var context = OrderContext.MetaDataContext;
            var lineItemMetaClass = OrderContext.Current.LineItemMetaClass;

            // Define all fields you want to add
            var metaFields = new[]
            {
                new { Name = Constant.LineItemFields.ChildId, DisplayName = "Child Id", Type = MetaDataType.ShortString, Length = 64 },
                new { Name = Constant.LineItemFields.ChildName, DisplayName = "Child Name", Type = MetaDataType.LongString, Length = 256 },
                new { Name = Constant.LineItemFields.PaymentFrequency, DisplayName = "Payment Frequency", Type = MetaDataType.ShortString, Length = 64 },
                new { Name = Constant.LineItemFields.IsCustomPrice, DisplayName = "Is Custom Price", Type = MetaDataType.Boolean, Length = 1 },
            };

            foreach (var f in metaFields)
            {
                var field = MetaField.Load(context, f.Name)
                            ?? MetaField.Create(
                                context,
                                lineItemMetaClass.Namespace,
                                f.Name,
                                f.DisplayName,
                                string.Empty,
                                f.Type,
                                f.Length,
                                allowNulls: true,
                                multiLanguageValue: false,
                                allowSearch: true,
                                isEncrypted: false
                            );

                if (lineItemMetaClass.MetaFields.All(x => x.Id != field.Id))
                {
                    lineItemMetaClass.AddField(field);
                }
            }
        }
    }
}
