#https://stackoverflow.com/questions/39936971/procdump-error-writing-dump-file-0x80070005-error-0x80070005-2147024891-acc
#Prevent IIS from recycling the process during procdump and causing an Access Denied error message
$iispid = Get-Process svchost | ?{$_.modules.ModuleName -eq "iisw3adm.dll"} | Select -First 1 -ExpandProperty Id
$workerpid = Get-Process w3wp | Sort ws -Descending | Select -First 1 -ExpandProperty Id
#move to location where you want to save the dump files
#Add -accepteula to the sysinternals calls if you want to bypass the initial EULA prompt on new servers
& "C:\Temp\PSTools\pssuspend.exe" $iispid -accepteula
Write-Output "Creating memory dump for w3wp PID $workerpid"
md -Force d:\temp
& "C:\Temp\Procdump\procdump.exe" -ma $workerpid D:\Temp\w3wp_process.dmp -accepteula
& "C:\Temp\PSTools\pssuspend.exe" $iispid -r -accepteula
