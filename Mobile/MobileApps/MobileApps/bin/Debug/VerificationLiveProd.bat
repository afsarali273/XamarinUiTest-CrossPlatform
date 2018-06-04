set SAVESTAMP=%date:~0%_%time:~0,2%-%time:~3,2%-%time:~6,2%
set SAVESTAMP2=%SAVESTAMP:/=-%.xml

set SAVESTAMP1=%date:~0%_%time:~0,2%-%time:~3,2%-%time:~6,2%
set SAVESTAMP1=%SAVESTAMP1:/=-%

set Tile_Name=DailyVerification_LIVE
set Enviroment=LIVE_Build_Version

set Build_Verison=9.7.0
set Build_Num=(736)
set Device=Samsung_S3_4.3
set DailyVFolder=Veri 9.7

rem creates folder for xml backup on the local machine
mkdir XMLLiveProdResults

rem creates path to save xml files if the path exists line 16 is skipped
mkdir Y:AppXML\%Tile_Name%\%Enviroment%_%Build_Verison%\%Build_Num%\%Device%

rem delets screenshot folder if it still exist
rmdir /s /q "Screenshot %Build_Verison%%Build_Num%"

nunit-console DailyVerification.dll -xml="DailyVerification_%Build_Verison%" 
copy DailyVerification_%Build_Verison% "Y:\AppXML\%Tile_Name%\%Enviroment%_%Build_Verison%\%Build_Num%\%Device%\TestResult_%Build_Verison%_%SAVESTAMP2%"   
xcopy "Screenshot %Build_Verison%%Build_Num%" "Y:\Media Backup\MobileApp\TestResult_%Build_Verison%_%SAVESTAMP1%" /e /i

MOVE DailyVerification_%Build_Verison% ".\XMLLiveProdResults\TestResult_%Build_Verison%_%SAVESTAMP2%"

rem  creates backup folder and copies screenshot to backup folder on the local machine
xcopy "C:\Scripts\%DailyVFolder%\VodaiOSPRODLocal\VodaiOS\bin\Debug\Screenshot %Build_Verison% %Build_Num%" ".\Backup\Screenshot_%Build_Verison%_%SAVESTAMP1%" /e /i


rem delets screenshot folder after execution
rmdir /s /q "Screenshot %Build_Verison%%Build_Num%"