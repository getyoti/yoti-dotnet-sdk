#!/usr/bin/env bash

set -e
dotnet test test/Yoti.Auth.Tests/Yoti.Auth.Tests.csproj -c Release --verbosity minimal
dotnet test test/Yoti.Auth.Sandbox.Tests/Yoti.Auth.Sandbox.Tests.csproj -c Release --verbosity minimal
