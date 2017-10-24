@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService using http://localhost:8080/[MemberAccounts;Approval;ExchangeRate] virtual services" claim-processing-loop.bat http://localhost:8080/MemberAccounts http://localhost:8080/Approval http://localhost:8080/ExchangeRate
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService using http://localhost:8080/[Approval;ExchangeRate] virtual services" tibco-claim-processing-loop.bat http://localhost:8080/Approval http://localhost:8080/ExchangeRate
:continue
start "ApprovalService" approval-loop.bat
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
