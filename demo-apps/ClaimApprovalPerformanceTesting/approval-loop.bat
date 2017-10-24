@echo off
:start
echo The approval service will restart each time you press Enter.
echo Press CTRL+C to exit the loop.
Application\bin\Debug\ApprovalService.exe
goto :start