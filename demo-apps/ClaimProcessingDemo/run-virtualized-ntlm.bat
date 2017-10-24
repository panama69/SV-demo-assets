@echo off
start "MemberAccountsService, NTLM Authentication" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe ntlm
start "ClaimProcessingService using http://%COMPUTERNAME%:8080/MemberAccounts, NTLM Authentication" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:8080/MemberAccounts ntlm
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe
