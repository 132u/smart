If ($CmdLine[0] < 1) Then Exit(1)

WinWaitActive("File Upload")
Send($CmdLine[1])
Send("{ENTER}")