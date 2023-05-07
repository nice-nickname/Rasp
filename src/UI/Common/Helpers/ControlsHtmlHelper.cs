using Incoding.Core.Extensions;
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

    public IHtmlContent ButtonConfirm(Action<ConfirmSettings> action)
    {
        var settings = new ConfirmSettings();
        action(settings);

        var disabled = settings.IsDisabled ? "disabled" : string.Empty;

        var button = _html.When(JqueryBind.Click)
                          .OnSuccess(dsl =>
                          {
                              dsl.Self().Trigger.None()
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done")
                                 .TimeOut(1500);

                              dsl.Self().Insert.Use(@"<span class=""material-symbols-rounded"">done_all</span>")
                                 .Append()
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done");
                              dsl.WithSelf(r => r.Children().Last().Prev())
                                 .JQuery.Dom.Remove()
                                 .If(() => Selector.Jquery.Self().Children().Last().Prev() == "done");
                              if (!string.IsNullOrWhiteSpace(settings.Icon))
                                  dsl.WithSelf(r => r.Children().Expression(JqueryExpression.First))
                                     .JQuery.Dom.Remove()
                                     .If(() => Selector.Jquery.Self().Children().Last() == "done_all");
                              dsl.Self().Insert.Use(@"<span class=""spinner-border spinner-border-sm mt-1"" role=""status"" aria-hidden=""true""></span>"
                                                  + settings.TextInProcess
                                                  + @"<span class=""material-symbols-rounded"">done_all</span>")
                                 .Html()
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done_all");
                              dsl.Self().JQuery.Attr.AddClass("disabled")
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done_all");

                              dsl.Self().Insert.Use(@"<span class=""material-symbols-rounded"">done</span>")
                                 .Append()
                                 .If(() => Selector.Jquery.Self().Children().Last() != "done"
                                        && Selector.Jquery.Self().Children().Last() != "done_all");

                              dsl.Self().JQuery.Attr.Set("data-bs-toggle", "tooltip")
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done");
                              dsl.Self().JQuery.Attr.Set("data-bs-placement", "top")
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done");
                              dsl.Self().JQuery.Attr.Set("data-bs-title", settings.TextConfirm)
                                 .If(() => Selector.Jquery.Self().Children().Last() == "done");

                              dsl.Self().JQuery.Call("tooltip");
                          })
                          .When(JqueryBind.None);

        if (!string.IsNullOrWhiteSpace(settings.Url))
            button.Ajax(settings.Url);

        var icon = !string.IsNullOrWhiteSpace(settings.Icon)
                ? @$"<span class=""material-symbols-rounded"">{settings.Icon}</span>"
                : string.Empty;

        return button.OnSuccess(dsl =>
                     {
                         settings.OnSuccess?.Invoke(dsl);

                         dsl.Self().Insert.Use(icon + settings.Text).Html().TimeOut(125);
                         dsl.Self().JQuery.Attr.RemoveClass("disabled").TimeOut(125);
                         dsl.Self().JQuery.Call("tooltip", "dispose").TimeOut(125);
                     })
                     .AsHtmlAttributes(classes: $"btn btn-{settings.Color.ToStringLower()} align-self-end h-fit {disabled} {settings.AdditionalClasses}", id: settings.Id)
                     .ToButton($"{icon}{settings.Text}");
    }
}
