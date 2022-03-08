#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BlogCore/BlogCore.csproj", "src/BlogCore/"]
COPY ["src/BlogCore.Application/BlogCore.Application.csproj", "src/BlogCore.Application/"]
COPY ["src/BlogCore.Domain/BlogCore.Domain.csproj", "src/BlogCore.Domain/"]
COPY ["src/BlogCore.Repository/BlogCore.Repository.csproj", "src/BlogCore.Repository/"]
COPY ["src/BlogCore.IRepository/BlogCore.IRepository.csproj", "src/BlogCore.IRepository/"]
COPY ["src/BlogCore.Core/BlogCore.Core.csproj", "src/BlogCore.Core/"]
RUN dotnet restore "src/BlogCore/BlogCore.csproj"
COPY . .
WORKDIR "/src/src/BlogCore"
RUN dotnet build "BlogCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogCore.dll"]