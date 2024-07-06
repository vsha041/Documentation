# make sure you are logged in to a machine that has access to all the servers - server1.com.co.nz, server2.com.co.nz
# otherwise your account can get locked out

cls
Get-WmiObject -Class win32_logicalDisk -ComputerName server1.com.co.nz, server2.com.co.nz | Select-Object pscomputername, deviceid, freespace, size | Format-Table pscomputername,DeviceId, @{n="Size";e={[math]::Round($_.Size/1GB,2)}},@{n="FreeSpace";e={[math]::Round($_.FreeSpace/1GB,2)}},@{n="PercentageFree";e={[math]::Round($_.FreeSpace/$_.Size,4)*100}}
