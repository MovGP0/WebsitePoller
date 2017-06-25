$scripts = Get-ChildItem -Path [System.IO.Path]::Combine($PSScriptRoot, '*.ps1');
$scripts | ForEach-Object { . $_.FullName; };

Export-ModuleMember -Function @('New-BinariesZip', 'New-ZipFile');