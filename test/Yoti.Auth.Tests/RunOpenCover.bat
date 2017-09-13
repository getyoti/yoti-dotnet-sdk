@ECHO OFF

REM Get OpenCover Executable (so we dont have to change the code when the version number changes)
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="OpenCover.Console.exe" SET OpenCoverExe=%%~dpnxa

SET ProtoBufAttributeNamespace=AttrpubapiV1
SET ProtoBufCommonNamespace=CompubapiV1

SET Filter="+[Yoti.Auth]* +[Yoti.Auth.Owin]* -[Yoti.Auth.Tests]* -[Yoti.Auth]%ProtoBufAttributeNamespace%* -[Yoti.Auth]%ProtoBufCommonNamespace%*"
SET OutputFile=..\..\..\OpenCover\Yoti.Auth.Tests\coverage.xml

%OpenCoverExe% "-target:%ProgramFiles%\dotnet\dotnet.exe" -targetargs:test -register:user -filter:%Filter% -output:%OutputFile% -oldStyle