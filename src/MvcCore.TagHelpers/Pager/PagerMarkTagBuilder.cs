using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcCore.TagHelpers.Pager
{
    public class PagerMarkTagBuilder : IElementTagBuilder
    {
        public string Href { get; set; }

        public bool Disabled { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }

        public bool IsActive { get; set; }

        public int? PageIndex { get; set; }

        public TagBuilder Build()
        {
            var linkBuilder = new TagBuilder("a");
            linkBuilder.Attributes["aria-label"] = "Head";
            if (IsStart)
            {
                var startSpan = new TagBuilder("span");
                startSpan.Attributes["aria-hidden"] = "true";
                startSpan.InnerHtml.AppendHtml("&laquo;");
                linkBuilder.InnerHtml.AppendHtml(startSpan);
            }

            if (IsEnd)
            {
                var startSpan = new TagBuilder("span");
                startSpan.Attributes["aria-hidden"] = "true";
                startSpan.InnerHtml.AppendHtml("&raquo;");
                linkBuilder.InnerHtml.AppendHtml(startSpan);
            }

            linkBuilder.Attributes["href"] = Href;
            linkBuilder.InnerHtml.AppendHtml($"{PageIndex}");

            var pageliBuilder = new TagBuilder("li");
            if (Disabled)
            {
                pageliBuilder.Attributes["class"] = "disabled";
            }

            if (IsActive)
            {
                pageliBuilder.Attributes["class"] = "active";
            }
            pageliBuilder.InnerHtml.AppendHtml(linkBuilder);

            return pageliBuilder;
        }
    }
}
