﻿using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;

namespace UI.Common.Extensions;

public static class ImlExtension
{
    public static HtmlString Index<T>(this ITemplateSyntax<T> each)
    {
        return each.ForRaw("@index");
    }
}