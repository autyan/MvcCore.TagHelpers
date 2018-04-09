using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MvcCore.TagHelpers.Table
{
    internal class RowItemCollection
    {
        internal Type ItemType { get; }

        internal PropertyInfo[] ItemPropertyInfos { get; }

        internal IList<RowItem> RowItems { get; }

        internal RowItemCollection(IEnumerable items)
        {
            RowItems = new List<RowItem>();
            foreach (var item in items)
            {
                RowItems.Add(new RowItem(item, this));
            }
            ItemType = items.GetType().GenericTypeArguments[0];
            ItemPropertyInfos = ItemType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
