using System.Globalization;
using MvcCore.TagHelpers.Resource;

namespace MvcCore.TagHelpers
{
    public class MvcTaghelperStringLocalizer
    {
        private static CultureInfo _curtureInfo;

        internal static CultureInfo CultureInfo
        {
            get => _curtureInfo ?? (_curtureInfo = new CultureInfo("en-US"));
            set => _curtureInfo = value;
        }

        public static MvcTaghelperStringLocalizer Instance => new MvcTaghelperStringLocalizer();

        public string this[string name] => CultureDefinitions.GetResourceReader(CultureInfo)[name];
    }
}
