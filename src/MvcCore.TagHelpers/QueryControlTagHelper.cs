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

        [HtmlAttributeName("query-control-index")]
        public int? Index { get; set; }

        protected IList<IQueryParamElementTagBuilder> QueryTagBuilders;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            QueryTagBuilders = (IList<IQueryParamElementTagBuilder>)context.Items[typeof(IList<IQueryParamElementTagBuilder>)];

            var me = QueryTagBuilders.FirstOrDefault(c => c.ParamName == QueryName);
            if (me == null) throw new NullReferenceException("Query Item Not Found");
            me = SetupBuilder(me);
            if (Index != null)
            {
                me.ParamIndex = Index.Value;
            }
            output.SuppressOutput();
        }

        protected virtual IQueryParamElementTagBuilder SetupBuilder(IQueryParamElementTagBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(QueryType) && Enum.TryParse(QueryType, out InputControlType type))
            {
                builder.ParamType = type;
            }
            else
            {
                builder.ParamType = InputControlType.Text;
            }

            return builder;
        }
    }
}
