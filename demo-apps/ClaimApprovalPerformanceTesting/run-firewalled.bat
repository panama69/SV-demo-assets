@echo off
set FWIP=88.103.225.210
start "MemberAccountsService" Application\bin\Debug\MemberAccountsService.exe
rem start "ClaimProcessingService" Application\bin\Debug\ClaimProcessingService.exe
start "ClaimProcessingService using http://%FWIP%:8080/[MemberAccounts;Approval;ExchangeRate] virtual services" Application\bin\Debug\ClaimProcessingService.exe http://%FWIP%:8101/ServiceSimulation/Demo/MemberAccountsService/MemberAccountsService http://%FWIP%:8103/ServiceSimulation/Demo/ApprovalService/ApprovalService http://%FWIP%:8104/ServiceSimulation/Demo/ExchangeRateService/ExchangeRateService

start "ApprovalService" Application\bin\Debug\ApprovalService.exe
rem start "ExchangeRateService" Application\bin\Debug\ExchangeRateService.exe
start "GUIClient" ..\ClaimProcessingDemo\Application\GUIClient\bin\Debug\GUIClient.exe
