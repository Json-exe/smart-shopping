using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SmartShopping.Client.Components.Dialogs;

public partial class MindesthaltbarkeitsdatumAngebenDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    public DateTime Mindeshaltbarkeitsdatum { get; set; }

    private void Submit()
    {
        MudDialog.Close(DialogResult.Ok(Mindeshaltbarkeitsdatum));
    }

    private void Cancel() => MudDialog.Cancel();
}