using Incoding.Core.ViewModel;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
                    .Submit(o => o.Type = JqueryAjaxOptions.HttpVerbs.Post)
                    .OnError(dsl => settings.OnError?.Invoke(dsl))
                    .OnSuccess(dsl => settings.OnSave?.Invoke(dsl))
                    .OnComplete(dsl => dsl.Self().Form.Validation.Refresh())
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

        var selected = settings.Items.Where(s => s is { Selected: true }).ToArray();
        var selectedJson = JsonConvert.SerializeObject(selected.Select(s => s.Value.ToString()).ToArray());

        return _html.When(JqueryBind.InitIncoding)
                    .Direct(settings.Items)
                    .OnBegin(dsl => dsl.Self().JQuery.Attr.Set(HtmlAttribute.Multiple).If(() => settings.IsMultiselect))
                    .OnSuccess(dsl => dsl.Self().JQuery.PlugIn("selectpicker", settings.@params))
                    .OnComplete(dsl =>
                    {
                        dsl.Self().JQuery.Call("selectpickerval", selectedJson);
                        dsl.Self().Trigger.Change().If(() => selected.Any());

                        settings.OnInit?.Invoke(dsl);
                    })
                    .When(JqueryBind.Change)
                    .StopPropagation()
                    .OnSuccess(dsl => dsl.Self().JQuery.Call("triggerByTimeout", "none", settings.ChangeTimeout))
                    .When(JqueryBind.None)
                    .StopPropagation()
                    .OnSuccess(dsl => settings.OnChange?.Invoke(dsl))
                    .AsHtmlAttributes(new
                    {
                            name = settings.Name,
                            @class = settings.Class,
                            title = DataResources.NothingSelected
                    })
                    .ToTag(HtmlTag.Select, this._html.Partial("~/Views/Shared/Select/Select.cshtml", settings));
    }
}
