using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.Table;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("column", ParentTag = "table")]
    public class ColumnTemplateTagHelper : TagHelper
    {
        [HtmlAttributeName("col-name")]
        public string Name { get; set; }

        [HtmlAttributeName("col-header")]
        public string HeaderName { get; set; }

        [HtmlAttributeName("col-template")]
        public string Template { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var columnTemplates = (IList<ColumnTemplate>)context.Items[typeof(IList<ColumnTemplate>)];
            var column = columnTemplates.FirstOrDefault(col => col.ColumnName == Name);
            if (column != null)
            {
                column.Template = Template;
            }
            else
            {
                column = new ColumnTemplate
                {
                    ColumnName = Name,
                    HeaderName = string.IsNullOrWhiteSpace(HeaderName) ? Name : HeaderName,
                    Template = Template
                };
                columnTemplates.Add(column);
            }

            return Task.CompletedTask;
        }
    }
}
