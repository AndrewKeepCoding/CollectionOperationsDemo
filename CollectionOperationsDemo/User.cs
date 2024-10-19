using CommunityToolkit.Mvvm.ComponentModel;

namespace CollectionOperationsDemo;

public partial class User : ObservableObject
{
    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private int _age;

    [ObservableProperty]
    private string _country = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;
}
