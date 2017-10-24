@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService using http://localhost:8080/MemberAccounts virtual service" Application\bin\Debug\ClaimProcessingService.exe http://localhost:8080/MemberAccounts
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService" Application\bin\Debug\TIBCOClaimProcessingService.exe
:continue
start "ApprovalService" approval-loop.bat
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
