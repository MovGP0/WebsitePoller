function New-BinariesZip {
	process
	{
		Add-Type -Assembly 'System.IO';
		$path = [System.IO.Path]::Combine($PSScriptRoot, 'New-ZipFile.ps1');
		. $path;

		[string]$source = [System.IO.Path]::Combine($PSScriptRoot, '..\WebsitePoller\bin\debug\' ); 
		[string]$destination = [System.IO.Path]::Combine($PSScriptRoot, '..\binaries.zip');

		If(Test-Path -Path $destination) {
			Remove-Item -Path $destination;
		};

		New-ZipFile -Source $source -Destination $destination;
	}
}