using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcCore.TagHelpers.QueryForm
{
    public class QueryParamTagBuilder : IElementTagBuilder
    {
        private readonly QueryParamConfig _queryParam;

        public QueryParamTagBuilder(QueryParamConfig queryParam)
        {
            _queryParam = queryParam;
        }

        public TagBuilder Build()
        {
            var label = new TagBuilder("label");
            label.Attributes["for"] = _queryParam.ParamName;
            label.InnerHtml.Append(_queryParam.ParamDisplayName);
            var content = BuildControlTag();

            var inputGroupAddon = new TagBuilder("div");
            inputGroupAddon.Attributes["class"] = "input-group-addon";
            inputGroupAddon.InnerHtml.AppendHtml(label);

            var inputGroup = new TagBuilder("div");
            inputGroup.Attributes["class"] = "input-group";
            inputGroup.InnerHtml.AppendHtml(inputGroupAddon);
            inputGroup.InnerHtml.AppendHtml(content);

            var formGroup = new TagBuilder("div");
            formGroup.Attributes["class"] = "form-group";
            formGroup.InnerHtml.AppendHtml(inputGroup);

            return formGroup;
        }

        private TagBuilder BuildControlTag()
        {
            switch (_queryParam.ParamType)
            {
                case InputControlType.Select:
                    var selectTag = new TagBuilder("select");
                    selectTag.Attributes["name"] = _queryParam.ParamName;
                    selectTag.Attributes["class"] = "form-control";
                    selectTag.Attributes["placeholder"] = _queryParam.PlaceHolder;
                    if (!(_queryParam.ParamData is IEnumerable<SelectListItem> selectData))
                    {
                        throw new ArgumentException("Select control data source have to be type of (IEnumerable<SelectListItem>)");
                    }
                    else
                    {
                        var selected = _queryParam.ParamValue?.ToString();
                        foreach (var item in selectData)
                        {
                            var option = new TagBuilder("option");
                            option.Attributes["value"] = item.Value;
                            option.InnerHtml.Append(item.Text);
                            if (item.Value == selected || item.Text == selected)
                            {
                                option.Attributes[nameof(selected)] = nameof(selected);
                            }

                            selectTag.InnerHtml.AppendHtml(option);
                        }
                    }
                    return selectTag;
                default:
                    var controlTag = new TagBuilder("input");
                    controlTag.Attributes["name"] = _queryParam.ParamName;
                    controlTag.Attributes["class"] = "form-control";
                    controlTag.Attributes["placeholder"] = _queryParam.PlaceHolder;
                    controlTag.Attributes["type"] = Enum.GetName(typeof(InputControlType), _queryParam.ParamType);
                    controlTag.Attributes["value"] = _queryParam.ParamValue?.ToString();
                    return controlTag;
            }
        }
    }
}
