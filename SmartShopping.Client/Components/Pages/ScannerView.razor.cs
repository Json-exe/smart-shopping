using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using SmartShopping.Client.Components.Dialogs;
using SmartShopping.Lib.Api;
using SmartShopping.Lib.Database;
using SmartShopping.Lib.Models;
using Product = SmartShopping.Lib.Database.Models.Product;

namespace SmartShopping.Client.Components.Pages;

public partial class ScannerView
{
    private string _resultBarCode = string.Empty;

    private readonly IJSRuntime JsRuntime;

    private readonly IDialogService DialogService;

    private readonly OpenFoodFactsApiClient OpenFoodFactsApiClient;

    private string testcode = "4104450004048";

    private IDbContextFactory<SmartShoppingDb> _dbContextFactory;

    public ScannerView(IJSRuntime jsRuntime, IDialogService dialogService,
        OpenFoodFactsApiClient openFoodFactsApiClient, IDbContextFactory<SmartShoppingDb> dbContextFactory)
    {
        JsRuntime = jsRuntime;
        DialogService = dialogService;
        OpenFoodFactsApiClient = openFoodFactsApiClient;
        _dbContextFactory = dbContextFactory;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JsRuntime.InvokeVoidAsync("enumerateCameras");
        }
    }

    private async Task StartScanning()
    {
        await JsRuntime.InvokeVoidAsync("startCamera", DotNetObjectReference.Create(this));
    }

    private async Task StopScanning()
    {
        await JsRuntime.InvokeVoidAsync("stopCamera");
    }

    [JSInvokable]
    public async Task GetResult(string result)
    {
        _resultBarCode = result;
        await InvokeAsync(StateHasChanged);
        var product = await OpenFoodFactsApiClient.GetProductInformation(_resultBarCode);
        await OpenDialogAsync(product);
    }

    private async Task GetTestProduct()
    {
        _resultBarCode = testcode;
        await InvokeAsync(StateHasChanged);
        var product = await OpenFoodFactsApiClient.GetProductInformation(_resultBarCode);
        await OpenDialogAsync(product);
    }

    private async Task OpenDialogAsync(BaseProduct product)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        var dialog = await DialogService.ShowAsync<MindesthaltbarkeitsdatumAngebenDialog>(
            "Mindeshaltbarkeitsdatum festlegen",
            options);

        var result = await dialog.Result;

        if (result is { Canceled: false, Data: DateTime Mindesthaltbarkeitsdatum })
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();

            db.Products.Add(new Product
            {
                Barcode = product.Code,
                ExpirationDate = Mindesthaltbarkeitsdatum,
                Name = product.Product.ProductName,
                Nutriscore = product.Product.NutriscoreGrade
            });

            await db.SaveChangesAsync();
        }
    }
}