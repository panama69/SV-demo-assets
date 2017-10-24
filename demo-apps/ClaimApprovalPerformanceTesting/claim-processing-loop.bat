@echo off
:start
echo The claim processing service will restart each time you press Enter.
echo Press CTRL+C to exit the loop.
Application\bin\Debug\ClaimProcessingService.exe %1 %2 %3
goto :start