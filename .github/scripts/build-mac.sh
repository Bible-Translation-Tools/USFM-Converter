#!/bin/bash

set -o xtrace

dotnet restore -r osx-x64 ./USFMConverter/USFMConverter.sln
dotnet msbuild ./USFMConverter/USFMConverter.sln -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:OutputPath=../output-mac/
mkdir ./dmg-source
cp -r ./output-mac/publish/USFMConverter.app ./dmg-source/USFMConverter.app

create-dmg \
  --volname "USFM Converter" \
  --volicon "usfmconverter.icns" \
  --background "usfm-dmg-bg.png" \
  --window-pos 200 120 \
  --window-size 640 360 \
  --icon-size 100 \
  --icon "USFMConverter.app" 195 230 \
  --hide-extension "USFMConverter.app" \
  --app-drop-link 445 230 \
  "USFMConverter.dmg" \
  "dmg-source/"

mkdir dmg
cp USFMConverter.dmg ./dmg/USFMConverter.dmg
