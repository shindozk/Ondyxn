# Set page file to 16GB initial, 32GB max
$cs = Get-CimInstance -ClassName Win32_ComputerSystem
Set-CimInstance -InputObject $cs -Property @{AutomaticManagedPagefile = $false}

$existing = Get-CimInstance -ClassName Win32_PageFileSetting -ErrorAction SilentlyContinue
if ($existing) {
    Set-CimInstance -InputObject $existing -Property @{InitialSize = 16384; MaximumSize = 32768}
} else {
    New-CimInstance -ClassName Win32_PageFileSetting -Property @{Name = "C:\pagefile.sys"; InitialSize = 16384; MaximumSize = 32768} -ClientOnly
}

Write-Output "Page file configured: 16GB initial, 32GB max. REBOOT REQUIRED."
