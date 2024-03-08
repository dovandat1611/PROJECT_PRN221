using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using PROJECT_PRN221.Models;
using static PROJECT_PRN221.Utils.Mail;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using PROJECT_PRN221.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PROJECT_PRN221;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(); 

// Add SignalR
builder.Services.AddSignalR();

// Add DbContext
builder.Services.AddDbContext<ProjectPrn221Context>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"))
);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddOptions();
var mailsettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsettings);
builder.Services.AddTransient<IEmailSender, SendMailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/NotFound";
        await next();
    }
});

app.MapGet("/NotFound", () =>
{
    var content = File.ReadAllText("Pages/NotFound.cshtml");
    return Results.Content(content, "text/html");
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

//// Map SignalR hub
app.MapHub<HubServer>("/hub");


app.Run();
