# Stage 1: Build and publish using .NET 9 SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app

# Copy only the .csproj files first to take advantage of Docker caching
COPY ./src/BuldingBlock/BuldingBlock.csproj ./BuldingBlock/
COPY ./src/Services/Identity/src/Identity/Identity.csproj ./Services/Identity/src/Identity/
COPY ./src/Services/Identity/src/Identity.Api/Identity.Api.csproj ./Services/Identity/src/Identity.Api/

# Restore NuGet packages
RUN dotnet restore ./Services/Identity/src/Identity.Api/Identity.Api.csproj

# Copy the full source code
COPY ./src/BuldingBlock ./BuldingBlock/
COPY ./src/Services/Identity/src/Identity/ ./Services/Identity/src/Identity/
COPY ./src/Services/Identity/src/Identity.Api/ ./Services/Identity/src/Identity.Api/

# Build the Identity.Api project in Release mode
RUN dotnet build -c Release --no-restore ./Services/Identity/src/Identity.Api/Identity.Api.csproj

# Publish the app to a separate folder for runtime image
RUN dotnet publish -c Release --no-build -o /app/publish ./Services/Identity/src/Identity.Api/Identity.Api.csproj

# Stage 2: Use .NET 9 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set working directory in runtime image
WORKDIR /app

# Copy published output from builder stage
COPY --from=builder /app/publish .

# Define environment variables
ENV ASPNETCORE_URLS="https://*:5005;http://*:6005"
ENV ASPNETCORE_ENVIRONMENT="docker"

# Optionally expose ports
EXPOSE 5005 6005

# Set entry point
ENTRYPOINT ["dotnet", "Identity.Api.dll"]
