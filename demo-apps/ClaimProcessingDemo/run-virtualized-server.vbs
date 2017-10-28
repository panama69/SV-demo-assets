Option Explicit
On Error Resume Next
Dim WshShell
set WshShell=CreateObject("WScript.Shell")
WshShell.run "demo-apps\ClaimProcessingDemo\run-virtualized-server.bat"
WScript.Quit