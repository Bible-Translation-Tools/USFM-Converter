#!/bin/bash

export INSTALL4J_LICENSE

dotnet publish ./USFMConverter/USFMConverter.sln -c Release --self-contained true -o ./output-win -r win-x64
dotnet publish ./USFMConverter/USFMConverter.sln -c Release --self-contained true -o ./output-nix -r linux-x64
dotnet publish ./USFMConverter/USFMConverter.sln -c Release --self-contained true -o ./output-mac -r osx-x64

docker run -v $(pwd):/repo -e INSTALL4J_LICENSE wycliffeassociates/install4j-docker /install4j/install4j9.0.2/bin/install4jc --license=$INSTALL4J_LICENSE /repo/usfmconverter.install4j