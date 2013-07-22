; ждем пока окно загрузки будет активным
Local $handle = WinWaitActive("[CLASS:#32770]", "");
; убеждаемся что окно активно
WinFlash($handle);

ControlSetText($handle, "", "Edit1", $CmdLine[1]);

Send("{ENTER}");