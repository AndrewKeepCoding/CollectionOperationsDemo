using CommunityToolkit.Mvvm.ComponentModel;

namespace CollectionOperationsDemo.Operations;

public partial class PropertyFilter(string propertyName, string filter) : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string _filter = filter;

    [ObservableProperty]
    private bool _isCaseSensitive;

    [ObservableProperty]
    private bool _isExactMatch;

    public string PropertyName { get; } = propertyName;
}
