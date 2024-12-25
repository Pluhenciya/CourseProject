# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем csproj и восстанавливаем зависимости
COPY ./*.csproj ./
RUN dotnet restore

# Копируем все файлы и собираем приложение
COPY ./ ./
WORKDIR /source
RUN dotnet publish -c Release -o /app --no-restore


# Используем официальный образ .NET Runtime для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app ./
ENTRYPOINT ["dotnet", "AutodorInfoApi.dll"]
