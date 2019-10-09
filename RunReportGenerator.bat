@ECHO OFF

REM dotnet tool install -g dotnet-reportgenerator-globaltool
REM dotnet tool install dotnet-reportgenerator-globaltool --tool-path tools

REM Get Report Generator (so we dont have to change the code when the version number changes)
for /R "%USERPROFILE%\.dotnet\tools" %%a in (*) do if /I "%%~nxa"=="ReportGenerator.exe" SET ReportGeneratorExe=%%~dpnxa

SET OutputDirectory=OpenCover\Yoti.Auth.Tests
SET CoverageDirectory=%OutputDirectory%\Coverage

if not exist %CoverageDirectory% mkdir %CoverageDirectory%

%ReportGeneratorExe% -targetdir:%CoverageDirectory% -reporttypes:Html;Badges -reports:%OutputDirectory%\Coverage.xml -verbosity:Error

start "report" "%CoverageDirectory%\index.htm"