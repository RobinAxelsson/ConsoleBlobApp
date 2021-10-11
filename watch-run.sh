#!/usr/bin/env bash
pushd $HOME/Code/NET-YH-T2/ConsoleBlobApp/src/BlobCI
fswatch -or ./run.sh | xargs -n1 -I{} bash run.sh
popd
