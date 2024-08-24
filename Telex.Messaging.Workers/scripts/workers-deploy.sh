#!/bin/bash
set -e

# navigate to repo root and fetch latest changes
cd "$(git rev-parse --show-toplevel)"

git checkout dev
git pull origin dev

# install dependencies
dotnet restore ./Telex.Messaging/Telex.Messaging.sln

# build app
cd ./Telex.Messaging/Telex.Messaging.Workers
dotnet build --no-restore -c Release

# publish app
dotnet publish Telex.Messaging.Workers.csproj --no-build -c Release

# restart the systemd service
sudo systemctl restart telex-workers
