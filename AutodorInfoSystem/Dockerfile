# ���������� ����������� ����� .NET SDK ��� ������ ����������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# �������� csproj � ��������������� �����������
COPY AutodorInfoSystem/*.csproj ./
RUN dotnet restore

# �������� ��� ����� � �������� ����������
COPY AutodorInfoSystem/. ./
RUN dotnet publish -c Release -o out

# ���������� ����������� ����� .NET Runtime ��� ������� ����������
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# ��������� ������� ��� ������� ����������
ENTRYPOINT ["dotnet", "AutodorInfoSystem.dll"]