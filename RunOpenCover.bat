@ECHO OFF

REM Get OpenCover Executable (so we dont have to change the code when the version number changes)
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

SET ProtoBufAttributeNamespace=AttrpubapiV1
SET ProtoBufCommonNamespace=CompubapiV1
SET OpenCoverDirectory="..\OpenCover"

if not exist %OpenCoverDirectory% mkdir %OpenCoverDirectory%

SET Filter="+[Yoti.Auth]* +[Yoti.Auth.Owin]* -[Yoti.Auth.Tests]* -[Yoti.Auth]%ProtoBufAttributeNamespace%* -[Yoti.Auth]%ProtoBufCommonNamespace%*"
SET OutputFile=%OpenCoverDirectory%\Yoti.Auth.Tests\coverage.xml
SET TargetArgs="test test/Yoti.Auth.Tests"

%OpenCoverExe% "-target:%ProgramFiles%\dotnet\dotnet.exe" -targetargs:%TargetArgs% -register:user -filter:%Filter% -output:%OutputFile% -oldStyle