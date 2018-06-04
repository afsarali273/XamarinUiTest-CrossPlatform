@echo off
echo Registering file type association for shell-script (^*.sh;^*.bash) to be executed by git-bash
assoc .bash=shell-script
assoc .sh=shell-script
ftype shell-script="C:\Program Files\Git\git-bash.exe" %%1
echo File association done.
pause