#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UI/ProjectArchitecture.UI.Api/ProjectArchitecture.UI.Api.csproj", "UI/ProjectArchitecture.UI.Api/"]
RUN dotnet restore "UI/ProjectArchitecture.UI.Api/ProjectArchitecture.UI.Api.csproj"
COPY . .
WORKDIR "/src/UI/ProjectArchitecture.UI.Api"
RUN dotnet build "ProjectArchitecture.UI.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectArchitecture.UI.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectArchitecture.UI.Api.dll"]