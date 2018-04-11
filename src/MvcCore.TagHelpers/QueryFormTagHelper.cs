using System.Collections.Generic;
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

        private readonly IList<QueryParamConfig> _queryParamConfigs = new List<QueryParamConfig>();

        [HtmlAttributeName("item-source")]
        public object Query { get; set; }

        [HtmlAttributeName("form-method")]
        public string FormMethod { get; set; } = "Post";

        [HtmlAttributeName("query-class")]
        public string QueryControlClass { get; set; }

        public override void Init(TagHelperContext context)
        {
            foreach (var propertyInfo in Query.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                _queryParamConfigs.Add(new QueryParamConfig(propertyInfo, Query));
            }
            base.Init(context);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            context.Items[typeof(IList<QueryParamConfig>)] = _queryParamConfigs;

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

            foreach (var queryParam in _queryParamConfigs)
            {
                var controlTag = new QueryParamTag(queryParam).Build();
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
            submit.Attributes["value"] = "Submit";
            submit.Attributes["type"] = "submit";
            submit.Attributes["class"] = "btn btn-primary pull-right";

            var reset = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            reset.Attributes["value"] = "Reset";
            reset.Attributes["type"] = "reset";
            reset.Attributes["class"] = "btn btn-danger pull-right";

            var panelFooter = new TagBuilder("div");
            panelFooter.Attributes["class"] = "panel-footer";
            panelFooter.InnerHtml.AppendHtml(submit).AppendHtml(reset);

            return panelFooter;
        }
    }
}
