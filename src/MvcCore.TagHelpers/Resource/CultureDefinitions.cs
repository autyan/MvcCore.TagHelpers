using System.Collections.Generic;
using System.Globalization;

namespace MvcCore.TagHelpers.Resource
{
    public static class CultureDefinitions
    {
        public static readonly IDictionary<string,string> DefaultResourceManager = new Dictionary<string, string>
        {
            {"Submit", "Submit" },
            {"Reset", "Reset" },
            {"TotalItem", "{0} items in totlal" },
        };

        public static readonly IDictionary<string, string> ZhCnResourceManager = new Dictionary<string, string>
        {
            {"Submit", "提交" },
            {"Reset", "重置" },
            {"TotalItem", "共 {0} 项结果" },
        };


        public static IDictionary<string, string> GetResourceReader(CultureInfo cultureInfo)
        {
            switch (cultureInfo.Name)
            {
                case "zh-CN":
                    return ZhCnResourceManager;
                default:
                    return DefaultResourceManager;
            }
        }
    }
}
