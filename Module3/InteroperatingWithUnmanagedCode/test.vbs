set manager = CreateObject("PowerManagerCom.PowerManager")

result = manager.GetLastSleepTime
WScript.Echo("Last sleep time " & result)

result = manager.GetLastWakeTime
WScript.Echo("Last wake time " & result)

result = manager.Sleep
WScript.Echo("System was sleeped " & result)

result = manager.Hibernate
WScript.Echo("System was hibernated " & result)


