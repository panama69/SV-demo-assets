@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService" claim-processing-loop.bat
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService" tibco-claim-processing-loop.bat
:continue
start "ApprovalService" approval-loop.bat
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
