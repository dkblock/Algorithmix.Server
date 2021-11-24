#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Algorithmix.Server/Algorithmix.Api/Algorithmix.Api.csproj", "Algorithmix.Server/Algorithmix.Api/"]
COPY ["Algorithmix.Server/Algorithmix.Database/Algorithmix.Database.csproj", "Algorithmix.Server/Algorithmix.Database/"]
COPY ["Algorithmix.Server/Algorithmix.Common/Algorithmix.Common.csproj", "Algorithmix.Server/Algorithmix.Common/"]
COPY ["Algorithmix.Server/Algorithmix.Entities/Algorithmix.Entities.csproj", "Algorithmix.Server/Algorithmix.Entities/"]
COPY ["Algorithmix.Server/Algorithmix.Services/Algorithmix.Services.csproj", "Algorithmix.Server/Algorithmix.Services/"]
COPY ["Algorithmix.Server/Algorithmix.Mappers/Algorithmix.Mappers.csproj", "Algorithmix.Server/Algorithmix.Mappers/"]
COPY ["Algorithmix.Server/Algorithmix.Models/Algorithmix.Models.csproj", "Algorithmix.Server/Algorithmix.Models/"]
COPY ["Algorithmix.Server/Algorithmix.Identity/Algorithmix.Identity.csproj", "Algorithmix.Server/Algorithmix.Identity/"]
COPY ["Algorithmix.Server/Algorithmix.Repository/Algorithmix.Repository.csproj", "Algorithmix.Server/Algorithmix.Repository/"]
RUN dotnet restore "Algorithmix.Server/Algorithmix.Api/Algorithmix.Api.csproj"
COPY . .
WORKDIR "/src/Algorithmix.Server/Algorithmix.Api"
RUN dotnet build "Algorithmix.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Algorithmix.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Algorithmix.Api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Algorithmix.Api.dll