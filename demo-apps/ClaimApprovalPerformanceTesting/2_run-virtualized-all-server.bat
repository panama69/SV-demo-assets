@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService using http://localhost:7200/[MemberAccounts;Approval;ExchangeRate] virtual services" Application\bin\Debug\ClaimProcessingService.exe http://alm-dev:6070/MemberAccounts http://alm-dev:6070/Approval http://alm-dev:6070/ExchangeRate
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService using http://localhost:7200/[Approval;ExchangeRate] virtual services" Application\bin\Debug\TIBCOClaimProcessingService.exe http://localhost:7200/Approval http://localhost:7200/ExchangeRate
:continue
start "ApprovalService" Application\bin\Debug\ApprovalService.exe
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
