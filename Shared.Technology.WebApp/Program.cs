using Core.App;
using Core.App.Plugins.DataAccess;
using Core.Sys;
using Experts.Blogger.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddRazorPages();

builder
    .Services
    .AddCoreSystem()
    .AddCoreApplication(builder.Configuration, builder.Environment.IsDevelopment())
    .AddBlogger();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
} else
{
    app.UseDataBase();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
