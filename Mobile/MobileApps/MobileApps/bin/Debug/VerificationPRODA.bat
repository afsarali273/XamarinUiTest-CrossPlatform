set SAVESTAMP=%DATE:/=-%-%TIME::=_%
set SAVESTAMP=%SAVESTAMP: =%
set SAVESTAMP=%SAVESTAMP:,=.%.xml

set SAVESTAMP1=%DATE:/=-%-%TIME::=_%
set SAVESTAMP1=%SAVESTAMP1: =%
mkdir Backup

nunit-console VerifiOne.dll -xml="VerifiOnePRODA_%SAVESTAMP%" 
nunit-console VerifiTwo.dll -xml="VerifiTwoPRODA_%SAVESTAMP%"
nunit-console VerifiThree.dll -xml="VerifiThreePRODA_%SAVESTAMP%"


