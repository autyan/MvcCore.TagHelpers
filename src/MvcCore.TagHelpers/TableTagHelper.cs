using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("table")]
    public class TableTagHelper : TagHelper
    {
        [HtmlAttributeName("table-form-id")]
        public string FromId { get; set; } = "queryForm";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            var childContent = output.GetChildContentAsync().Result;
            output.Content.AppendHtml(childContent);
        }
    }
}
