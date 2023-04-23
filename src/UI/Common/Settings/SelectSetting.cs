using Incoding.Core.ViewModel;

namespace UI;

public class SelectSetting
{
    public bool IsMultiselect { get; set; }

    public string Name { get; set; }

    public string Class { get; set; }

    public string Placeholder { get; set; }

    public IEnumerable<KeyValueVm> Items { get; set; }
}
