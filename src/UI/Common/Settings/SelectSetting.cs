using Domain;

namespace UI;

public class SelectSetting
{
    public bool IsMultiselect { get; set; }

    public bool IsSearchable { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public int Size { get; set; } = 7;

    public IEnumerable<DropDownItem> Items { get; set; }
}
