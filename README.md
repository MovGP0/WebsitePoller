# WebsitePoller
Service that polls a website and notifies the user if it has changed

## Building
For Building you need .NET Framework 4.5.2 Targeting Pack or Developer Pack installed. 

To build execute the following: 
```powershell
. "$env:WinDir\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" .\WebsitePoller.sln
```
The compiled binaries can then be found in `.\WebsitePoller\bin\Debug\`. 

## Configuration
In order to configure the WebsitePoller, you need to edit the `settings.hson` file before installation. 

For configuration after installation, you need to find the installation folder of the service and edit the `settings.hson` file with admin rights. 

## Staring Service
In order to start the service without installing it, open a new console. Then you execute the following command: 
```powershell
./WebsitePoller.exe
```

## Installation 
In order to install the service, you need to open a new console with admin rights. Then you execute the following command: 
```powershell
./WebsitePoller.exe install
```
Installing the service has some benefits, since the service will automatically start and run in background when the computer starts. 

## Uninstallation 
In order to uninstall the service, you need to open a new console with admin rights. Then you execute the following command: 
```powershell
./WebsitePoller.exe uninstall
```

## Service management
The service supports start/stop as well as pause/resume. To do so, open `services.msc` and find the service. You can also use the PowerShell to manage the services. To do so, enter 
```powershell
Get-Command *-Service
```
to find the proper PowerShell commands. 