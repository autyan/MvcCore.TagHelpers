using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MvcCore.TagHelpers.Pager;

namespace MvcCore.TagHelpers
{
    [HtmlTargetElement("mvc-pager")]
    public class PagerTaghelper : TagHelper
    {
        private long? _totalPage;

        private int? _pagerCount;

        private string _linkHref;

        private int? _pageIndex;

        private int? _skip;

        //private readonly StringBuilder _tageBuilder = new StringBuilder();

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

            var ulTag = new TagBuilder("ul");
            ulTag.Attributes["class"] = "pagination";

            //append firstPage button
            var startTag = HasPreviousPagers
                ? new PagerMarkTagBuilder
                {
                    Href = $"{LinkHref}skip=0&take={Take}",
                    IsStart = true
                }
                : new PagerMarkTagBuilder
                {
                    Href = "#",
                    IsStart = true,
                    Disabled = true
                };

            ulTag.InnerHtml.AppendHtml(startTag.Build());

            //append pagers before currentPage
            var beforeIndexCount = PageIndex - RenderPagerCount / 2;
            var startIndex = beforeIndexCount <= 1 ? 1 : beforeIndexCount;
            var currentIndex = startIndex;
            while (currentIndex < PageIndex)
            {
                ulTag.InnerHtml.AppendHtml(new PagerMarkTagBuilder
                {
                    Href = LinkHrefWithQueryParamters(currentIndex),
                    PageIndex = currentIndex
                }.Build());
                currentIndex += 1;
            }

            //append currentPager
            ulTag.InnerHtml.AppendHtml(new PagerMarkTagBuilder
            {
                Href = LinkHrefWithQueryParamters(PageIndex),
                PageIndex = PageIndex,
                IsActive = true
            }.Build());
            currentIndex += 1;

            //append pagers after currentPage
            var finalPage = (startIndex + RenderPagerCount) > TotalPage ? TotalPage : RenderPagerCount;
            while (currentIndex < finalPage)
            {
                ulTag.InnerHtml.AppendHtml(new PagerMarkTagBuilder
                {
                    Href = LinkHrefWithQueryParamters(currentIndex),
                    PageIndex = currentIndex
                }.Build());
                currentIndex += 1;
            }

            //append finalPage button
            var endTag = IsFinalPager
                ? new PagerMarkTagBuilder
                {
                    Href = "#",
                    IsEnd = true,
                    Disabled = true

                }
                : new PagerMarkTagBuilder
                {
                    Href = $"{LinkHref}skip={(TotalPage - 1) * Take}&take={Take}",
                    IsEnd = true
                };

            ulTag.InnerHtml.AppendHtml(endTag.Build());

            var navTag = new TagBuilder("nav");
            navTag.Attributes["style"] = "float:right";
            navTag.InnerHtml.AppendHtml(ulTag);

            output.Content.AppendHtml(navTag);
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
