using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SmartShopping.Lib.Database;
using SmartShopping.Lib.Database.Models;

namespace SmartShopping.Lib.ViewModel;

public sealed partial class ProductViewViewModel : BaseViewModel
{
    private readonly IDbContextFactory<SmartShoppingDb> _contextFactory;

    public ProductViewViewModel(IDbContextFactory<SmartShoppingDb> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    [ObservableProperty] private List<Product> _products = [];
    
    [RelayCommand]
    private async Task Load()
    {
        await using var db = await _contextFactory.CreateDbContextAsync();
        Products = await db.Products.ToListAsync();
    }
}