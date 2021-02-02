#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY test-sdk/test-sdk.csproj test-sdk/
RUN dotnet restore "test-sdk/test-sdk.csproj"
COPY . .
WORKDIR "/src/test-sdk"
RUN dotnet build "test-sdk.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test-sdk.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test-sdk.dll"]
