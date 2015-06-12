$deployServerName=$args[0]

if(Test-Path \\$deployServerName.als.local\ABBYYLS\CAT\log)
{
	Remove-Item \\$deployServerName.als.local\ABBYYLS\CAT\log -Recurse 
}