using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Common.Helpers;

public class ControlsHtmlHelper<T>
{
    private readonly IHtmlHelper<T> _html;

    public ControlsHtmlHelper(IHtmlHelper<T> html)
    {
        this._html = html;
    }

    public class FormSettings
    {
        public string Url { get; set; }

        public string Name { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSave { get; set; }

        public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnError { get; set; }
    }

    public IDisposable Form(Action<FormSettings> action)
    {
        var settings = new FormSettings();
        action(settings);

        return _html.When(JqueryBind.Submit)
                    .StopPropagation()
                    .PreventDefault()
                    .Submit()
                    .OnError(dsl => settings.OnError?.Invoke(dsl))
                    .OnSuccess(dsl => settings.OnSave?.Invoke(dsl))
                    .AsHtmlAttributes(new
                    {
                        action = settings.Url,
                        name = settings.Name
                    })
                    .ToBeginTag(HtmlTag.Form);
    }
}