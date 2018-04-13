namespace MvcCore.TagHelpers.QueryForm
{
    public interface IQueryParamElementTagBuilder : IElementTagBuilder
    {
        string ParamName { get; set; }

        string ParamDisplayName { get; set; }

        string PlaceHolder { get; set; }

        InputControlType ParamType { get; set; }

        string ParamValue { get; set; }

        int ParamIndex { get; set; }

        string Class { get; set; }
    }
}
