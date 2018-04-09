using System.Collections.Generic;

namespace MvcCore.TagHelpers.Table
{
    public class ColumnTemplate
    {
        public string HeaderName { get; set; }

        public IDictionary<string, string> HeaderAttributes { get; set; }

        public string ColumnName { get; set; }

        public IDictionary<string, string> ColumnAttributes { get; set; }

        public string Template { get; set; }
    }
}
