using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcCore.TagHelpers
{
    public interface IElementTagBuilder
    {
        TagBuilder Build();
    }
}
