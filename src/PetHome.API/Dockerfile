# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080 


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetHome.API/PetHome.API.csproj", "src/PetHome.API/"]
COPY ["src/Accounts/PetHome.Accounts.API/PetHome.Accounts.API.csproj", "src/Accounts/PetHome.Accounts.API/"]
COPY ["src/Accounts/PetHome.Accounts.Application/PetHome.Accounts.Application.csproj", "src/Accounts/PetHome.Accounts.Application/"]
COPY ["src/Shared/PetHome.Core/PetHome.Core.csproj", "src/Shared/PetHome.Core/"]
COPY ["src/Accounts/PetHome.Accounts.Contracts/PetHome.Accounts.Contracts.csproj", "src/Accounts/PetHome.Accounts.Contracts/"]
COPY ["src/Accounts/PetHome.Accounts.Domain/PetHome.Accounts.Domain.csproj", "src/Accounts/PetHome.Accounts.Domain/"]
COPY ["src/Volunteers/PetHome.Volunteers.Domain/PetHome.Volunteers.Domain.csproj", "src/Volunteers/PetHome.Volunteers.Domain/"]
COPY ["src/Shared/PetHome.Framework/PetHome.Framework.csproj", "src/Shared/PetHome.Framework/"]
COPY ["src/Shared/PetHome.SharedKernel/PetHome.SharedKernel.csproj", "src/Shared/PetHome.SharedKernel/"]
COPY ["src/Species/PetHome.Species.Domain/PetHome.Species.Domain.csproj", "src/Species/PetHome.Species.Domain/"]
COPY ["src/Accounts/PetHome.Accounts.Infrastructure/PetHome.Accounts.Infrastructure.csproj", "src/Accounts/PetHome.Accounts.Infrastructure/"]
COPY ["src/Discussions/PetHome.Discussions.API/PetHome.Discussions.API.csproj", "src/Discussions/PetHome.Discussions.API/"]
COPY ["src/Discussions/PetHome.Discussions.Application/PetHome.Discussions.Application.csproj", "src/Discussions/PetHome.Discussions.Application/"]
COPY ["src/Discussions/PetHome.Discussions.Domain/PetHome.Discussions.Domain.csproj", "src/Discussions/PetHome.Discussions.Domain/"]
COPY ["src/Discussions/PetHome.Discussions.Contracts/PetHome.Discussions.Contracts.csproj", "src/Discussions/PetHome.Discussions.Contracts/"]
COPY ["src/Discussions/PetHome.Discussions.Infrastructure/PetHome.Discussions.Infrastructure.csproj", "src/Discussions/PetHome.Discussions.Infrastructure/"]
COPY ["src/Species/PetHome.Species.API/PetHome.Species.API.csproj", "src/Species/PetHome.Species.API/"]
COPY ["src/Species/PetHome.Species.Application/PetHome.Species.Application.csproj", "src/Species/PetHome.Species.Application/"]
COPY ["src/Species/PetHome.Species.Contracts/PetHome.Species.Contracts.csproj", "src/Species/PetHome.Species.Contracts/"]
COPY ["src/Species/PetHome.Species.Infrastructure/PetHome.Species.Infrastructure.csproj", "src/Species/PetHome.Species.Infrastructure/"]
COPY ["src/VolunteerRequests/PetHome.VolunteerRequests.API/PetHome.VolunteerRequests.API.csproj", "src/VolunteerRequests/PetHome.VolunteerRequests.API/"]
COPY ["src/VolunteerRequests/PetHome.VolunteerRequests.Application/PetHome.VolunteerRequests.Application.csproj", "src/VolunteerRequests/PetHome.VolunteerRequests.Application/"]
COPY ["src/VolunteerRequests/PetHome.VolunteerRequests.Contracts/PetHome.VolunteerRequests.Contracts.csproj", "src/VolunteerRequests/PetHome.VolunteerRequests.Contracts/"]
COPY ["src/Volunteers/PetHome.Volunteers.Contracts/PetHome.Volunteers.Contracts.csproj", "src/Volunteers/PetHome.Volunteers.Contracts/"]
COPY ["src/VolunteerRequests/PetHome.VolunteerRequests.Domain/PetHome.VolunteerRequests.Domain.csproj", "src/VolunteerRequests/PetHome.VolunteerRequests.Domain/"]
COPY ["src/VolunteerRequests/PetHome.VolunteerRequests.Infrastructure/PetHome.VolunteerRequests.Infrastructure.csproj", "src/VolunteerRequests/PetHome.VolunteerRequests.Infrastructure/"]
COPY ["src/Volunteers/PetHome.Volunteers.API/PetHome.Volunteers.API.csproj", "src/Volunteers/PetHome.Volunteers.API/"]
COPY ["src/Volunteers/PetHome.Volunteers.Application/PetHome.Volunteers.Application.csproj", "src/Volunteers/PetHome.Volunteers.Application/"]
COPY ["src/Volunteers/PetHome.Volunteers.Infrastructure/PetHome.Volunteers.Infrastructure.csproj", "src/Volunteers/PetHome.Volunteers.Infrastructure/"]
RUN dotnet restore "./src/PetHome.API/PetHome.API.csproj"
COPY . .
WORKDIR "/src/src/PetHome.API"
RUN dotnet build "./PetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetHome.API.dll"]