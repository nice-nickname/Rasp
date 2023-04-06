using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Common.Helpers;

namespace UI.Common.Extensions;

public static class HtmlExtensions
{
    public static ControlsHtmlHelper<T> Controls<T>(this IHtmlHelper<T> helper) 
        where T : new()
    {
        return new ControlsHtmlHelper<T>(helper);
    }
}