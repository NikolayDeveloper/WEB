InstallUtil /LogToConsole=true Iserv.Niis.ExternalServices.StatusSender.Host.exe
sc failure  IntegrationIntelStatusSender reset= 0 actions= restart/1000/restart/5000/restart/60000
pause