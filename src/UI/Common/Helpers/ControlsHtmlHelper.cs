using Incoding.Core.ViewModel;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Resources;

namespace UI.Common.Helpers;

public partial class ControlsHtmlHelper<T>
{
    private readonly IHtmlHelper<T> _html;

    public ControlsHtmlHelper(IHtmlHelper<T> html)
    {
        this._html = html;
        this.Dropdown = new DropdownHelper<T>(this._html);
    }

    public DropdownHelper<T> Dropdown { get; set; }

    public IDisposable Form(Action<FormSettings> action)
    {
        var settings = new FormSettings();
        action(settings);

        return _html.When(JqueryBind.Submit)
                    .StopPropagation()
                    .PreventDefault()
                    .Submit()
                    .OnError(dsl => 
                    {
                        settings.OnError?.Invoke(dsl);
                        dsl.Self().Form.Validation.Refresh();
                    })
                    .OnSuccess(dsl => settings.OnSave?.Invoke(dsl))
                    .AsHtmlAttributes(new
                    {
                            action = settings.Url,
                            name = settings.Name,
                            @class = settings.Class
                    })
                    .ToBeginTag(HtmlTag.Form);
    }

    public IHtmlContent Select(Action<SelectSetting> action)
    {
        var settings = new SelectSetting();
        action(settings);

        return _html.When(JqueryBind.InitIncoding)
                    .Direct(settings.Items)
                    .OnBegin(dsl =>
                    {
                        dsl.Self().JQuery.Attr.Set(HtmlAttribute.Multiple).If(() => settings.IsMultiselect);
                        dsl.Self().JQuery.Attr.Set("data-live-search", settings.IsSearchable);
                        dsl.Self().JQuery.Attr.Set("data-live-search-placeholder", DataResources.SearchPlaceholder);
                        dsl.Self().JQuery.Attr.Set("data-none-result-text", DataResources.NothingFound);
                    })
                    .OnSuccess(dsl => dsl.Self().JQuery.PlugIn("selectpicker"))
                    .AsHtmlAttributes(new
                    {
                            name = settings.Name,
                            @class = settings.Class,
                            title = DataResources.NothingSelected
                    })
                    .ToTag(HtmlTag.Select, this._html.Partial("~/Views/Shared/Select/Select.cshtml", settings));
    }
}
