FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ./Backend/APIGateway/APIGateway.sln ./
COPY ./Backend/APIGateway/APIGateway/*.csproj APIGateway/

RUN dotnet restore APIGateway/APIGateway.csproj

COPY ./Backend/APIGateway/ /src/
WORKDIR /src/APIGateway
RUN dotnet build "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

ARG APIGATEWAY_PORT
EXPOSE ${APIGATEWAY_PORT}

ENTRYPOINT ["dotnet", "APIGateway.dll"]