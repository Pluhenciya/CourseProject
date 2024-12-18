using AutodorInfoSystem.Middlewares;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ExcelService>();
builder.Services.AddSingleton<HttpClientService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = Environment.GetEnvironmentVariable("JWTIssuer") ?? builder.Configuration["JWT:Issuer"],
        ValidAudience = Environment.GetEnvironmentVariable("JWTAudience") ?? builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKey") ?? builder.Configuration["JWT:Key"])
        ),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["A"];

            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token is valid.");
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});
var app = builder.Build();

app.UseMiddleware<HttpClientMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Index}/{id?}");

app.Run();
