using System.Globalization;

namespace MvcCore.TagHelpers
{
    public class MvcCoreTagHelpers
    {
        public static void SetCulture(string culture)
        {
            MvcTaghelperStringLocalizer.CultureInfo = new CultureInfo(culture);
        }
    }
}
