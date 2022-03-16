#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Web/Orion.API/Orion.API.csproj", "src/Web/Orion.API/"]
COPY ["src/Infrastructure/Orion.SQLRepository/Orion.SQLRepository.csproj", "src/Infrastructure/Orion.SQLRepository/"]
COPY ["src/Application/Orion.Application/Orion.Application.csproj", "src/Application/Orion.Application/"]
COPY ["src/Domain/Orion.Domain/Orion.Domain.csproj", "src/Domain/Orion.Domain/"]
COPY ["src/Infrastructure/Orion.ThirdPartyServices/Orion.ThirdPartyServices.csproj", "src/Infrastructure/Orion.ThirdPartyServices/"]
COPY ["src/Infrastructure/Orion.CosmosRepository/Orion.CosmosRepository.csproj", "src/Infrastructure/Orion.CosmosRepository/"]
RUN dotnet restore "src/Web/Orion.API/Orion.API.csproj"
COPY . .
WORKDIR "/src/src/Web/Orion.API"
RUN dotnet build "Orion.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orion.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orion.API.dll"]