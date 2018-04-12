using System;
using System.Collections.Generic;

namespace MvcCore.TagHelpers.Table
{
    public class ColumnTemplate
    {
        public string HeaderName { get; set; }

        public IDictionary<string, string> HeaderAttributes { get; set; } = new Dictionary<string, string>();

        public string ColumnName { get; set; }

        public IDictionary<string, string> ColumnAttributes { get; set; }

        public string Template { get; set; }

        public Func<object, string> ColumnItemAction { get; set; }
    }
}
