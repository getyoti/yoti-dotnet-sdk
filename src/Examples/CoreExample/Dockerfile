FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY Examples/CoreExample/CoreExample.csproj Examples/CoreExample/
COPY Yoti.Auth/Yoti.Auth.csproj Yoti.Auth/
RUN dotnet restore Examples/CoreExample/CoreExample.csproj
COPY . .
WORKDIR /src/Examples/CoreExample
COPY Examples/CoreExample/https C:/Users/ContainerUser/AppData/Roaming/ASP.NET/Https

RUN dotnet build CoreExample.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CoreExample.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreExample.dll"]
