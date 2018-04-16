﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("column", ParentTag = "row")]
    public class ColumnTemplateTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "td";
            var childContent = await output.GetChildContentAsync();
            output.Content.AppendHtml(childContent);
        }
    }
}
