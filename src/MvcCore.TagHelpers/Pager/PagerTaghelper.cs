using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MvcCore.TagHelpers.Pager
{
    [HtmlTargetElement("mvc-pager")]
    public class PagerTaghelper : TagHelper
    {
        private long? _totalPage;

        private int? _pagerCount;

        private string _linkHref;

        private int? _pageIndex;

        private int? _skip;

        private readonly StringBuilder _tageBuilder = new StringBuilder();

        private readonly HttpContext _httpContext;

        [HtmlAttributeName("pager-skip")]
        public int? Skip
        {
            get => _skip ?? (_skip = 0);
            set => _skip = value;
        }

        [HtmlAttributeName("pager-take")]
        public int? Take { get; set; }

        [HtmlAttributeName("pager-totalCount")]
        public long? TotalCount { get; set; }

        [HtmlAttributeName("pager-width")]
        public double Width { get; set; } = 238;
        

        public PagerTaghelper(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        public int RenderPagerCount
        {
            get
            {
                if (_pagerCount != null) return _pagerCount.Value;
                _pagerCount = Convert.ToInt32((Width - 68) / 34.0);

                return _pagerCount.Value;
            }
        }

        private long? TotalPage
        {
            get
            {
                if (_totalPage != null) return _totalPage.Value;
                var maxItempage = TotalCount / Take;
                if (TotalCount % Take > 0)
                {
                    maxItempage += 1;
                }

                _totalPage = maxItempage;
                return _totalPage;
            }
        }

        private int? PageIndex => _pageIndex ?? (_pageIndex = Skip / Take + 1);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            //append nav element
            _tageBuilder.Append("<nav style=\"float:right;\"><ul class=\"pagination\">");

            //append firstPage button
            _tageBuilder.Append(HasPreviousPagers
                ? $"<li><a href=\"{LinkHref}skip=0&take={Take}\" aria-label=\"Head\"><span aria-hidden=\"true\">&laquo;</span></a></li>"
                : "<li class=\"disabled\"><a href=\"#\" aria-label=\"Head\"><span aria-hidden=\"true\">&laquo;</span></a></li>");

            //append pagers before currentPage
            var beforeIndexCount = PageIndex - RenderPagerCount / 2;
            var startIndex = beforeIndexCount <= 1 ? 1 : beforeIndexCount;
            var currentIndex = startIndex;
            while (currentIndex < PageIndex)
            {
                _tageBuilder.Append($"<li><a href=\"{LinkHrefWithQueryParamters(currentIndex)}\">{currentIndex}</a></li>");
                currentIndex += 1;
            }

            //append currentPager
            _tageBuilder.Append($"<li class=\"active\"><a href=\"{LinkHrefWithQueryParamters(PageIndex)}\">{PageIndex}</a></li>");
            currentIndex += 1;

            //append pagers after currentPage
            var finalPage = (startIndex + RenderPagerCount) > TotalPage ? TotalPage : RenderPagerCount;
            while (currentIndex < finalPage)
            {
                _tageBuilder.Append($"<li><a href=\"{LinkHrefWithQueryParamters(currentIndex)}\">{currentIndex}</a></li>");
                currentIndex += 1;
            }

            _tageBuilder.Append(IsFinalPager
                ? "<li class=\"disabled\"><a href=\"#\" aria-label=\"End\"><span aria-hidden=\"true\">&raquo;</span></a></li>"
                : $"<li href=\"{LinkHref}skip={(TotalPage - 1) * Take}&take={Take}\"><a href=\"#\" aria-label=\"End\"><span aria-hidden=\"true\">&raquo;</span></a></li>");

            _tageBuilder.Append("</ul></nav>");

            output.Content.AppendHtml(_tageBuilder.ToString());
        }

        private bool HasPreviousPagers => PageIndex > 1;

        private bool IsFinalPager => PageIndex == TotalPage;

        private string LinkHref
        {
            get
            {
                if (_linkHref == null)
                {
                    var builder = new StringBuilder();
                    builder.Append(_httpContext.Request.PathBase).Append("?");
                    foreach (var query in _httpContext.Request.Query)
                    {
                        if (!string.Equals(query.Key.ToLower(), nameof(Skip).ToLower(), StringComparison.OrdinalIgnoreCase)
                            && !string.Equals(query.Key.ToLower(), nameof(Take).ToLower(), StringComparison.OrdinalIgnoreCase))
                        {
                            builder.Append(query.Key).Append("=").Append(query.Value).Append("&");
                        }
                    }

                    _linkHref = builder.ToString();
                }

                return _linkHref;
            }
        }

        private string LinkHrefWithQueryParamters(int? index) => $"{LinkHref}skip={(index - 1) * Take}&take={Take}";
    }
}
