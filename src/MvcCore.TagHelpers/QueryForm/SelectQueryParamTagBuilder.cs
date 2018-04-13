using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MvcCore.TagHelpers.QueryForm
{
    public class SelectQueryParamTagBuilder : QueryParamTagBuilder
    {
        public string ValuePropertyName { get; set; }

        public string TextPropertyName { get; set; }

        private static readonly Dictionary<Type, IList<PropertyInfo>> SelectItemPropertyCache = new Dictionary<Type, IList<PropertyInfo>>();

        internal SelectQueryParamTagBuilder(IQueryParamElementTagBuilder builder) : base(builder)
        {

        }

        protected override TagBuilder BuildControlTag()
        {
            var selectTag = new TagBuilder("select");
            selectTag.Attributes["name"] = ParamName;
            selectTag.Attributes["class"] = "form-control";
            selectTag.Attributes["placeholder"] = PlaceHolder;
            var selected = ParamValue;
            if (ParamData is IEnumerable selectOptions)
            {
                foreach (var item in selectOptions)
                {
                    var option = new TagBuilder("option");
                    var keyValue = GetSelectOptionKeyValuePair(item);
                    option.Attributes["value"] = keyValue.Key;
                    option.InnerHtml.Append(keyValue.Value);
                    if (keyValue.Key == selected)
                    {
                        option.Attributes[nameof(selected)] = nameof(selected);
                    }

                    selectTag.InnerHtml.AppendHtml(option);
                }
            }
            else
            {
                throw new ArgumentException("Select control data source have to be type of (IEnumerable)");
            }
            return selectTag;
        }

        private static PropertyInfo GetProperty(Type type, string propertyName)
        {
            if (SelectItemPropertyCache.ContainsKey(type))
            {
                return SelectItemPropertyCache[type].First(p => p.Name == propertyName);
            }

            var properties = type.GetProperties();
            SelectItemPropertyCache[type] = properties;
            return properties.First(p => p.Name == propertyName);
        }

        private KeyValuePair<string, string> GetSelectOptionKeyValuePair(object item)
        {
            var key = GetProperty(item.GetType(), ValuePropertyName).GetValue(item)?.ToString();
            var value = GetProperty(item.GetType(), TextPropertyName).GetValue(item)?.ToString();
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
