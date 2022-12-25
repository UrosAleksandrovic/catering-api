FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# copy of csproj and restore  as distinct layers
COPY *.sln .
COPY ./src/Catering.Api/*.csproj ./src/Catering.Api/
COPY ./src/Catering.Application/*.csproj ./src/Catering.Application/
COPY ./src/Catering.DependencyInjection/*.csproj ./src/Catering.DependencyInjection/
COPY ./src/Catering.Domain/*.csproj ./src/Catering.Domain/
COPY ./src/Catering.Infrastructure/*.csproj ./src/Catering.Infrastructure/
COPY ./tests/Catering.Domain.Test/*.csproj ./tests/Catering.Domain.Test/

RUN dotnet restore

# copy everything else and build app
COPY ./src/Catering.Api/. ./src/Catering.Api/
COPY ./src/Catering.Application/. ./src/Catering.Application/
COPY ./src/Catering.DependencyInjection/. ./src/Catering.DependencyInjection/
COPY ./src/Catering.Domain/. ./src/Catering.Domain/
COPY ./src/Catering.Infrastructure/. ./src/Catering.Infrastructure/
COPY ./tests/Catering.Domain.Test/ ./tests/Catering.Domain.Test/

RUN dotnet test

WORKDIR /app/src/Catering.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

RUN apt-get update && apt-get -y install libldap-2.4-2 ldap-utils libldap-common
RUN ln -s /usr/lib/x86_64-linux-gnu/libldap-2.4.so.2 /usr/lib/x86_64-linux-gnu/libldap.so.2 && \
    ln -s /usr/lib/x86_64-linux-gnu/liblber-2.4.so.2 /usr/lib/x86_64-linux-gnu/liblber.so.2

WORKDIR /app

COPY --from=build /app/src/Catering.Api/out ./
ARG MAIN_PORT
ENV ASPNETCORE_URLS=http://+:${MAIN_PORT}
ENTRYPOINT ["dotnet","Catering.Api.dll"]