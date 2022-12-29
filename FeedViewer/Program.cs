// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Testing;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Be sure to add your "Syncfusion:LicenseKey" to UserSecrets
// See README.md for further details
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
    builder.Configuration["Syncfusion:LicenseKey"]);

builder.Services.AddSyncfusionBlazor();
builder.Services.AddRazorPages();
builder.Services.AddSignalR(
    e => e.MaximumReceiveMessageSize = 102400000);
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(new TestData());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();