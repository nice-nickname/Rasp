﻿namespace UI;

using Incoding.Web.MvcContrib;

public class DropdownListSettings
{
    public string Url { get; set; }

    public string CustomTemplate { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnSuccess { get; set; }
}