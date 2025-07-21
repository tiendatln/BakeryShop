# 1) Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy các csproj
COPY AdminUI/AdminUI.csproj AdminUI/
COPY DTOs/DTOs.csproj DTOs/
COPY Service/Service.csproj Service/

# Restore
WORKDIR /src/AdminUI
RUN dotnet restore

# Copy source
COPY AdminUI/ AdminUI/
COPY DTOs/ DTOs/
COPY Service/ Service/

# Publish
WORKDIR /src/AdminUI
RUN dotnet publish AdminUI/AdminUI.csproj -c Release -o /app/publish

# 2) Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminUI.dll"]
