#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ParteiWebService/ParteiWebService.csproj", "ParteiWebService/"]
COPY ["ParteiWebService/cloudbobdb.db", "ParteiWebService/"]
COPY ["ParteiWebService/cloudbobdb.db", "app/build"]
COPY ["ParteiWebService/cloudbobdb.db", "app/publish"]
COPY ["ParteiWebService/cloudbobdb.db", "app/"]

RUN dotnet restore "ParteiWebService/ParteiWebService.csproj"
COPY . .
WORKDIR "/src/ParteiWebService"
RUN dotnet build "ParteiWebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParteiWebService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ParteiWebService.dll"]