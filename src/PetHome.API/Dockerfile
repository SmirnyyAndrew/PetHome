#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetHome.API/PetHome.API.csproj", "src/PetHome.API/"]
COPY ["src/PetHome.Application/PetHome.Application.csproj", "src/PetHome.Application/"]
COPY ["src/PetHome.Domain/PetHome.Domain.csproj", "src/PetHome.Domain/"]
COPY ["src/PetHome.Infrastructure/PetHome.Infrastructure.csproj", "src/PetHome.Infrastructure/"]
RUN dotnet restore "./src/PetHome.API/PetHome.API.csproj"
COPY . .
WORKDIR "/src/src/PetHome.API"
RUN dotnet build "./PetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PetHome.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetHome.API.dll"]