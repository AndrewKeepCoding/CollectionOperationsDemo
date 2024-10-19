using System;
using System.Collections.Generic;

namespace CollectionOperationsDemo.Operations;

public interface IOperation
{
    event EventHandler? ValueUpdated;

    string Name { get; }

    bool IsEnabled { get; set; }

    IEnumerable<object> Execute(IEnumerable<object> source);
}
