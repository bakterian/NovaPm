set packageFolder=%~dp0
dotnet "%packageFolder%NovaPm.Console.dll" -l %packageFolder%CapturedData.log

pause