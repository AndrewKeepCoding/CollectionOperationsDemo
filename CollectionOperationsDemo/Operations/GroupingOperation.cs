using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CollectionOperationsDemo.Operations;

public partial class GroupingOperation : ObservableObject, IOperation
{
    [ObservableProperty]
    private bool _isEnabled;

    [ObservableProperty]
    private string _groupingPropertyName;

    public GroupingOperation()
    {
        GroupingProperties = typeof(User).GetProperties().Select(x => x.Name).ToArray();
        GroupingPropertyName = GroupingProperties.FirstOrDefault() ?? string.Empty;

        PropertyChanged += GroupingOperation_PropertyChanged;
    }

    public event EventHandler? ValueUpdated;

    public string Name { get; } = "Grouping";

    public string[] GroupingProperties { get; }

    public IEnumerable<object> Execute(IEnumerable<object> sourceItems)
    {
        if (IsEnabled is false)
        {
            return sourceItems;
        }

        var groupedItems = sourceItems.GroupBy(item => item.GetType().GetProperty(GroupingPropertyName)?.GetValue(item)).ToList();

        return groupedItems;
    }

    private void GroupingOperation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ValueUpdated?.Invoke(this, EventArgs.Empty);
    }
}
