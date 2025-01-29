using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SmartShopping.Client.Components;
using SmartShopping.Lib;
using SmartShopping.Lib.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddLib();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

await using var scope = app.Services.CreateAsyncScope();
try
{
    var db = await scope.ServiceProvider.GetRequiredService<IDbContextFactory<SmartShoppingDb>>()
        .CreateDbContextAsync();
    await db.Database.MigrateAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();