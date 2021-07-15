# USFM-Converter
Tool for converting USFM to readable formats like HTML and DOCX


## Mac OS build reference

### Publishing the app

#### Packaging the program with `.app` extension

Run the following command in the project directory where .csproj file is. __
`dotnet msbuild -t:BundleApp -p:RuntimeIdentifier=osx-x64 -property:Configuration=Release`

**The command does not automatically put the icon in the `.app` directory.**
**You must place the `converter_logo_inverted_icon.icns` file into `Resources` directory in the `.app` folder**
**`converter_logo_inverted_icon.icns` file can be found in `USFMConverter/UI/Assets`**

### Folder Structure

The `.app` file can be found in
> USFMConverter/bin/Release/net5.0/osx-x64/publish/USFMConverter.app

Above command will also create binary build of the program in
> USFMConverter/bin/Release/net5.0/osx-x64

The `USFMConverter.app` has the following folder structure

```
USFMConverter.app
|
----Contents\
|
------MacOS\ (all your DLL files, etc. -- the output of `dotnet publish`)
|     |
|     ---USFMConverter
|     |
|     ---USFMConverter.dll
|     |
|     ---Avalonia.dll
|
------Resources\
|     |
|     -----USFMConverter.icns (icon file)
|
------Info.plist [stores information on your bundle identifier, version, etc.)
```
