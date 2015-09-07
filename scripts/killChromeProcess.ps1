$chrome = Get-Process chrome -ErrorAction SilentlyContinue

if ($chrome)
{
	Stop-Process -Name chrome
}
