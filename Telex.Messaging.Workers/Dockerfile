# Stage 1: Base Image for Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development

# Stage 2: Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy solution and project files
COPY Telex.Messaging.sln ./
COPY Telex.Messaging.Workers/Telex.Messaging.Workers.csproj Telex.Messaging.Workers/
COPY SharedModels/*.csproj SharedModels/
RUN dotnet restore Telex.Messaging.Workers/Telex.Messaging.Workers.csproj

# Copy everything else and build the application
COPY . .
WORKDIR /Telex.Messaging.Workers
RUN dotnet build -c Release -o /app/build

# Stage 3: Publish Image
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage 4: Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Command to run the application
ENTRYPOINT ["dotnet", "Telex.Messaging.Workers.dll"]
