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

        return _html.When(JqueryBind.InitIncoding)
                    .Direct(settings.Items)
                    .OnBegin(dsl => dsl.Self().JQuery.Attr.Set(HtmlAttribute.Multiple).If(() => settings.IsMultiselect))
                    .OnSuccess(dsl => dsl.Self().JQuery.PlugIn("selectpicker",
                                                               new
                                                               {
                                                                       liveSearch = settings.IsSearchable,
                                                                       size = settings.Size,
                                                                       liveSearchPlaceholder = DataResources.SearchPlaceholder,
                                                                       noneResultText = DataResources.NothingFound,
                                                                       noneSelectedText = DataResources.NothingSelected,
                                                                       selectedTextFormat = $"count > {settings.MaxVisibleElements - 1}",
                                                                       countSelectedText = DataResources.SelectControl_CountSelectedText,
                                                                       actionsBox = settings.ActionBox,
                                                                       selectAllText = DataResources.SelectControl_SelectAll,
                                                                       deselectAllText = DataResources.SelectControl_DeselectAll,
                                                               }))
                    .OnComplete(dsl =>
                    {
                        dsl.Self().JQuery.Call("selectpicker", "deselectAll").If(() => !settings.Items.Any(s => s.Selected));
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

    public IHtmlContent DeleteConfirm(Action<ConfirmSettings> action)
    {
        var settings = new ConfirmSettings();
        action(settings);

        var disabled = settings.IsDisabled ? "disabled" : string.Empty;

        return _html.When(JqueryBind.Click)
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
                        dsl.WithSelf(r => r.Children().Expression(JqueryExpression.First))
                           .JQuery.Dom.Remove()
                           .If(() => Selector.Jquery.Self().Children().Last() == "done_all");
                        dsl.Self().Insert.Use(@"<span class=""spinner-border spinner-border-sm mt-1"" role=""status"" aria-hidden=""true""></span>"
                                            + DataResources.Deletion
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
                        dsl.Self().JQuery.Attr.Set("data-bs-title", DataResources.ConfirmDelete)
                           .If(() => Selector.Jquery.Self().Children().Last() == "done");

                        dsl.Self().JQuery.Call("tooltip");
                    })
                    .When(JqueryBind.None)
                    .Ajax(settings.Url)
                    .OnSuccess(dsl => settings.OnSuccess?.Invoke(dsl))
                    .AsHtmlAttributes(classes: $"btn btn-danger align-self-end h-fit {disabled}", id: settings.Id)
                    .ToButton(@$"<span class=""material-symbols-rounded"">delete</span>{DataResources.Delete}");
    }
}
