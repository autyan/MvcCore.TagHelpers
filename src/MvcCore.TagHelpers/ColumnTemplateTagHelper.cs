using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.Table;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("column", ParentTag = "mvc-table")]
    public class ColumnTemplateTagHelper : TagHelper
    {
        [HtmlAttributeName("col-name")]
        public string Name { get; set; }

        [HtmlAttributeName("col-header")]
        public string HeaderName { get; set; }

        [HtmlAttributeName("col-template")]
        public string Template { get; set; }

        [HtmlAttributeName("col-attributes")]
        public string ColumnAttributes { get; set; }

        [HtmlAttributeName("col-style")]
        public string ColumnStyle { get; set; }

        [HtmlAttributeName("col-action")]
        public Func<object, string> ColumnItemAction {get; set;}

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var columnTemplates = (IList<ColumnTemplate>)context.Items[typeof(IList<ColumnTemplate>)];
            var column = columnTemplates.FirstOrDefault(col => col.ColumnName == Name);
            if (column != null)
            {
                column.Template = Template;
                column.ColumnItemAction = ColumnItemAction;
                column.ColumnAttributes = ParseAttributes();
                column.HeaderAttributes.Add("style", ColumnStyle);
            }
            else
            {
                column = new ColumnTemplate
                {
                    ColumnName = Name,
                    HeaderName = string.IsNullOrWhiteSpace(HeaderName) ? Name : HeaderName,
                    Template = Template,
                    ColumnItemAction = ColumnItemAction,
                    ColumnAttributes = ParseAttributes(),
                };
                column.HeaderAttributes.Add("style", ColumnStyle);
                columnTemplates.Add(column);
            }

            return Task.CompletedTask;
        }

        private IDictionary<string, string> ParseAttributes()
        {
            var attributesDict = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(ColumnAttributes)) return attributesDict;

            foreach (var attr in ColumnAttributes.Split(';'))
            {
                var keyValue = attr.Split('=');
                if (keyValue.Length != 2) continue;

                attributesDict.Add(keyValue[0], keyValue[1]);
            }

            return attributesDict;
        }
    }
}
