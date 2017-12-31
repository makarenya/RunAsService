@echo off
rem  This batch file installs RunAsService
setlocal

set SERVICE_EXE=RunAsService.exe

if "%1" == "user" goto findinstaller
if "%1" == "localservice" goto findinstaller
if "%1" == "localsystem" goto findinstaller
if "%1" == "networkservice" goto findinstaller
goto usage

:findinstaller
set INSTALL_UTIL_ROOT=%SYSTEMROOT%\Microsoft.Net\Framework\v1.1.4322
set INSTALL_UTIL_EXE=%INSTALL_UTIL_ROOT%\InstallUtil.exe

if not exist %INSTALL_UTIL_EXE% goto installutilnotfound

if not exist %SERVICE_EXE% goto servicenotfound

rem  All seems to be OK, so install the service
echo.
echo Installing RunAsService using account-type %1
echo.

%INSTALL_UTIL_EXE% /name=RunAsService /account=%1 %SERVICE_EXE%

goto end

:usage
rem User did not supply the proper account parameter, show usage info
echo.
echo To install RunAsService you must supply an account type that is used to start 
echo RunAsService as parameter:
echo     Install account-type
echo.
echo where account-type can have one of the following values:
echo - user
echo   An account defined by a specific user on the network.
echo   Specifying User causes the system to prompt for a valid user name and password
echo   when the service is installed.
echo - localsystem
echo   An account that has a high privileged level.
echo - localservice (only on WinXP and later)
echo   An account that acts as a non-privileged user on the local computer, and 
echo   presents anonymous credentials to any remote server.
echo - networkservice (only on WinXP and later)
echo   An account that provides extensive local privileges, and presents the 
echo   computer's credentials to any remote server.
echo.
echo.

goto end

:servicenotfound
rem service executable could not be found
echo.
echo The service executable %SERVICE_EXE% could not be found, are you running install.cmd
echo in the correct directory?
echo.
goto end


:installutilnotfound
rem Install util could not be found. SDK correct installed?
echo.
echo The file installutil.exe (part of the .Net SDK runtime) could not be found.
echo It should be located in %INSTALL_UTIL_ROOT%.
echo Is the .Net SDK installed properly?
echo.

:end

endlocal