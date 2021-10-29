#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UI/ProjectArchitecture.UI.Jobs/ProjectArchitecture.UI.Jobs.csproj", "UI/ProjectArchitecture.UI.Jobs/"]
RUN dotnet restore "UI/ProjectArchitecture.UI.Jobs/ProjectArchitecture.UI.Jobs.csproj"
COPY . .
WORKDIR "/src/UI/ProjectArchitecture.UI.Jobs"
RUN dotnet build "ProjectArchitecture.UI.Jobs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectArchitecture.UI.Jobs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectArchitecture.UI.Jobs.dll"]