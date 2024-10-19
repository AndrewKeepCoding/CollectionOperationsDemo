using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace CollectionOperationsDemo.Operations;

public partial class SortingOperation : ObservableObject, IOperation
{
    [ObservableProperty]
    private bool _isEnabled;

    public SortingOperation()
    {
        Sorts = new ObservableCollection<PropertySort>(typeof(User).GetProperties().Select(x => new PropertySort(x.Name)));
        Sorts.CollectionChanged += Sorts_CollectionChanged;

        PropertyChanged += SortingOperation_PropertyChanged;

        foreach (var itemProperty in Sorts)
        {
            itemProperty.PropertyChanged += SortingOperation_PropertyChanged;
        }
    }

    public event EventHandler? ValueUpdated;

    public string Name { get; } = "Sorting";

    public ObservableCollection<PropertySort> Sorts { get; }

    public IEnumerable<object> Execute(IEnumerable<object> sourceItems)
    {
        if (IsEnabled is false)
        {
            return sourceItems;
        }

        var sortedItems = sourceItems;

        foreach (var sort in Sorts)
        {
            sortedItems = SortItems(sortedItems, sort);
        }

        return sortedItems;
    }

    private static IEnumerable<object> SortItems(IEnumerable<object> sourceItems, PropertySort sort)
    {
        if (sort.IsEnabled is false ||
            sort.PropertyInfo is null)
        {
            return sourceItems;
        }

        return sort.Order is SortOrder.Asc
            ? sourceItems.OrderBy(item => sort.PropertyInfo.GetValue(item))
            : sourceItems.OrderByDescending(item => sort.PropertyInfo.GetValue(item));
    }

    private void Sorts_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ValueUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void SortingOperation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ValueUpdated?.Invoke(this, EventArgs.Empty);
    }
}
