FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR app/
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR src/
COPY ["LuckySix.Api/LuckySix.Api.sln", "LuckySix.Api/"]
COPY ["LuckySix.Api/LuckySix.Api.csproj", "LuckySix.Api/"]
COPY ["LuckySix.Core/LuckySix.Core.csproj", "LuckySix.Core/"]
COPY ["LuckySix.Data/LuckySix.Data.csproj", "LuckySix.Data/"]
RUN dotnet restore "LuckySix.Api/LuckySix.Api.csproj"
COPY . .
WORKDIR "/src/LuckySix.Api/"
RUN dotnet build "LuckySix.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LuckySix.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR app/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LuckySix.Api.dll"]