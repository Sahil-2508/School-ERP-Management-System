# =========================
# Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the project file from the WebApplication-1 folder
COPY ["WebApplication-1/WebApplication-1.csproj", "WebApplication-1/"]

# Restore dependencies
RUN dotnet restore "WebApplication-1/WebApplication-1.csproj"

# Copy all source code
COPY . .

# Build from the project directory
WORKDIR "/src/WebApplication-1"

# Publish application
RUN dotnet publish "WebApplication-1.csproj" \
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

# Render will provide PORT
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

# Start application
ENTRYPOINT ["dotnet", "WebApplication-1.dll"]
