using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.Table;
using SmartFormat;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("mvc-table")]
    public class TableTagHelper : TagHelper
    {
        private RowItemCollection _rowItemCollection;

        private KeyValuePair<string, string>[] _columns;

        private IList<ColumnTemplate> _columnTemplates;

        [HtmlAttributeName("item-source")]
        public IEnumerable Items { get; set; }

        [HtmlAttributeName("table-columns")]
        public string Columns { get; set; }

        [HtmlAttributeName("column-ignore")]
        public string IgnoreColumns { get; set; }

        [HtmlAttributeName("table-form-id")]
        public string FromId { get; set; } = "queryForm";

        public override void Init(TagHelperContext context)
        {
            var colDefinitions = Columns?.Split(',');
            if (colDefinitions != null)
            {
                _columns = new KeyValuePair<string, string>[colDefinitions.Length];
                var index = 0;
                while (index < _columns.Length)
                {
                    var keyValue = colDefinitions[index].Split(':');
                    var column = keyValue[0];
                    var header = keyValue.Length == 1 ? keyValue[0] : keyValue[1];
                    _columns[index] = new KeyValuePair<string, string>(header, column);
                    index++;
                }
            }

            _rowItemCollection = new RowItemCollection(Items);
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            SetupDefaultColumnTemplate(context);

            var childContent = output.GetChildContentAsync().Result;
            output.Content.AppendHtml(childContent);

            AppendTableHeaders(output);

            AppenpTableBody(output);
        }

        private void SetupDefaultColumnTemplate(TagHelperContext context)
        {
            _columnTemplates = new List<ColumnTemplate>();
            var ignoreColumns = IgnoreColumns?.Split(',');
            if (_columns != null)
            {
                foreach (var pair in _columns)
                {
                    if (ignoreColumns != null && ignoreColumns.Contains(pair.Key)) continue;
                    _columnTemplates.Add(new ColumnTemplate
                    {
                        ColumnName = pair.Key,
                        HeaderName = pair.Value
                    });
                }
            }
            else
            {
                foreach (var propertyInfo in _rowItemCollection.ItemPropertyInfos)
                {
                    if (ignoreColumns != null && ignoreColumns.Contains(propertyInfo.Name)) continue;

                    var display = propertyInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    var displayName = propertyInfo.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;

                    var template = new ColumnTemplate
                    {
                        ColumnName = propertyInfo.Name,
                    };
                    if (display != null)
                    {
                        template.HeaderName = display.Name;
                    }
                    else if (displayName != null)
                    {
                        template.HeaderName = displayName.DisplayName;
                    }
                    else
                    {
                        template.HeaderName = propertyInfo.Name;
                    }

                    _columnTemplates.Add(template);
                }
            }

            context.Items[typeof(IList<ColumnTemplate>)] = _columnTemplates;
        }

        private void AppendTableHeaders(TagHelperOutput output)
        {
            var theader = new TagBuilder("thead");

            var headTr = new TagBuilder("tr");
            foreach (var column in _columnTemplates)
            {
                var head = new TagBuilder("td");
                head.InnerHtml.Append(column.HeaderName);
                head.MergeAttributes(column.HeaderAttributes);
                headTr.InnerHtml.AppendHtml(head);
            }

            theader.InnerHtml.AppendHtml(headTr);
            output.Content.AppendHtml(theader);
        }

        private void AppenpTableBody(TagHelperOutput output)
        {
            var tbody = new TagBuilder("tbody");
            foreach (var rowItem in _rowItemCollection.RowItems)
            {
                var bodyTr = new TagBuilder("tr");
                foreach (var column in _columnTemplates)
                {
                    var colTd = new TagBuilder("td");
                    colTd.InnerHtml.Append(!string.IsNullOrWhiteSpace(column.Template)
                        ? Smart.Format(column.Template, rowItem)
                        : rowItem.GetColumnValue(column.ColumnName).ToString());

                    colTd.MergeAttributes(column.ColumnAttributes);
                    bodyTr.InnerHtml.AppendHtml(colTd);
                }

                tbody.InnerHtml.AppendHtml(bodyTr);
            }

            output.Content.AppendHtml(tbody);
        }
    }
}
