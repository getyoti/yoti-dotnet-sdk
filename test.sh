#!/usr/bin/env bash

set -e
dotnet test test/Yoti.Auth.Tests/Yoti.Auth.Tests.csproj -c Release --verbosity minimal
