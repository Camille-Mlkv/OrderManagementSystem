FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ./Backend/Services/CartService/*.sln ./
COPY ./Backend/Services/CartService/CartService.Application/*.csproj CartService.Application/
COPY ./Backend/Services/CartService/CartService.Domain/*.csproj CartService.Domain/
COPY ./Backend/Services/CartService/CartService.Infrastructure/*.csproj CartService.Infrastructure/
COPY ./Backend/Services/CartService/CartService.API/*.csproj CartService.API/

RUN dotnet restore CartService.API/CartService.API.csproj

COPY ./Backend/Services/CartService/ /src/
WORKDIR /src/CartService.API
RUN dotnet build "./CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CartService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

EXPOSE 5004

ENTRYPOINT ["dotnet", "CartService.API.dll"]