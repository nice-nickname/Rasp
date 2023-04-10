namespace UI.Common.Extensions;

using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;

public static class ImlExtension
{
    public static HtmlString Index<T>(this ITemplateSyntax<T> each)
    {
        return each.ForRaw("@index");
    }
}
