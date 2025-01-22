using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BarcodeScannerLiveApp.Components.Dialogs;

public partial class MindesthaltbarkeitsdatumAngebenDialog(MudDialogInstance mudDialog) : ComponentBase
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = mudDialog;

    public DateTime Mindeshaltbarkeitsdatum { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}