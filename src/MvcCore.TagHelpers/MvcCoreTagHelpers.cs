using System.Globalization;

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

        public MvcCoreTagHelpers SetTableIgnore(string ingoreProperties)
        {
            TableTagHelper.AddIgnoreProperty(ingoreProperties);
            return Instance;
        }
    }
}
