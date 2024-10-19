using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CollectionOperationsDemo.Operations;

public partial class FilteringOperation : ObservableObject, IOperation
{
    [ObservableProperty]
    private bool _isEnabled;

    public FilteringOperation()
    {
        Filters = typeof(User).GetProperties().Select(x => new PropertyFilter(x.Name, string.Empty)).ToArray();

        PropertyChanged += FilteringOperation_PropertyChanged;

        foreach (var filter in Filters)
        {
            filter.PropertyChanged += FilteringOperation_PropertyChanged;
        }
    }

    public event EventHandler? ValueUpdated;

    public string Name { get; } = "Filtering";

    public PropertyFilter[] Filters { get; }

    public IEnumerable<object> Execute(IEnumerable<object> sourceItems)
    {
        if (IsEnabled is false)
        {
            return sourceItems;
        }

        var filteredItems = sourceItems;

        foreach (var filter in Filters)
        {
            filteredItems = FilterItems(filteredItems, filter);
        }

        return filteredItems;
    }

    private static IEnumerable<object> FilterItems(IEnumerable<object> sourceItems, PropertyFilter filter)
    {
        if (filter.IsEnabled is false ||
            string.IsNullOrWhiteSpace(filter.Filter) is true ||
            typeof(User).GetProperty(filter.PropertyName) is not { } property)
        {
            return sourceItems;
        }

        sourceItems = filter.IsExactMatch is true
            ? sourceItems.Where(item => property.GetValue(item)?.ToString()?
                .Equals(filter.Filter, filter.IsCaseSensitive is true
                    ? StringComparison.Ordinal
                    : StringComparison.OrdinalIgnoreCase) is true)
            : sourceItems.Where(item => property.GetValue(item)?.ToString()?
                .Contains(filter.Filter, filter.IsCaseSensitive is true
                    ? StringComparison.Ordinal
                    : StringComparison.OrdinalIgnoreCase) is true);
        return sourceItems;
    }

    private void FilteringOperation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ValueUpdated?.Invoke(this, EventArgs.Empty);
    }
}
