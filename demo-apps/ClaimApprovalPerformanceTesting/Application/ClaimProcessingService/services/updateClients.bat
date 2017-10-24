@echo off
call svcutil http://localhost:8101/ServiceSimulation/Demo/MemberAccountsService /reference:..\..\bin\Debug\Types.dll
rem call svcutil http://localhost:8103/ServiceSimulation/Demo/ApprovalService /reference:..\..\bin\Debug\Types.dll /reference:..\..\bin\Debug\ApprovalService.exe
call svcutil http://localhost:8103/ServiceSimulation/Demo/ApprovalService /reference:..\..\bin\Debug\Types.dll
call svcutil http://localhost:8104/ServiceSimulation/Demo/ExchangeRateService /reference:..\..\bin\Debug\Types.dll
