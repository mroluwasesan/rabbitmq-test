# Stage 1: Base Image for Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Install curl with caching
RUN apt-get update && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy solution and project files
COPY Telex.Messaging.sln ./
COPY Telex.Messaging.Api/Telex.Messaging.Api.csproj Telex.Messaging.Api/
COPY SharedModels/*.csproj SharedModels/
RUN dotnet restore Telex.Messaging.Api/Telex.Messaging.Api.csproj

# Copy everything else and build the application
COPY . .
WORKDIR /Telex.Messaging.Api
RUN dotnet build -c Release -o /app/build

# Stage 3: Publish Image
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Command to run the application
ENTRYPOINT ["dotnet", "Telex.Messaging.Api.dll"]
