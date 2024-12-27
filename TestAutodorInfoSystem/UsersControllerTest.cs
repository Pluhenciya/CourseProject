using AutodorInfoApi.Models;
using AutodorInfoApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace AutodorInfoApi.Tests
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _configMock;

        public TokenServiceTests()
        {
            // Настройка мока конфигурации
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(config => config["JWT:Key"]).Returns("SuperSecretKey_JSM_The_Best_837412904361249126341789268041263489127346784263490"); // Updated key
            _configMock.Setup(config => config["JWT:Issuer"]).Returns("my_issuer");
            _configMock.Setup(config => config["JWT:Audience"]).Returns("my_audience");

            // Инициализация TokenService с моком конфигурации
            _tokenService = new TokenService(_configMock.Object);
        }


        [Fact]
        public void CreateToken_ReturnsValidToken_WhenUserIsAdmin()
        {
            // Arrange
            var user = new User
            {
                Login = "adminUser",
                IdUser = 1,
                Admin = new Admin() // Указываем, что это администратор
            };

            // Act
            var token = _tokenService.CreateToken(user);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SuperSecretKey_JSM_The_Best_837412904361249126341789268041263489127346784263490")),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            // Проверяем, что токен можно валидировать
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            Assert.NotNull(principal);
            Assert.Equal("adminUser", principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.Equal("1", principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal("Admin", principal.FindFirst(ClaimTypes.Role)?.Value);
        }

        [Fact]
        public void CreateToken_ReturnsValidToken_WhenUserIsProjecter()
        {
            // Arrange
            var user = new User
            {
                Login = "projecterUser",
                IdUser = 2,
                Projecter = new Projecter() // Указываем, что это проектировщик
            };

            // Act
            var token = _tokenService.CreateToken(user);

            // Assert
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SuperSecretKey_JSM_The_Best_837412904361249126341789268041263489127346784263490")),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            // Проверяем, что токен можно валидировать
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            Assert.NotNull(principal);
            Assert.Equal("projecterUser", principal.FindFirst(ClaimTypes.Name)?.Value);
            Assert.Equal("2", principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Assert.Equal("Projecter", principal.FindFirst(ClaimTypes.Role)?.Value);
        }
    }
}