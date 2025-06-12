# Stage 1: Build and publish using .NET 9 SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app

# Copy only the project files first for optimized Docker layer caching
COPY ./src/BuldingBlock/BuldingBlock.csproj ./BuldingBlock/
COPY ./src/Services/Passenger/src/Passenger/Passenger.csproj ./Services/Passenger/src/Passenger/
COPY ./src/Services/Passenger/src/Passenger.Api/Passenger.Api.csproj ./Services/Passenger/src/Passenger.Api/

# Restore NuGet packages for the API project
RUN dotnet restore ./Services/Passenger/src/Passenger.Api/Passenger.Api.csproj

# Copy the entire source code after restore
COPY ./src/BuldingBlock ./BuldingBlock/
COPY ./src/Services/Passenger/src/Passenger/ ./Services/Passenger/src/Passenger/
COPY ./src/Services/Passenger/src/Passenger.Api/ ./Services/Passenger/src/Passenger.Api/

# Build the Passenger.Api project in Release mode (no restore needed)
RUN dotnet build -c Release --no-restore ./Services/Passenger/src/Passenger.Api/Passenger.Api.csproj

# Publish the app to the /app/publish directory
RUN dotnet publish -c Release --no-build -o /app/publish ./Services/Passenger/src/Passenger.Api/Passenger.Api.csproj

# Stage 2: Runtime image with .NET 9 ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Copy published files from build stage
COPY --from=builder /app/publish .

# Environment variables
ENV ASPNETCORE_URLS="https://*:5012;http://*:6012"
ENV ASPNETCORE_ENVIRONMENT=docker

ENTRYPOINT ["dotnet", "Passenger.Api.dll"]
