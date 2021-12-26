@echo off

set HERE=%~dp0

call "%HERE%\vars.cmd"

call dotnet build

if exist %userprofile%\.nuget\packages\%PACKAGE% (
    rmdir /s /q %userprofile%\.nuget\packages\%PACKAGE%
)

if exist %NUGET_FEED%\%PACKAGE% (
    rmdir /s /q %NUGET_FEED%\%PACKAGE%
)

call nuget add %HERE%\..\bin\Debug\%PACKAGE%.%VERSION%.nupkg -Source %NUGET_FEED%

cd %TEST_PROJECT%

rmdir /s /q bin
rmdir /s /q obj

call dotnet clean

call dotnet restore

cd %HERE%\..