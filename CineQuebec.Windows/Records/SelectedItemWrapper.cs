namespace CineQuebec.Windows.Records;

public class SelectedItemWrapper<T>(T item, bool isSelected)
{
    public T Item => item;
    public bool IsSelected { get; set; } = isSelected;
}