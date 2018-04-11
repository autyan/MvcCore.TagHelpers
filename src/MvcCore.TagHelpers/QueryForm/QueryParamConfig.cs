using System.Reflection;
using MvcCore.TagHelpers.Extensions;

namespace MvcCore.TagHelpers.QueryForm
{
    public class QueryParamConfig
    {
        public string ParamName { get; set; }

        public string ParamDisplayName { get; set; }

        public string PlaceHolder { get; set; }

        public InputControlType ParamType { get; set; } = InputControlType.Text;

        public object ParamData { get; set; }

        public object ParamValue { get; set; }

        public QueryParamConfig(PropertyInfo queryPropertyInfo, object query)
        {
            ParamName = queryPropertyInfo.Name;
            var displayName = queryPropertyInfo.GetDisplayName();
            ParamDisplayName = string.IsNullOrWhiteSpace(displayName) ? queryPropertyInfo.Name : displayName;
            PlaceHolder = ParamDisplayName;

            ParamValue = queryPropertyInfo.GetValue(query);
        }
    }
}
