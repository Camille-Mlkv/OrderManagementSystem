FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./Backend/Services/OrderService/*.sln ./
COPY ./Backend/Services/OrderService/OrderService.Application/*.csproj OrderService.Application/
COPY ./Backend/Services/OrderService/OrderService.Domain/*.csproj OrderService.Domain/
COPY ./Backend/Services/OrderService/OrderService.Infrastructure/*.csproj OrderService.Infrastructure/
COPY ./Backend/Services/OrderService/OrderService.API/*.csproj OrderService.API/

RUN dotnet restore OrderService.API/OrderService.API.csproj

COPY ./Backend/Services/OrderService/ /src/

WORKDIR /src/OrderService.API

RUN dotnet publish -c Release -o /app/publish 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN apt-get update && \
    apt-get install -y curl netcat-traditional telnet && \
    apt-get clean 

ARG CERT_PATH=/certificates/aspnetapp.pfx
COPY ${CERT_PATH} /https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD}

EXPOSE 5001

ENTRYPOINT ["dotnet", "OrderService.API.dll"]