#!/usr/bin/env bash
fswatch -or ./run.sh | xargs -n1 -I{} bash run.sh
