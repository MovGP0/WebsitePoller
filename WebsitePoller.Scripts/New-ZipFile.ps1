function New-ZipFile
{
	param (
		[Parameter(Mandatory=$true, Position=0)]
        [ValidateScript({Test-Path -Path $_})]
		[string]
		$source, 

		[Parameter(Mandatory=$true, Position=1)]
		[ValidateScript({(Test-Path -Path $_ ) -eq $false})]
		[string]
		$destination
	)

	process
	{
		Add-Type -Assembly "System.IO.Compression.FileSystem"

		$compress = [System.IO.Compression.CompressionLevel]::Optimal;
		[System.IO.Compression.ZipFile]::CreateFromDirectory($Source, $destination, $compress, $false);
	}
}