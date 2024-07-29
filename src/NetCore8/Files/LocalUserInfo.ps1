
$users = Get-LocalUser

$users | Select-Object Name, Enabled, LastLogon | ConvertTo-Json