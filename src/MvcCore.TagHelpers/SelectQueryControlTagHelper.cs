using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.QueryForm;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("query-control-select", ParentTag = "query-form")]
    public class SelectQueryControlTagHelper : QueryControlTagHelper
    {
        [HtmlAttributeName("option-value-name")]
        public string OptionValueName { get; set; } = "Value";

        [HtmlAttributeName("option-text-name")]
        public string OptionTextName { get; set; } = "Text";

        public SelectQueryControlTagHelper()
        {
            QueryType = "Select";
        }

        protected override void SetupBuilder(IQueryParamElementTagBuilder builder)
        {
            var selectBuilder = new SelectQueryParamTagBuilder(builder)
            {
                ValuePropertyName = OptionValueName,
                TextPropertyName = OptionTextName
            };

            QueryTagBuilders.Remove(builder);
            QueryTagBuilders.Add(selectBuilder);

            if (!string.IsNullOrWhiteSpace(QueryType) && Enum.TryParse(QueryType, out InputControlType type))
            {
                selectBuilder.ParamType = type;
            }
            else
            {
                selectBuilder.ParamType = InputControlType.Text;
            }

            selectBuilder.ParamData = QueryControlObject;
            selectBuilder.ParamIndex = Index;
        }
    }
}
