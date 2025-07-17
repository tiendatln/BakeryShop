# File Dockerfile (ở solution/)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY UserUI/UserUI.csproj UserUI/
COPY DTOs/DTOs.csproj DTOs/
COPY Service/Service.csproj Service/

RUN dotnet restore UserUI/UserUI.csproj

COPY . .
WORKDIR /src/UserUI
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserUI.dll"]
