@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService" Application\bin\Debug\ClaimProcessingService.exe
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService" Application\bin\Debug\TIBCOClaimProcessingService.exe
:continue
start "ApprovalService" Application\bin\Debug\ApprovalService.exe
start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
