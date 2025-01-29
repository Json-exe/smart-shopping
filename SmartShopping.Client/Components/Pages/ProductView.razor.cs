using SmartShopping.Lib.ViewModel;

namespace SmartShopping.Client.Components.Pages;

public partial class ProductView : ViewModelComponent<ProductViewViewModel>
{
    public ProductView(ProductViewViewModel viewModel) : base(viewModel)
    {
    }

    protected override async Task OnInitializedAsync()
    {
        await ViewModel.LoadCommand.ExecuteAsync(null);
        await base.OnInitializedAsync();
    }
}