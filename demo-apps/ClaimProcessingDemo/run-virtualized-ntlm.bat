@echo off
start "MemberAccountsService, NTLM Authentication" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe ntlm
start "ClaimProcessingService using http://%COMPUTERNAME%:7200/MemberAccounts, NTLM Authentication" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:7200/MemberAccounts ntlm
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe
