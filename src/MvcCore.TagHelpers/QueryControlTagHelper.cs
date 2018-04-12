using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.QueryForm;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("query-control", ParentTag = "query-form")]
    public class QueryControlTagHelper : TagHelper
    {
        [HtmlAttributeName("query-name")]
        public string QueryName { get; set; }

        [HtmlAttributeName("query-type")]
        public string QueryType { get; set; }

        [HtmlAttributeName("query-control-data")]
        public object QueryControlObject { get; set; }

        [HtmlAttributeName("query-control-index")]
        public int Index { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var queryParamConfigs = (IList<QueryParamConfig>)context.Items[typeof(IList<QueryParamConfig>)];

            var me = queryParamConfigs.FirstOrDefault(c => c.ParamName == QueryName);
            if (me == null) throw new NullReferenceException("Query Item Not Found");

            if (!string.IsNullOrWhiteSpace(QueryType) && Enum.TryParse(QueryType, out InputControlType type))
            {
                me.ParamType = type;
            }
            else
            {
                me.ParamType = InputControlType.Text;
            }

            me.ParamData = QueryControlObject;
            me.ParamIndex = Index;

            output.SuppressOutput();
        }
    }
}
