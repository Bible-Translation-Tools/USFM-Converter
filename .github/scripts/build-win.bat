dotnet publish .\USFMConverter\USFMConverter.sln -c Release --self-contained true -o .\output-win -r win-x64
dir output-win
"%programfiles(x86)%\Inno Setup 6\iscc.exe" .\installerscript.iss
