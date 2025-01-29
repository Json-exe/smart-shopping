using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SmartShopping.Client.Components.Dialogs;

public partial class MindesthaltbarkeitsdatumAngebenDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
}