﻿# -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore dependency sớm để tận dụng layer cache
COPY ["NotificationAPI.csproj", "./"]
RUN dotnet restore "NotificationAPI.csproj"

# Copy toàn bộ source code
COPY . . 

# Build project
RUN dotnet publish "NotificationAPI.csproj" -c Release -o /app/publish --no-restore

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Expose cổng mà Render sẽ map
EXPOSE 10000

# Copy từ stage build sang runtime
COPY --from=build /app/publish .

# Chạy app
ENTRYPOINT ["dotnet", "NotificationAPI.dll"]
