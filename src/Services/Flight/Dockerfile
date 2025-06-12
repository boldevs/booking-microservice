# Stage 1: Build and publish using .NET 9 SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app

# Copy only the project files first for optimized Docker caching
COPY ./src/BuldingBlock/BuldingBlock.csproj ./BuldingBlock/
COPY ./src/Services/Flight/src/Flight/Flight.csproj ./Services/Flight/src/Flight/
COPY ./src/Services/Flight/src/Flight.Api/Flight.Api.csproj ./Services/Flight/src/Flight.Api/

# Restore NuGet packages for the API project
RUN dotnet restore ./Services/Flight/src/Flight.Api/Flight.Api.csproj

# Copy the remaining source files into the container
COPY ./src/BuldingBlock ./BuldingBlock/
COPY ./src/Services/Flight/src/Flight/ ./Services/Flight/src/Flight/
COPY ./src/Services/Flight/src/Flight.Api/ ./Services/Flight/src/Flight.Api/

# Build the Flight.Api project in Release mode (no restore needed)
RUN dotnet build -c Release --no-restore ./Services/Flight/src/Flight.Api/Flight.Api.csproj

# Publish to the /app/publish directory
RUN dotnet publish -c Release --no-build -o /app/publish ./Services/Flight/src/Flight.Api/Flight.Api.csproj

# Stage 2: Use .NET 9 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Copy published files from the builder stage
COPY --from=builder /app/publish .

# Set ASP.NET Core environment variables
ENV ASPNETCORE_URLS="https://*:5003;http://*:5004"
ENV ASPNETCORE_ENVIRONMENT=docker

# Optionally expose ports
EXPOSE 5003 5004

# Run the application
ENTRYPOINT ["dotnet", "Flight.Api.dll"]
