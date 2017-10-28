@echo off
start "MemberAccountsService, HTTP Basic Authentication" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe basic
start "ClaimProcessingService using http://%COMPUTERNAME%:7200/MemberAccounts, HTTP Basic Authentication" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:7200/MemberAccounts basic
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe
