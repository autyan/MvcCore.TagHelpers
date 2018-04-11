using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MvcCore.TagHelpers.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute display)
            {
                return display.Name;
            }

            if (propertyInfo.GetCustomAttribute(typeof(DisplayNameAttribute)) is DisplayNameAttribute displayName)
            {
                return displayName.DisplayName;
            }

            return string.Empty;
        }
    }
}
