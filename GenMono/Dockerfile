FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base 
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GenMono/GenMono.csproj", "GenMono/"]
COPY ["DatabaseHelper/DatabaseHelper.csproj", "DatabaseHelper/"]
COPY ["Extension/Extension.csproj", "Extension/"]
COPY ["JwtManager/JwtManager.csproj", "JwtManager/"]
RUN dotnet restore "GenMono/GenMono.csproj"
COPY . .
WORKDIR "/src/GenMono"
RUN dotnet build "GenMono.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GenMono.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "GenMono.dll"]