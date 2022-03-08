#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["~/src/BlogCore.Application/BlogCore.Application.csproj", "./"]
COPY ["~/src/BlogCore.Domain/BlogCore.Domain.csproj", "."]
COPY ["~/src/BlogCore.Repository/BlogCore.Repository.csproj", "."]
COPY ["~/src/BlogCore.IRepository/BlogCore.IRepository.csproj", "."]
COPY ["~/src/BlogCore.Core/BlogCore.Core.csproj", "."]
COPY . .
RUN dotnet restore "BlogCore.csproj"
RUN dotnet build "BlogCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogCore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogCore.dll"]