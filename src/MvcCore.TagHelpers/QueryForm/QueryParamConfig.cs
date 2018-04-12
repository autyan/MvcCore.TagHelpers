using System;
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

        public string ParamValue { get; set; }

        public int ParamIndex { get; set; } = 999;

        public QueryParamConfig(PropertyInfo queryPropertyInfo, object query)
        {
            ParamName = queryPropertyInfo.Name;
            var displayName = queryPropertyInfo.GetDisplayName();
            ParamDisplayName = string.IsNullOrWhiteSpace(displayName) ? queryPropertyInfo.Name : displayName;
            PlaceHolder = ParamDisplayName;
            ParamValue = GetValueString(queryPropertyInfo, query);
        }

        private string GetValueString(PropertyInfo queryPropertyInfo, object query)
        {
            if (query == null) return string.Empty;

            var value = queryPropertyInfo.GetValue(query);

            if (value == null) return string.Empty;

            var propertyUnderType = Nullable.GetUnderlyingType(queryPropertyInfo.PropertyType);
            if (queryPropertyInfo.PropertyType.IsEnum || ((propertyUnderType != null) && propertyUnderType.IsEnum))
            {
                return ((int)value).ToString();
            }

            return value.ToString();
        }
    }
}
