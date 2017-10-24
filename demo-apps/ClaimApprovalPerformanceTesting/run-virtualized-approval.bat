@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService using http://localhost:8080/[MemberAccounts;Approval] virtual services" Application\bin\Debug\ClaimProcessingService.exe http://localhost:8080/MemberAccounts http://localhost:8080/Approval
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService using http://localhost:8080/[Approval] virtual services" Application\bin\Debug\TIBCOClaimProcessingService.exe http://localhost:8080/Approval
:continue
start "ApprovalService" Application\bin\Debug\ApprovalService.exe
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
