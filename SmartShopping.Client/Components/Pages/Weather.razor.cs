using Microsoft.JSInterop;
using MudBlazor;
using SmartShopping.Client.Components.Dialogs;
using SmartShopping.Lib.Api;

namespace SmartShopping.Client.Components.Pages;

public partial class Weather
{
    private string _resultBarCode = string.Empty;

    private readonly IJSRuntime JsRuntime;

    private readonly IDialogService DialogService;

    private readonly OpenFoodFactsApiClient OpenFoodFactsApiClient;

    private string testcode = "96130780";

    public Weather(IJSRuntime jsRuntime, IDialogService dialogService,
        OpenFoodFactsApiClient openFoodFactsApiClient)
    {
        JsRuntime = jsRuntime;
        DialogService = dialogService;
        OpenFoodFactsApiClient = openFoodFactsApiClient;
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
        await OpenDialogAsync();
    }

    private async Task GetTestProduct()
    {
        _resultBarCode = testcode;
        await InvokeAsync(StateHasChanged);
        await OpenDialogAsync();
    }

    private Task OpenDialogAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };

        return DialogService.ShowAsync<MindesthaltbarkeitsdatumAngebenDialog>("Mindeshaltbarkeitsdatum festlegen",
            options);
    }
}