FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
COPY *.sln .
COPY /app/api/*.csproj ./app/
COPY /app/api /app
WORKDIR /app
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIROMENT=Development
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Insania.Users.Api.dll"]
