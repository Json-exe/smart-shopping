using SmartShopping.Lib.ViewModel;

namespace SmartShopping.Client.Components.Pages;

public partial class Counter : ViewModelComponent<ProductViewViewModel>
{
    public Counter(ProductViewViewModel viewModel) : base(viewModel)
    {
    }
}