# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution
COPY Fast_Change_Backend.slnx ./

# Copy project files
COPY AIService/*.csproj AIService/
COPY Application/*.csproj Application/
COPY BuildingBlocks/*.csproj BuildingBlocks/
COPY Contracts/*.csproj Contracts/
COPY Core/*.csproj Core/
COPY Domain/*.csproj Domain/
COPY Infrastructure/*.csproj Infrastructure/
COPY Persistence/*.csproj Persistence/
COPY Resources/*.csproj Resources/
COPY TelegramBot/*.csproj TelegramBot/

RUN dotnet restore Fast_Change_Backend.slnx

# Copy source
COPY . .

RUN dotnet publish Core/Core.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Core.dll"]