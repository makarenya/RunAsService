@echo off
rem  This batch file installs RunAsService
setlocal

set SERVICE_EXE=RunAsService.exe

:findinstaller
set INSTALL_UTIL_ROOT=%SYSTEMROOT%\Microsoft.Net\Framework\v1.1.4322
set INSTALL_UTIL_EXE=%INSTALL_UTIL_ROOT%\InstallUtil.exe

if not exist %INSTALL_UTIL_EXE% goto installutilnotfound

if not exist %SERVICE_EXE% goto servicenotfound

rem  All seems to be OK, so install the service
echo.
echo Installing RunAsService using account-type %1
echo.

%INSTALL_UTIL_EXE% /u /name=RunAsService %SERVICE_EXE%

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