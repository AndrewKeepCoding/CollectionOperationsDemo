using CollectionOperationsDemo.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CollectionOperationsDemo.Operations;

public partial class OperationItemTemplateSelector : DataTemplateSelector
{
    public StringToDataTemplateDictionary DataTemplates { get; } = [];

    public DataTemplate DefaultTemplate { get; set; } = new();

    protected override DataTemplate SelectTemplateCore(object item)
    {
        var result =
            item is IOperation operation &&
            DataTemplates.TryGetValue(operation.Name, out var template) is true
                ? template
                : DefaultTemplate;
        return result;
    }
}
