; ���� ���� ���� �������� ����� ��������
Local $handle = WinWaitActive("[CLASS:#32770]", "");
; ���������� ��� ���� �������
WinFlash($handle);

ControlSetText($handle, "", "Edit1", $CmdLine[1]);

Send("{ENTER}");