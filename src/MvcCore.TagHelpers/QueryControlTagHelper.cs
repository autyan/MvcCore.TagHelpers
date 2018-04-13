using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        protected IList<IQueryParamElementTagBuilder> QueryTagBuilders;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            QueryTagBuilders = (IList<IQueryParamElementTagBuilder>)context.Items[typeof(IList<IQueryParamElementTagBuilder>)];

            var me = QueryTagBuilders.FirstOrDefault(c => c.ParamName == QueryName);
            if (me == null) throw new NullReferenceException("Query Item Not Found");
            SetupBuilder(me);

            output.SuppressOutput();
        }

        protected virtual void SetupBuilder(IQueryParamElementTagBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(QueryType) && Enum.TryParse(QueryType, out InputControlType type))
            {
                builder.ParamType = type;
            }
            else
            {
                builder.ParamType = InputControlType.Text;
            }

            if (QueryControlObject is SelectList selectList)
            {
                builder.ParamData = selectList.Items;
            }
            else if (QueryControlObject is IEnumerable enumItems)
            {
                builder.ParamData = enumItems;
            }
            else
            {
                throw new ArgumentException("select control data only accept IEnumerable or SelectList");
            }
            builder.ParamIndex = Index;
        }
    }
}
