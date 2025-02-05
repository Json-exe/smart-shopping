using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SmartShopping.Client.Components.Dialogs;

public partial class MindesthaltbarkeitsdatumAngebenDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    private DateTime? Mindeshaltbarkeitsdatum { get; set; }

    private void Submit()
    {
        if (!Mindeshaltbarkeitsdatum.HasValue) return;
        MudDialog.Close(DialogResult.Ok(Mindeshaltbarkeitsdatum.Value));
    }

    private void Cancel() => MudDialog.Cancel();
}