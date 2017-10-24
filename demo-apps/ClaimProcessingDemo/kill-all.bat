@echo off
taskkill /IM MemberAccountsService.exe
taskkill /IM ClaimProcessingService.exe
taskkill /IM ApprovalService.exe
taskkill /IM ExchangeRateService.exe
taskkill /IM GUIClient.exe
