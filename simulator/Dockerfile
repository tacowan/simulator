FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY simulator.csproj .
RUN dotnet restore simulator.csproj
COPY . .
WORKDIR /src
RUN dotnet build simulator.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish simulator.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "simulator.dll"]
