using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcCore.TagHelpers.Extensions;

namespace MvcCore.TagHelpers.QueryForm
{
    public class QueryParamTagBuilder : IQueryParamElementTagBuilder
    {
        public string ParamName { get; set; }

        public string ParamDisplayName { get; set; }

        public string PlaceHolder { get; set; }

        public InputControlType ParamType { get; set; } = InputControlType.Text;

        public string ParamValue { get; set; }

        public int ParamIndex { get; set; } = 999;

        internal QueryParamTagBuilder(IQueryParamElementTagBuilder builder)
        {
            ParamName = builder.ParamName;
            ParamDisplayName = builder.ParamDisplayName;
            PlaceHolder = builder.PlaceHolder;
            ParamType = builder.ParamType;
            ParamValue = builder.ParamValue;
            ParamIndex = builder.ParamIndex;
        }

        public QueryParamTagBuilder(PropertyInfo queryPropertyInfo, object query)
        {
            ParamName = queryPropertyInfo.Name;
            var displayName = queryPropertyInfo.GetDisplayName();
            ParamDisplayName = string.IsNullOrWhiteSpace(displayName) ? queryPropertyInfo.Name : displayName;
            PlaceHolder = ParamDisplayName;
            ParamValue = GetValueString(queryPropertyInfo, query);
        }

        private static string GetValueString(PropertyInfo queryPropertyInfo, object query)
        {
            if (query == null) return string.Empty;

            var value = queryPropertyInfo.GetValue(query);

            if (value == null) return string.Empty;

            var propertyUnderType = Nullable.GetUnderlyingType(queryPropertyInfo.PropertyType);
            if (queryPropertyInfo.PropertyType.IsEnum || propertyUnderType?.IsEnum == true)
            {
                return ((int)value).ToString();
            }

            return value.ToString();
        }

        public virtual TagBuilder Build()
        {
            var label = new TagBuilder("label");
            label.Attributes["for"] = ParamName;
            label.InnerHtml.Append(ParamDisplayName);
            var content = BuildControlTag();

            var inputGroupAddon = new TagBuilder("div");
            inputGroupAddon.Attributes["class"] = "input-group-addon";
            inputGroupAddon.InnerHtml.AppendHtml(label);

            var inputGroup = new TagBuilder("div");
            inputGroup.Attributes["class"] = "input-group";
            inputGroup.InnerHtml.AppendHtml(inputGroupAddon);
            inputGroup.InnerHtml.AppendHtml(content);

            var formGroup = new TagBuilder("div");
            formGroup.Attributes["class"] = "form-group";
            formGroup.InnerHtml.AppendHtml(inputGroup);

            return formGroup;
        }

        protected virtual TagBuilder BuildControlTag()
        {
            var controlTag = new TagBuilder("input");
            controlTag.Attributes["name"] = ParamName;
            controlTag.Attributes["class"] = "form-control";
            controlTag.Attributes["placeholder"] = PlaceHolder;
            controlTag.Attributes["type"] = Enum.GetName(typeof(InputControlType), ParamType);
            controlTag.Attributes["value"] = ParamValue;
            return controlTag;
        }
    }
}
