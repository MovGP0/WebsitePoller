# WebsitePoller
Service that polls a website and notifies the user if it has changed

## Configuration
In order to configure the WebsitePoller, you need to edit the `settings.hson` file before installation. 

For configuration after installation, you need to find the installation folder of the service and edit the `settings.hson` file with admin rights. 

## Installation 
In order to install the service, you need to open a new console with admin rights. Then you execute the following command: 
```powershell
./WebsitePoller.exe install
```

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