@echo off
taskkill /IM MemberAccountsService.exe /T
taskkill /IM ClaimProcessingService.exe /T
taskkill /IM ApprovalService.exe /T
taskkill /IM ExchangeRateService.exe /T
taskkill /IM GUIClient.exe /T
