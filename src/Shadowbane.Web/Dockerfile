FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Shadowbane.Web/Shadowbane.Web.csproj", "Shadowbane.Web/"]
RUN dotnet restore "src/Shadowbane.Web/Shadowbane.Web.csproj"
COPY . .
WORKDIR "/src/Shadowbane.Web"
RUN dotnet build "Shadowbane.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shadowbane.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shadowbane.Web.dll"]
