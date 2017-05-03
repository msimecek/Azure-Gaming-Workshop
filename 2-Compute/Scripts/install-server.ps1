$Source = "https://dxgamingmount.blob.core.windows.net/workshop/OpenTTD.zip?sv=2016-05-31&si=workshop&sr=b&sig=hf1HSNGnUay2O7D%2FN5VSg2MKZnDdbl3%2FfGg0ndfRjGI%3D"
$Destination = "D:\Server\"
$DestinationZip= $Destination + "OpenTTD.zip"

if ((Test-Path $Destination) -eq $false) {
    
    New-Item $Destination -Type Directory -Force

    $WebClient = New-Object -TypeName System.Net.WebClient
    $WebClient.DownloadFile($Source, $DestinationZip)
    Expand-Archive $DestinationZip -DestinationPath $Destination

    New-NetFirewallRule -Name AlloW_OpenTTD_TCP -DisplayName "Allow OpenTTD TCP" -Description "OTTD Server" -Protocol TCP -LocalPort 3979 -Enabled True -Profile Any -Action Allow
    New-NetFirewallRule -Name AlloW_OpenTTD_UDP -DisplayName "Allow OpenTTD UDP" -Description "OTTD Server" -Protocol UDP -LocalPort 3979 -Enabled True -Profile Any -Action Allow
}

# TODO: download config separately

Start-Process "$($Destination)OpenTTD\openttd.exe" -ArgumentList "-D"