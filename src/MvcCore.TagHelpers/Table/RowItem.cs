using System;
using System.Linq;

namespace MvcCore.TagHelpers.Table
{
    internal class RowItem
    {
        private readonly object _item;

        private readonly RowItemCollection _collection;

        internal RowItem(object item, RowItemCollection collection)
        {
            _item = item;
            _collection = collection;
        }

        internal object GetColumnValue(string columnName)
        {
            var property = _collection.ItemPropertyInfos.FirstOrDefault(prop => prop.Name == columnName);
            if (property == null) throw new InvalidOperationException("column not found in source items");

            return property.GetValue(_item);
        }
    }
}
