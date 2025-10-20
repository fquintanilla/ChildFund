using System.ComponentModel;

namespace ChildFund.Services.Extensions
{
    public static class WebEnumHelper<T>
    {
        public static string GetDescription(T value)
        {
            Type type = value.GetType();

            var memInfo = type.GetMember(value.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return value.ToString();
        }

        public static string GetDescription(string value)
        {
            try
            {
                var valueEnum = Parse(value);
                return GetDescription(valueEnum);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
