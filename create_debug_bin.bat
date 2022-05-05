set RootDir=%~dp0
cd %RootDir%BIN
rd %RootDir%BIN\ /q /s
rem cd..

xcopy ".\SPP2Launcher\bin\Debug\*.exe" ".\.BIN\DEBUG\SPP2Launcher\" /y /r
xcopy ".\SPP2Launcher\bin\Debug\*.dll" ".\.BIN\DEBUG\SPP2Launcher\" /y /r
xcopy ".\SPP2Launcher\bin\Debug\en\*.dll" ".\.BIN\DEBUG\SPP2Launcher\en\" /y /r

Pause