using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace UI.Common.Helpers;

public class ControlsHtmlHelper<T>
{
    private readonly HtmlHelper<T> _html;

    public ControlsHtmlHelper(HtmlHelper<T> html)
    {
        this._html = html;
    }
}