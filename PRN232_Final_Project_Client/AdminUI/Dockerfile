# SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj riêng
COPY ./AdminUI.csproj ./AdminUI/
COPY ../DTOs/DTOs.csproj ./DTOs/
COPY ../Service/Service.csproj ./Service/

# Restore
WORKDIR /src/AdminUI
RUN dotnet restore

# Copy toàn bộ solution
COPY . ./AdminUI/
COPY ../DTOs/ ./DTOs/
COPY ../Service/ ./Service/

# Publish
WORKDIR /src/AdminUI
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AdminUI.dll"]
