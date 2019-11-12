#!/usr/bin/env bash

set -e
dotnet test src/Yoti.Auth.sln -c Release --verbosity minimal
