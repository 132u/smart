If ($CmdLine[0] < 1) Then Exit(1)

If (WinWait("File Upload","",10) = 0) Then Exit(2)
If (WinActivate("File Upload") = 0) Then Exit(3)
If (WinWaitActive("File Upload","",5) = 0) Then Exit(4)
If (ControlFocus("File Upload", "", 0) = 0) Then Exit(5)
Sleep(2000)
If (ControlSetText("File Upload",'',0,$CmdLine[1]) = 0) Then Exit(6)
Send("{TAB}")
Send("{TAB}")
Send("{TAB}")
Send("{ENTER}")