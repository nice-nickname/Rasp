using Domain.Api;

namespace UI;

public class SelectSetting
{
    public bool IsMultiselect { get; set; }

    public bool IsSearchable { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public IEnumerable<DropDownItem> Items { get; set; }
}
