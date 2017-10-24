@echo off
start "MemberAccountsService, HTTP Basic Authentication" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe basic
start "ClaimProcessingService using http://%COMPUTERNAME%:8080/MemberAccounts, HTTP Basic Authentication" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:8080/MemberAccounts basic
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe
