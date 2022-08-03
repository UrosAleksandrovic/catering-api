FROM mcr.microsoft.com/dotnet/core/sdk:6.0 AS build
WORKDIR /app

# copy of csproj and restore  as distinct layers
COPY *.sln .
COPY ./src/Catering.Api/*.csproj ./src/Catering.Api/
COPY ./src/Catering.Application/*.csproj ./src/Catering.Application/
COPY ./src/Catering.DependencyInjection/*.csproj ./src/Catering.DependencyInjection/
COPY ./src/Catering.Domain/*.csproj ./src/Catering.Domain/
COPY ./src/Catering.Infrastructure/*.csproj ./src/Catering.Infrastructure/

RUN dotnet restore

# copy everything else and build app
COPY ./src/Catering.Api/. ./src/Catering.Api/
COPY ./src/Catering.Application/. ./src/Catering.Application/
COPY ./src/Catering.DependencyInjection/. ./src/Catering.DependencyInjection/
COPY ./src/Catering.Domain/. ./src/Catering.Domain/
COPY ./src/Catering.Infrastructure/. ./src/Catering.Infrastructure/

WORKDIR /app/src/Catering.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/src/Catering.Api/out ./
ENV ASPNETCORE_URLS=http://+:5500
ENTRYPOINT ["dotnet","Catering.Api.dll"]