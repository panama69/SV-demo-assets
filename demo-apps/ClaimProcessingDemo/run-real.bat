@echo off
start "MemberAccountsService" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:8101/ServiceSimulation/Demo/MemberAccountsService/MemberAccountsService
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe
