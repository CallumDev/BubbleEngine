#!/bin/bash
# Build all external project binaries

# NLua
cd NLua
xbuild /p:Configuration=Release NLua.Net45.sln
cd ..
#
