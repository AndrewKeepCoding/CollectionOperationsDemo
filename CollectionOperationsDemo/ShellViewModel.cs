using Bogus;
using CollectionOperationsDemo.Operations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CollectionOperationsDemo;

public partial class ShellViewModel : ObservableObject
{
    private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    [ObservableProperty]
    private ObservableCollection<IOperation> _operations = [];

    [ObservableProperty]
    private IEnumerable<object> _operatedItems = [];

    [ObservableProperty]
    private IEnumerable<IGrouping<object, object>> _groupedItems = [];

    [ObservableProperty]
    private bool _runOnUpdates;

    [ObservableProperty]
    private IList<User> _originalItems = [];

    [ObservableProperty]
    private string _groupingItem = string.Empty;

    public ShellViewModel()
    {
        Operations.Add(new FilteringOperation());
        Operations.Add(new SortingOperation());
        Operations.Add(new GroupingOperation());

        foreach (var operation in Operations)
        {
            operation.ValueUpdated += Operation_ValueUpdated;
        }

        RunOnUpdates = true;
    }

    private static Task<List<User>> GenerateUsers(int totalCount, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            return new Faker<User>()
                .Rules((faker, user) =>
                {
                    user.FirstName = faker.Person.FirstName;
                    user.LastName = faker.Person.LastName;
                    user.Age = faker.Random.Int(0, 200);
                    user.Country = faker.Address.Country();
                    user.Email = faker.Person.Email;
                })
                .Generate(totalCount);
        },
        cancellationToken);
    }

    partial void OnRunOnUpdatesChanged(bool value)
    {
        if (value is false)
        {
            return;
        }

        _dispatcherQueue.TryEnqueue(async () => await RunOperations(CancellationToken.None));
    }

    private async void Operation_ValueUpdated(object? sender, EventArgs e)
    {
        await RunOperations(CancellationToken.None);
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RefreshItemsCount(int count, CancellationToken cancellationToken)
    {
        try
        {
            OriginalItems = await GenerateUsers(count, cancellationToken);
            await RunOperations(cancellationToken);
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task RunOperations(CancellationToken cancellationToken)
    {
        try
        {
            var operatedItems = await OperateItems(OriginalItems);
            OperatedItems = operatedItems is IEnumerable<IGrouping<object, object>>
                ? new CollectionViewSource()
                {
                    Source = operatedItems,
                    IsSourceGrouped = true,
                }.View
                : new ObservableCollection<object>(operatedItems);

            if (operatedItems is IEnumerable<IGrouping<object, object>> groupedItems)
            {
                GroupedItems = groupedItems;
            }
        }
        catch (TaskCanceledException)
        {
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    private Task<IEnumerable<object>> OperateItems(IEnumerable<User> source)
    {
        return Task.Run(() =>
        {
            IEnumerable<object> result = source;

            foreach (var operation in Operations)
            {
                result = operation.Execute(result);
            }

            return result;
        });
    }
}
