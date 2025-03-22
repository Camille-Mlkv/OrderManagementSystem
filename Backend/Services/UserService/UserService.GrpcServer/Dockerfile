FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ./Backend/Services/UserService/UserService.GrpcServer/*.csproj UserService.GrpcServer/
COPY ./Backend/Services/UserService/UserService.DataAccess/*.csproj UserService.DataAccess/

RUN dotnet restore UserService.GrpcServer/UserService.GrpcServer.csproj

COPY ./Backend/Services/UserService/ /src/
WORKDIR /src/UserService.GrpcServer
RUN dotnet build "./UserService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

EXPOSE 5009

ENTRYPOINT ["dotnet", "UserService.GrpcServer.dll"]