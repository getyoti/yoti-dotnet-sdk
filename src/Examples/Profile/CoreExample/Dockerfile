FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Examples/Profile/CoreExample/CoreExample.csproj Examples/Profile/CoreExample/
COPY Yoti.Auth/Yoti.Auth.csproj Yoti.Auth/
RUN dotnet restore Examples/Profile/CoreExample/CoreExample.csproj
COPY . .
WORKDIR /src/Examples/Profile/CoreExample
RUN dotnet build CoreExample.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CoreExample.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreExample.dll"]
