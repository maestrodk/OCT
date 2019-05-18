@echo off
if [%1]==[] goto arg1missing
if [%2]==[] goto arg2missing

if not exist %1 goto file1missing
if not exist %2 goto file2missing

move /Y %1\*.* %2

goto alldone

:arg1missing
echo Missing argument 1!
goto alldone

:arg2missing
echo Missing argument 2!
goto alldone

:file1missing
echo File %1 does not exis
goto alldone

:file2missing
echo File %2 does not exis
goto alldone


:alldone
echo Finished.