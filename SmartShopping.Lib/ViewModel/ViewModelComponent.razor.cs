using System.ComponentModel;
using Microsoft.AspNetCore.Components;

namespace SmartShopping.Lib.ViewModel;

public class ViewModelComponent<TViewModel> : ComponentBase, IDisposable where TViewModel : BaseViewModel
{
    protected readonly TViewModel ViewModel;

    public ViewModelComponent(TViewModel viewModel)
    {
        ViewModel = viewModel;
        viewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
        ViewModel.Dispose();
    }
}