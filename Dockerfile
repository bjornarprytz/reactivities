FROM node:16 AS app-build

WORKDIR /usr/src/app

# Install dependencies
COPY client-app/package*.json ./
RUN npm install

# Bundle app source
COPY ./client-app/ ./
RUN npm run build

# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS api-build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./server/*.sln .
COPY ./server/API/*.csproj ./API/
COPY ./server/Application/*.csproj ./Application/
COPY ./server/Domain/*.csproj ./Domain/
COPY ./server/Persistence/*.csproj ./Persistence/
COPY ./server/Infrastructure/*.csproj ./Infrastructure/
RUN dotnet restore

# copy everything else and build app
COPY ./server/. ./
RUN dotnet publish -c release -o /out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=api-build /out ./
COPY --from=app-build /usr/src/app/build ./wwwroot

ENTRYPOINT ["dotnet", "API.dll"]