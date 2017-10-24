@echo off
:start
echo Approval service will restart each time you press Enter.
echo Press CTRL+C to exit the loop.
Application\bin\Debug\TIBCOClaimProcessingService.exe %1 %2
goto :start