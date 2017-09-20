@ECHO OFF

REM Get Report Generator (so we dont have to change the code when the version number changes)
for /R "%~dp0packages" %%a in (*) do if /I "%%~nxa"=="ReportGenerator.exe" SET ReportGeneratorExe=%%~dpnxa

SET OutputDirectory=..\OpenCover\Yoti.Auth.Tests

%ReportGeneratorExe% -targetdir:%OutputDirectory%\Coverage -reporttypes:Html;Badges -reports:%OutputDirectory%\Coverage.xml  -verbosity:Error

start "report" "%OutputDirectory%\Coverage\index.htm"