start "MemberAccountsService" Application\MemberAccountsService\bin\Debug\MemberAccountsService.exe
start "ClaimProcessingService using http://%COMPUTERNAME%:7200/MemberAccounts" Application\ClaimProcessingService\bin\Debug\ClaimProcessingService.exe http://%COMPUTERNAME%:7200/MemberAccounts
start "GUIClient" Application\GUIClient\bin\Debug\GUIClient.exe