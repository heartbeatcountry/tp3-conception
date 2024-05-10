namespace CineQuebec.Windows.Records;

public class SelectedItemWrapper<T>(T item, bool isSelected)
{
    public T Item { get; init; } = item;
    public bool IsSelected { get; set; } = isSelected;
}