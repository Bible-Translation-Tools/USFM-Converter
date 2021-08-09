#!/bin/bash

dotnet msbuild ./USFMConverter/USFMConverter.sln -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:OutputPath=../output-mac/
mkdir ./dmg-source
cp -r ./output-mac/publish/USFMConverter.app ./dmg-source/USFMConverter.app
create-dmg \
  --volname "USFM Converter" \
  --background "usfm-dmg-bg.png" \
  --window-pos 200 120 \
  --window-size 640 360 \
  --icon-size 100 \
  --icon "USFMConverter.app" 190 230 \
  --hide-extension "USFMConverter.app" \
  --app-drop-link 440 230 \
  "USFMConverter.dmg" \
  "dmg-source/"
mkdir installers
cp USFMConverter.dmg ./installers/USFMConverter.dmg
