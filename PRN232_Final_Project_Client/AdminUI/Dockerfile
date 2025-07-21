# Base build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY AdminUI/AdminUI.csproj AdminUI/
COPY DTOs/DTOs.csproj DTOs/
COPY Service/Service.csproj Service/

# Restore
WORKDIR /src/AdminUI
RUN dotnet restore

# Copy toàn bộ source
WORKDIR /src
COPY . .

# Publish
WORKDIR /src/AdminUI
RUN dotnet publish -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminUI.dll"]
