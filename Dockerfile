#syntax=docker/dockerfile:1.7-labs
ARG NODE_VERSION=23.11

### node install files (for copying to other instance.
FROM node:${NODE_VERSION}-alpine AS node
WORKDIR /app

RUN node -v

RUN npm install -g yarn --force

COPY --link --parents ./src/**/package.json .
COPY --link ./package.json .
COPY --link yarn.lock .

## get production node_modules
FROM node AS prod-deps
RUN yarn install --production --frozen-lockfile

## prepare build env for astro build
FROM node AS node-build

RUN npm install -g dotenv-cli --force

RUN yarn install --frozen-lockfile

COPY --link . .
RUN yarn --cwd ./src/App/ build


### dotnet build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG FEED_ACCESSTOKEN
WORKDIR /source

RUN dotnet tool install --tool-path /tools dotnet-gcdump
RUN dotnet tool install --tool-path /tools dotnet-trace
RUN dotnet tool install --tool-path /tools dotnet-dump
RUN dotnet tool install --tool-path /tools dotnet-counters

# COPY --link --parents ./src/**/*.csproj .
COPY --link . .

RUN dotnet restore

RUN dotnet publish ./src/Server/Server.csproj --no-restore -o /app

### dotnet runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

## install node into dotnet alpine?
COPY --from=node /usr/lib /usr/lib
COPY --from=node /usr/local/lib /usr/local/lib
COPY --from=node /usr/local/include /usr/local/include
COPY --from=node /usr/local/bin /usr/local/bin


WORKDIR /tools
COPY --from=build /tools .

WORKDIR /app
COPY --link --from=build /app .

COPY --link --from=prod-deps /app/node_modules ./node_modules
COPY --link --from=node-build /app/src/App/dist ./dist

RUN mkdir ./site_output
# RUN chmod -R 777 ./site_output

RUN chown -hR $APP_UID ./site_output
USER $APP_UID

EXPOSE 8000
EXPOSE 8001

ENTRYPOINT dotnet Server.dll
