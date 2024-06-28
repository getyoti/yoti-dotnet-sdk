FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Examples/DigitalIdentity/DigitalIdentity/DigitalIdentityExample.csproj Examples/DigitalIdentity/DigitalIdentity/
COPY Yoti.Auth/Yoti.Auth.csproj Yoti.Auth/
COPY . .
WORKDIR /src/Examples/DigitalIdentity/DigitalIdentity

FROM build AS publish
RUN dotnet publish DigitalIdentityExample.csproj -c Release -r linux-x64 -o /app -p:TargetFrameworks=netcoreapp6.0 -f netcoreapp6.0

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "./DigitalIdentityExample.dll"]
