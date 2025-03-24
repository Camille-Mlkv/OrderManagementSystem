FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src

COPY ./Backend/Services/MealService/MealService.GrpcServer/*.csproj MealService.GrpcServer/
COPY ./Backend/Services/MealService/MealService.Infrastructure/*.csproj MealService.Infrastructure/
COPY ./Backend/Services/MealService/MealService.Application/*.csproj MealService.Application/
COPY ./Backend/Services/MealService/MealService.Domain/*.csproj MealService.Domain/

RUN dotnet restore MealService.GrpcServer/MealService.GrpcServer.csproj

COPY ./Backend/Services/MealService/ /src/
WORKDIR /src/MealService.GrpcServer
RUN dotnet build "./MealService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MealService.GrpcServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

ARG MEAL_GRPCSERVER_PORT
EXPOSE ${MEAL_GRPCSERVER_PORT}

ENTRYPOINT ["dotnet", "MealService.GrpcServer.dll"]