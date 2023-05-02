using Domain;
using Incoding.Web.MvcContrib;
using Resources;

namespace UI;

public class SelectSetting
{
    public bool IsMultiselect { get; set; }

    public bool ActionBox { get; set; }

    public bool IsSearchable { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Class { get; set; } = string.Empty;

    public int Size { get; set; } = 7;

    public int MaxVisibleElements { get; set; } = 3;

    public int ChangeTimeout { get; set; } = 0;

    public IEnumerable<DropDownItem> Items { get; set; } = new List<DropDownItem>();

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnInit { get; set; }

    public Action<IIncodingMetaLanguageCallbackBodyDsl>? OnChange { get; set; }

    public object @params => 
            new
            {
                    liveSearch = this.IsSearchable,
                    size = this.Size,
                    liveSearchPlaceholder = DataResources.SearchPlaceholder,
                    noneResultText = DataResources.NothingFound,
                    noneSelectedText = DataResources.NothingSelected,
                    selectedTextFormat = $"count > {this.MaxVisibleElements - 1}",
                    countSelectedText = DataResources.SelectControl_CountSelectedText,
                    actionsBox = this.ActionBox,
                    selectAllText = DataResources.SelectControl_SelectAll,
                    deselectAllText = DataResources.SelectControl_DeselectAll,
            };
}
