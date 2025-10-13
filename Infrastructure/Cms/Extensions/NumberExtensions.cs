namespace ChildFund.Infrastructure.Cms.Extensions
{
	public static class NumberExtensions
    {
        public static string FormatCurrency(this decimal amount)
        {
            var cultureInfo = new CultureInfo("en-US");
            string formattedAmount = amount.ToString("N2", cultureInfo);
            return "$" + formattedAmount;
        }
    }
}
