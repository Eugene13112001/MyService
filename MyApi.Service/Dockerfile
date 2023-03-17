#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MyApi.Service/MyApi.Service.csproj", "MyApi.Service/"]
RUN dotnet restore "MyApi.Service/MyApi.Service.csproj"
COPY . .
WORKDIR "/src/MyApi.Service"
RUN dotnet build "MyApi.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyApi.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyApi.Service.dll"]