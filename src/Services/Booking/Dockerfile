# Use .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder

WORKDIR /app

# Copy project files with correct paths and folder name "BuldingBlock"
COPY ./src/BuldingBlock/BuldingBlock.csproj ./BuldingBlock/
COPY ./src/Services/Booking/src/Booking/Booking.csproj ./Services/Booking/src/Booking/
COPY ./src/Services/Booking/src/Booking.Api/Booking.Api.csproj ./Services/Booking/src/Booking.Api/

# Restore NuGet packages for the API project
RUN dotnet restore ./Services/Booking/src/Booking.Api/Booking.Api.csproj

# Copy all source code to the container
COPY ./src/BuldingBlock ./BuldingBlock/
COPY ./src/Services/Booking/src/Booking/ ./Services/Booking/src/Booking/
COPY ./src/Services/Booking/src/Booking.Api/ ./Services/Booking/src/Booking.Api/

# Build project in Release configuration without restore (already done)
RUN dotnet build -c Release --no-restore ./Services/Booking/src/Booking.Api/Booking.Api.csproj

WORKDIR /app/Services/Booking/src/Booking.Api

# Publish project to 'out' folder without building (already built)
RUN dotnet publish -c Release --no-build -o out

# Use the runtime image for .NET 9
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

# Copy published output from the builder stage
COPY --from=builder /app/Services/Booking/src/Booking.Api/out .

# Environment variables for ASP.NET Core
ENV ASPNETCORE_URLS="https://*:5010;http://*:6010"
ENV ASPNETCORE_ENVIRONMENT=docker

ENTRYPOINT ["dotnet", "Booking.Api.dll"]
