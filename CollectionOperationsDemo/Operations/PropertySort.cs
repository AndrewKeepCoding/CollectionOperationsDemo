using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Reflection;

namespace CollectionOperationsDemo.Operations;

public partial class PropertySort(string propertyName) : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private SortOrder _order;

    public SortOrder[] SortOrderOptions { get; } = Enum.GetValues<SortOrder>();

    public string PropertyName { get; } = propertyName;

    public PropertyInfo? PropertyInfo { get; } = typeof(User).GetProperty(propertyName);

    partial void OnIsEnabledChanged(bool oldValue, bool newValue)
    {
        System.Diagnostics.Debug.WriteLine($"IsEnabled changed from {oldValue} to {newValue}");
    }
}
