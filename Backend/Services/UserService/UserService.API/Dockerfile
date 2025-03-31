FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ./Backend/Services/UserService/*.sln ./
COPY ./Backend/Services/UserService/UserService.BusinessLogic/*.csproj UserService.BusinessLogic/
COPY ./Backend/Services/UserService/UserService.DataAccess/*.csproj UserService.DataAccess/
COPY ./Backend/Services/UserService/UserService.API/*.csproj UserService.API/

RUN dotnet restore UserService.API/UserService.API.csproj

COPY ./Backend/Services/UserService/ /src/
WORKDIR /src/UserService.API
RUN dotnet build "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

EXPOSE 5012 5002

ENTRYPOINT ["dotnet", "UserService.API.dll"]