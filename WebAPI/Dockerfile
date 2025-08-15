# Use .NET SDK to build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY *.sln ./

# Copy all project files
COPY Core/Core.csproj Core/
COPY Repository/Repository.csproj Repository/
COPY Service/Service.csproj Service/
COPY VNPAY/VNPAY.csproj VNPAY/
COPY WebAPI/WebAPI.csproj WebAPI/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build & publish WebAPI project
WORKDIR /src/WebAPI
RUN dotnet publish -c Release -o /app/out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "WebAPI.dll"]