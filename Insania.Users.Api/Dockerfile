# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release
COPY ["/api/Insania.Users/Insania.Users.Api/Insania.Users.Api.csproj", "Insania.Users.Api/"]
COPY ["/api/Insania.Users/Insania.Users.BusinessLogic/Insania.Users.BusinessLogic.csproj", "Insania.Users.BusinessLogic/"]
COPY ["/api/Insania.Users/Insania.Users.Contracts/Insania.Users.Contracts.csproj", "Insania.Users.Contracts/"]
COPY ["/api/Insania.Users/Insania.Users.Entities/Insania.Users.Entities.csproj", "Insania.Users.Entities/"]
COPY ["/api/Insania.Users/Insania.Users.Models/Insania.Users.Models.csproj", "Insania.Users.Models/"]
COPY ["/api/Insania.Users/Insania.Users.DataAccess/Insania.Users.DataAccess.csproj", "Insania.Users.DataAccess/"]
COPY ["/api/Insania.Users/Insania.Users.Database/Insania.Users.Database.csproj", "Insania.Users.Database/"]
COPY ["/api/Insania.Users/Insania.Users.Messages/Insania.Users.Messages.csproj", "Insania.Users.Messages/"]
COPY ["/api/Insania.Users/Insania.Users.Middleware/Insania.Users.Middleware.csproj", "Insania.Users.Middleware/"]
RUN dotnet restore "./Insania.Users.Api/Insania.Users.Api.csproj"
WORKDIR /src
COPY . .
WORKDIR "/src/Insania.Users.Api"
RUN dotnet build "Insania.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Insania.Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Insania.Users.Api.dll"]