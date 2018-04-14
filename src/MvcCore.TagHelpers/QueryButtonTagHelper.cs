using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("query-button", ParentTag = "query-form")]
    public class QueryButtonTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var buttonCreateFuncs = (List<TagHelperContent>)context.Items[typeof(List<TagHelperContent>)];
            var childContent = await output.GetChildContentAsync();
            buttonCreateFuncs.Add(childContent);
            output.SuppressOutput();
        }
    }
}
