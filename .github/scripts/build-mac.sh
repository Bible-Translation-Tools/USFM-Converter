#!/bin/bash

set -o xtrace

dotnet restore -r osx-x64 ./USFMConverter/USFMConverter.sln
dotnet msbuild ./USFMConverter/USFMConverter.sln -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:OutputPath=../output-mac/
mkdir ./dmg-source
cp -r ./output-mac/publish/USFMConverter.app ./dmg-source/USFMConverter.app

#!/bin/bash
APP_NAME="./dmg-source/USFMConverter.app"
ENTITLEMENTS="./usfmconverter.entitlements"
SIGNING_IDENTITY="wait" # matches Keychain Access certificate name

find "$APP_NAME/Contents/MacOS/"|while read fname; do
    if [[ -f $fname ]]; then
        echo "[INFO] Signing $fname"
        codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$fname"
    fi
done

echo "[INFO] Signing app file"

codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" "$APP_NAME"

create-dmg \
  --volname "USFM Converter" \
  --volicon "usfmconverter.icns" \
  --background "usfm-dmg-bg.png" \
  --window-pos 200 120 \
  --window-size 640 360 \
  --icon-size 80 \
  --icon "USFMConverter.app" 176 248 \
  --hide-extension "USFMConverter.app" \
  --app-drop-link 473 248 \
  "USFMConverter.dmg" \
  "dmg-source/"

mkdir dmg
cp USFMConverter.dmg ./dmg/USFMConverter.dmg
