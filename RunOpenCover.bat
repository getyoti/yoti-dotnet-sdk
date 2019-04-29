@ECHO OFF

REM Get OpenCover Executable (so we dont have to change the code when the version number changes)
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

SET OpenCoverDirectory="OpenCover\Yoti.Auth.Tests"

if not exist %OpenCoverDirectory% mkdir %OpenCoverDirectory%

SET Filter="+[Yoti.Auth]* -[Yoti.Auth.Tests]* -[Yoti.Auth]*ProtoBuf.Attribute* -[Yoti.Auth]*ProtoBuf.Common*"
SET OutputFile=%OpenCoverDirectory%\coverage.xml
SET TargetArgs="test test/Yoti.Auth.Tests"

%OpenCoverExe% "-target:%ProgramFiles%\dotnet\dotnet.exe" -targetargs:%TargetArgs% -register:user -filter:%Filter% -output:%OutputFile%