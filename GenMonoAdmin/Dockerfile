FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GenMonoAdmin/GenMonoAdmin.csproj", "GenMonoAdmin/"]
COPY ["DatabaseHelper/DatabaseHelper.csproj", "DatabaseHelper/"]
COPY ["Extension/Extension.csproj", "Extension/"]
RUN dotnet restore "GenMonoAdmin/GenMonoAdmin.csproj"
COPY . .
WORKDIR "/src/GenMonoAdmin"
RUN dotnet build "GenMonoAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenMonoAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "GenMonoAdmin.dll"]
