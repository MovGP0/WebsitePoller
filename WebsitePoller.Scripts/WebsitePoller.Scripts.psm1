Add-Type -Assembly 'System.IO';
[string]$path = [System.IO.Path]::Combine($PSScriptRoot, '*.ps1');
$scripts = Get-ChildItem -Path $path;
$scripts | ForEach-Object { . $_.FullName; };

Export-ModuleMember -Function @('New-BinariesZip', 'New-ZipFile');