using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("row")]
    public class RowItemTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "tr";
            var childContent = await output.GetChildContentAsync();
            output.Content.AppendHtml(childContent);
        }
    }
}
