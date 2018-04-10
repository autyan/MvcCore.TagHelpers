using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("query-form")]
    public class QueryFormTagHelper : TagHelper
    {
        private const string DefaultId = "queryForm";

        private PropertyInfo[] _queryParams;

        [HtmlAttributeName("item-source")]
        public object Query { get; set; }

        [HtmlAttributeName("form-method")]
        public string FormMethod { get; set; } = "Post";

        public override void Init(TagHelperContext context)
        {
            _queryParams = Query.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            var childContent = output.GetChildContentAsync().Result;
            output.Content.AppendHtml(childContent);

            output.Attributes.SetAttribute("class", "panel panel-default");
            AppendQueryformInputs(output);
        }

        private void AppendQueryformInputs(TagHelperOutput output)
        {
            var formbody = new TagBuilder("div");
            formbody.Attributes["class"] = "panel-body";
            var form = new TagBuilder("form");
            if (form.Attributes["Id"] == null && form.Attributes["id"] == null)
            {
                form.Attributes["id"] = DefaultId;
            }
            form.Attributes["class"] = "form-inline";
            form.Attributes["method"] = FormMethod;

            foreach (var queryParam in _queryParams)
            {
                
            }

            output.Content.AppendHtml(formbody);
        }
    }
}
