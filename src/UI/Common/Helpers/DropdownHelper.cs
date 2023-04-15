﻿using Incoding.Web.Extensions;
using Incoding.Web.MvcContrib;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UI.Common.Helpers;

public partial class ControlsHtmlHelper<T>
{
    public class DropdownHelper<T>
    {
        private readonly IHtmlHelper<T> _html;

        public DropdownHelper(IHtmlHelper<T> html)
        {
            this._html = html;
        }

        public IHtmlContent Button(Action<DropdownItemSettings> action)
        {
            var settings = new DropdownItemSettings();
            action(settings);

            return this._html.When(JqueryBind.Click)
                       .OnSuccess(dsl =>
                       {
                           dsl.Window.Console.Log("r-debug", "dropdown button successfully clicked");
                           settings.OnSuccess?.Invoke(dsl);
                       })
                       .AsHtmlAttributes()
                       .ToTag(HtmlTag.Li, $@"<a class=""dropdown-item"" href=""#{settings.Href}"">{settings.Text}</a>");
        }

        public IHtmlContent Divider()
        {
            return @"<li><hr class=""dropdown-divider""></li>".ToMvcHtmlString();
        }

        public IHtmlContent List(Action<DropdownListSettings> action)
        {
            var settings = new DropdownListSettings();
            action(settings);

            var template = string.IsNullOrWhiteSpace(settings.CustomTemplate)
                    ? "~/Views/Shared/DropDown_Item_Tmpl.cshtml"
                    : settings.CustomTemplate;

            return this._html.When(JqueryBind.InitIncoding)
                       .Ajax(settings.Url)
                       .OnSuccess(dsl =>
                       {
                           dsl.Window.Console.Log("r-debug", "dropdown list successfully loaded");
                           dsl.Self().Insert.WithTemplateByView(template).Html();
                           settings.OnSuccess?.Invoke(dsl);
                       })
                       .When(JqueryBind.None)
                       .OnSuccess(dsl => settings.OnClick?.Invoke(dsl))
                       .AsHtmlAttributes()
                       .ToTag(HtmlTag.Ul);
        }

        public void SetTitle(IIncodingMetaLanguageCallbackBodyDsl dsl, Selector title)
        {
            dsl.WithSelf(s => s.Closest(p => p.EqualsAttribute("role", "dropdown"))
                               .Find(c => c.EqualsAttribute("role", "title")))
               .Insert.Use(title).Text();
        }
    }
}
