@echo off

SET Region=%1
SET Environment=%2

SET InstallPath=C:\inetpub\wwwroot\UrlShortenerDemo
SET InstallFilesPath=C:\QueueIT\InstallFiles\UrlShortenerDemo\

echo Deleting existing web site
%windir%\system32\inetsrv\appcmd delete site /site.name:UrlShortenerDemo

echo Deleting existing application pool 
%windir%\system32\inetsrv\appcmd delete apppool /apppool.name:UrlShortenerDemo

echo Removing old files
rmdir %InstallPath% /S /Q

echo Creating directory %InstallPath%
mkdir %InstallPath%

echo Copying new files
xcopy %InstallFilesPath%\_PublishedWebsites\UrlShortenerDemo %InstallPath% /Y /E
IF %ERRORLEVEL% NEQ 0 GOTO Error

echo Creating App Pool
%windir%\system32\inetsrv\appcmd add apppool /name:UrlShortenerDemo /managedRuntimeVersion:v4.0 /managedPipelineMode:Integrated
%windir%\system32\inetsrv\appcmd set config /section:applicationPools /[name='UrlShortenerDemo'].processModel.identityType:NetworkService
IF %ERRORLEVEL% NEQ 0 GOTO Error

echo Creating Website 
%windir%\system32\inetsrv\appcmd add site /name:UrlShortenerDemo /id:457 /physicalPath:%InstallPath% /bindings:http/*:80:
IF %ERRORLEVEL% NEQ 0 GOTO Error

echo Setting App Pool on website (No SSL)
%windir%\system32\inetsrv\appcmd set app /app.name:UrlShortenerDemo/ /applicationPool:UrlShortenerDemo
IF %ERRORLEVEL% NEQ 0 GOTO Error

echo Starting IIS Windows Service
net start W3SVC

IF %ERRORLEVEL% NEQ 0 IF %ERRORLEVEL% NEQ 2 GOTO Error

GOTO End

:Error
echo ######################################################
echo # ERROR OCCURED WHILE INSTALLING OR STARTING SERVICE #
echo ######################################################

:End