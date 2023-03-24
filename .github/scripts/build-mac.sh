#!/bin/bash

set -o xtrace

dotnet restore -r osx-x64 ./USFMConverter/USFMConverter.sln
dotnet msbuild ./USFMConverter/USFMConverter.sln -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release -p:OutputPath=../output-mac/
mkdir ./dmg-source
cp -r ./output-mac/publish/USFMConverter.app ./dmg-source/USFMConverter.app

# Create a new keychain
security create-keychain -p "$KEYCHAIN_PASSWORD" build.keychain
# Set it as the default keychain
security default-keychain -s build.keychain
# Unlock the keychain so it can be used without an authorisation prompt
security unlock-keychain -p "$KEYCHAIN_PASSWORD" build.keychain

# Decode certificate to file
echo "$MACOS_CERTIFICATE" | base64 --decode > certificate.p12
# Import into keychain
security import certificate.p12 -k build.keychain -P "$MACOS_CERTIFICATE_PWD" -T /usr/bin/codesign
# Allow codesign to access keychain
security set-key-partition-list -S apple-tool:,apple:,codesign: -s -k "$KEYCHAIN_PASSWORD" build.keychain

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
  --window-size 640 400 \
  --icon-size 80 \
  --icon "USFMConverter.app" 176 248 \
  --hide-extension "USFMConverter.app" \
  --app-drop-link 473 248 \
  --codesign "$SIGNING_IDENTITY" \
  "USFMConverter.dmg" \
  "dmg-source/"

mkdir dmg
cp USFMConverter.dmg ./dmg/USFMConverter.dmg
