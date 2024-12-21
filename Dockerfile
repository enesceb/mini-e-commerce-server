# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["1likte.API/1likte.API.csproj", "1likte.API/"]
COPY ["1likte.Core/1likte.Core.csproj", "1likte.Core/"]
COPY ["1likte.Data/1likte.Data.csproj", "1likte.Data/"]
COPY ["1likte.Model/1likte.Model.csproj", "1likte.Model/"]
RUN dotnet restore "1likte.API/1likte.API.csproj"

COPY . .
WORKDIR "/src/1likte.API"
RUN dotnet build "1likte.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "1likte.API.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "1likte.API.dll"]
