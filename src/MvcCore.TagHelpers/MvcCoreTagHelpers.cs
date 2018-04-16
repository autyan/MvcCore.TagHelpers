using System;
using System.Globalization;
using System.Reflection;

namespace MvcCore.TagHelpers
{
    public class MvcCoreTagHelpers
    {
        public static MvcCoreTagHelpers Instance { get; } = new MvcCoreTagHelpers();

        private MvcCoreTagHelpers() { }

        public MvcCoreTagHelpers SetCulture(string culture)
        {
            MvcTaghelperStringLocalizer.CultureInfo = new CultureInfo(culture);
            return Instance;
        }

        public MvcCoreTagHelpers SetQueryIgnore(string ingoreProperties)
        {
            QueryFormTagHelper.AddIgnoreProperty(ingoreProperties);
            return Instance;
        }

        public MvcCoreTagHelpers SetQueryIgnore(Type ignoreType)
        {
            foreach (var propertyInfo in ignoreType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                QueryFormTagHelper.AddIgnoreProperty(propertyInfo.Name);
            }
            return Instance;
        }
    }
}
