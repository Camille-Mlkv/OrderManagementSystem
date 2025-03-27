FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ./Backend/Services/CartService/CartService.GrpcServer/*.csproj CartService.GrpcServer/
COPY ./Backend/Services/CartService/CartService.Infrastructure/*.csproj CartService.Infrastructure/
COPY ./Backend/Services/CartService/CartService.Application/*.csproj CartService.Application/
COPY ./Backend/Services/CartService/CartService.Domain/*.csproj CartService.Domain/

RUN dotnet restore CartService.GrpcServer/CartService.GrpcServer.csproj

COPY ./Backend/Services/CartService/ /src/
WORKDIR /src/CartService.GrpcServer
RUN dotnet build "./CartService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CartService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

ARG CART_GRPCSERVER_PORT
EXPOSE ${CART_GRPCSERVER_PORT}

ENTRYPOINT ["dotnet", "CartService.GrpcServer.dll"]