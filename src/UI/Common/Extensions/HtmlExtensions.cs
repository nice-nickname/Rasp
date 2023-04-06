using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UI.Common.Helpers;

namespace UI.Common.Extensions;

public static class HtmlExtensions
{
    public static ControlsHtmlHelper<T> Controls<T>(this HtmlHelper<T> helper)
    {
        return new ControlsHtmlHelper<T>(helper);
    }
}