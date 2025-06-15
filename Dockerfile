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

# Port (ggf. anpassen, Standard: 5210 laut launchSettings.json)
EXPOSE 5210

ENV ASPNETCORE_URLS=http://+:5210
ENTRYPOINT ["dotnet", "Authentication.dll"]
