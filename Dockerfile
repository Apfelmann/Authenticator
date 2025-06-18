# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY AuthenticationBackend/*.csproj ./AuthenticationBackend/
RUN dotnet restore ./AuthenticationBackend/Authentication.csproj

COPY . .
WORKDIR /src/AuthenticationBackend
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 9001

ENV ASPNETCORE_URLS=http://+:9001
ENTRYPOINT ["dotnet", "Authentication.dll"]
