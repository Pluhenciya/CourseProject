using AutodorInfoApi.Data;
using AutodorInfoApi.Models;
using AutodorInfoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AutodorContext>(options =>
{
    options.UseMySql(Environment.GetEnvironmentVariable("MySQLConnString") ?? builder.Configuration.GetConnectionString("DefaultConnection"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.4.0-mysql"));
});

builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<TokenService, TokenService>();

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

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AutodorContext>();
    if (!dbContext.Admins.Any())
    {
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Включите аутентификацию
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
