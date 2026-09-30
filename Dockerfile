# =========================
# Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file
COPY ["WebApplication_1.csproj", "./"]

# Restore dependencies
RUN dotnet restore "WebApplication_1.csproj"

# Copy source code
COPY . .

# Build and publish
RUN dotnet publish "WebApplication_1.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# =========================
# Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Render uses port 10000
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

# Start MVC application
ENTRYPOINT ["dotnet", "WebApplication_1.dll"]
