# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

# Set working directory inside the container
WORKDIR /src

# Copy project files for NuGet restore (leverages Docker cache)
COPY ./src/BuldingBlock/BuldingBlock.csproj ./BuldingBlock/
COPY ./src/ApiGateway/src/ApiGateway.csproj ./ApiGateway/src/

# Restore NuGet packages for ApiGateway project
RUN dotnet restore ./ApiGateway/src/ApiGateway.csproj

# Copy remaining project files
COPY ./src/BuldingBlock ./BuldingBlock/
COPY ./src/ApiGateway/src ./ApiGateway/src/

# Build ApiGateway project in Release configuration (no restore needed)
RUN dotnet build -c Release --no-restore ./ApiGateway/src/ApiGateway.csproj

# Change working directory to ApiGateway's source for publishing
WORKDIR /src/ApiGateway/src

# Publish project to 'out' folder (no build needed)
RUN dotnet publish -c Release --no-build -o out

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Set working directory for the runtime image
WORKDIR /app

# Copy published output from builder stage to runtime image
COPY --from=builder /src/ApiGateway/src/out .

# Expose application ports
EXPOSE 80
EXPOSE 443

# Define the container entry point
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
