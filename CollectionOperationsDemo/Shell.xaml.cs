using Microsoft.UI.Xaml.Controls;

namespace CollectionOperationsDemo;

public sealed partial class Shell : Page
{
    public Shell()
    {
        InitializeComponent();
    }

    public ShellViewModel ViewModel { get; } = new();
}
