@echo off
if "%1" == "-t" goto :tibco
start "MemberAccountsService" demo-apps\ClaimApprovalPerformanceTesting\Application\bin\Debug\MemberAccountsService.exe
rem start "ClaimProcessingService using http://localhost:7200/[MemberAccounts;Approval;ExchangeRate] virtual services" Application\bin\Debug\ClaimProcessingService.exe http://localhost:8101/ServiceSimulation/Demo/MemberAccountsService/MemberAccountsService http://localhost:8103/ServiceSimulation/Demo/ApprovalService/ApprovalService http://localhost:8104/ServiceSimulation/Demo/ExchangeRateService/ExchangeRateService
start "ClaimProcessingService using http://dockerclient:6070/ExchangeRate virtual services" demo-apps\ClaimApprovalPerformanceTesting\Application\bin\Debug\ClaimProcessingService.exe http://localhost:8101/ServiceSimulation/Demo/MemberAccountsService/MemberAccountsService http://localhost:8103/ServiceSimulation/Demo/ApprovalService/ApprovalService http://localhost:8104/ServiceSimulation/Demo/ExchangeRateService/ExchangeRateService
rem http://dockerclient:6070/ExchangeRate
goto :continue
:tibco
start "TIBCOMemberAccountsService" Application\bin\Debug\TIBCOMemberAccountsService.exe
start "TIBCOClaimProcessingService using http://localhost:7200/[Approval;ExchangeRate] virtual services" Application\bin\Debug\TIBCOClaimProcessingService.exe http://localhost:7200/Approval http://localhost:7200/ExchangeRate
:continue
start "ApprovalService" demo-apps\ClaimApprovalPerformanceTesting\Application\bin\Debug\ApprovalService.exe
start "ExchangeRateService" demo-apps\ClaimApprovalPerformanceTesting\Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" demo-apps\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
ping.exe -n 6 -w 1 127.0.0.1 >nul 2>&1