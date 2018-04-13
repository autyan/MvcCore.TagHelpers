using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.QueryForm;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("query-form")]
    public class QueryFormTagHelper : TagHelper
    {
        private const string DefaultId = "queryForm";

        private readonly IList<IQueryParamElementTagBuilder> _queryParamElementTagBuilders = new List<IQueryParamElementTagBuilder>();

        private static readonly List<string> GlobalIgnoreProperties;

        private readonly List<string> _ignoreProperties = new List<string>();

        private readonly List<string> _includeProperties = new List<string>();

        [HtmlAttributeName("item-source")]
        public object Query { get; set; }

        [HtmlAttributeName("form-method")]
        public string FormMethod { get; set; } = "Post";

        [HtmlAttributeName("query-class")]
        public string QueryControlClass { get; set; }

        [HtmlAttributeName("query-ignore")]
        public string QueryIgnoreProperty {
            get => string.Join(",", _ignoreProperties);
            set => _ignoreProperties.AddRange(value.Split(','));
        }

        [HtmlAttributeName("query-include")]
        public string QueryIncludeProperty
        {
            get => string.Join(",", _includeProperties);
            set => _includeProperties.AddRange(value.Split(','));
        }

        static QueryFormTagHelper()
        {
            GlobalIgnoreProperties = new List<string>();
        }

        public static void AddIgnoreProperty(string properties)
        {
            GlobalIgnoreProperties.AddRange(properties.Split(',').Select(p => p?.Trim()));
        }

        public override void Init(TagHelperContext context)
        {
            foreach (var propertyInfo in Query.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if ((_ignoreProperties.Any(i => i == propertyInfo.Name) || GlobalIgnoreProperties.Any(i => i == propertyInfo.Name))
                    && _includeProperties.All(p => p != propertyInfo.Name))
                {
                    continue;
                }

                _queryParamElementTagBuilders.Add(new QueryParamTagBuilder(propertyInfo, Query));

            }
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items[typeof(IList<IQueryParamElementTagBuilder>)] = _queryParamElementTagBuilders;

            output.TagName = "div";
            var childContent = output.GetChildContentAsync().Result;
            output.Content.AppendHtml(childContent);

            output.Attributes.SetAttribute("class", "panel panel-default");
            var form = new TagBuilder("form");
            if (!form.Attributes.ContainsKey("Id") || !form.Attributes.ContainsKey("id"))
            {
                form.Attributes["id"] = DefaultId;
            }
            form.Attributes["class"] = "form-inline query-form";
            form.Attributes["method"] = FormMethod;

            var body = AppendQueryformInputs();
            form.InnerHtml.AppendHtml(body);

            var footer = AppendQueryButtonInputs();
            form.InnerHtml.AppendHtml(footer);

            output.Content.AppendHtml(form);
        }

        private TagBuilder AppendQueryformInputs()
        {
            var panelBody = new TagBuilder("div");
            panelBody.Attributes["class"] = "panel-body";

            foreach (var tagBuilder in _queryParamElementTagBuilders.OrderBy(q => q.ParamIndex))
            {
                var controlTag = tagBuilder.Build();
                if (QueryControlClass != null)
                {
                    controlTag.MergeAttribute("class", QueryControlClass);
                }
                panelBody.InnerHtml.AppendHtml(controlTag);
            }

            return panelBody;
        }

        private static TagBuilder AppendQueryButtonInputs()
        {
            var submit = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            submit.Attributes["value"] = MvcTaghelperStringLocalizer.Instance["Submit"];
            submit.Attributes["type"] = nameof(submit);
            submit.Attributes["class"] = "btn btn-primary pull-right";

            var reset = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            reset.Attributes["value"] = MvcTaghelperStringLocalizer.Instance["Reset"];
            reset.Attributes["type"] = nameof(reset);
            reset.Attributes["class"] = "btn btn-danger pull-right";

            var panelFooter = new TagBuilder("div");
            panelFooter.Attributes["class"] = "panel-footer";
            panelFooter.InnerHtml.AppendHtml(submit).AppendHtml(reset);

            return panelFooter;
        }
    }
}
