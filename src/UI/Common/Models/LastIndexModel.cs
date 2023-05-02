namespace UI.Common.Models;

public class LastIndexModel<T>
{
    public int LastIndex { get; set; }

    public List<T> Items { get; set; }
}
