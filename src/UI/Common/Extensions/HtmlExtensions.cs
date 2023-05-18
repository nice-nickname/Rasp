using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Common.Helpers;

namespace UI.Common.Extensions;

public static class Extension
{
    public static T? GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
    {
        var type = enumVal.GetType();
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
        return (attributes.Length > 0) ? (T)attributes[0] : null;
    }
}

public static class HtmlExtensions
{
    public static ControlsHtmlHelper<T> Controls<T>(this IHtmlHelper<T> helper)
    {
        return new ControlsHtmlHelper<T>(helper);
    }
}
