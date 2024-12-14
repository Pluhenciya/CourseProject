using AutodorInfoSystem.Data;
using AutodorInfoSystem.Models;
using AutodorInfoSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AutodorContext>(options =>
{
    options.UseMySql(Environment.GetEnvironmentVariable("MySQLConnString") ?? builder.Configuration.GetConnectionString("DefaultConnection"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.0-mysql"));
});

builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<ExcelService>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AutodorContext>();

    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
        var username = Environment.GetEnvironmentVariable("ADMIN_LOGIN");
        var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
        var createdUser = new User
        {
            Login = username,
            Password = BCrypt.Net.BCrypt.HashPassword(password)
        };
        var user = dbContext.Users.Find(username);
        if (user == null)
            return;
        dbContext.Users.Add(createdUser);
        dbContext.SaveChanges();
        dbContext.Admins.Add(new Admin
        {
            UsersIdUser = user.IdUser
        });
        dbContext.SaveChanges();
    }
}

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
