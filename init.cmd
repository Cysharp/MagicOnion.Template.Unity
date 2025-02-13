@echo off
setlocal

cd %~dp0
cd tools\RepoInitializer
dotnet run -- %*