#!/bin/bash

cd `dirname $0`
cd tools/RepoInitializer

dotnet run -- "$@"
