# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY Be3.Domain/Be3.Domain.csproj Be3.Domain/
COPY Be3.Infrastructure/Be3.Infrastructure.csproj Be3.Infrastructure/
COPY Be3.Application/Be3.Application.csproj Be3.Application/
COPY Be3.Api/Be3.Api.csproj Be3.Api/

# Restore dependencies (restore each project individually to avoid test project)
RUN dotnet restore Be3.Domain/Be3.Domain.csproj && \
    dotnet restore Be3.Infrastructure/Be3.Infrastructure.csproj && \
    dotnet restore Be3.Application/Be3.Application.csproj && \
    dotnet restore Be3.Api/Be3.Api.csproj

# Copy all source files
COPY Be3.Api/ Be3.Api/
COPY Be3.Application/ Be3.Application/
COPY Be3.Domain/ Be3.Domain/
COPY Be3.Infrastructure/ Be3.Infrastructure/

# Build the application
WORKDIR /src/Be3.Api
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Be3.Api.dll"]

