using System.Windows.Forms;
using NLog;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	public static class HotKey
	{
		public static Logger Logger = LogManager.GetCurrentClassLogger();

		public static void ShiftRight()
		{
			Logger.Trace("Hotkey клик 'Shift Right'");
			SendKeys.SendWait("+{RIGHT}");
		}

		public static void CtrlRight()
		{
			Logger.Trace("Hotkey клик 'Ctrl Right'");
			SendKeys.SendWait("^{RIGHT}");
		}

		public static void CtrlLeft()
		{
			Logger.Trace("Hotkey клик 'Ctrl Left'");
			SendKeys.SendWait("^{LEFT}");
		}

		public static void ArrowRight()
		{
			Logger.Trace("Hotkey клик 'Arrow Right'");
			SendKeys.SendWait("{RIGHT}");
		}

		public static void CtrlEnter()
		{
			Logger.Trace("Hotkey клик 'Ctrl Enter'");
			SendKeys.SendWait("^{ENTER}");
		}

		public static void ShiftF3()
		{
			Logger.Trace("Hotkey клик 'Shift F3'");
			SendKeys.SendWait("+{F3}");
		}

		public static void CtrlZ()
		{
			Logger.Trace("Hotkey клик 'Ctrl z'");
			SendKeys.SendWait("^{z}");
		}

		public static void CtrlY()
		{
			Logger.Trace("Hotkey клик 'Ctrl y'");
			SendKeys.SendWait("^{y}");
		}

		public static void Tab()
		{
			Logger.Trace("Hotkey клик 'Tab'");
			SendKeys.SendWait("{TAB}");
		}

		public static void CtrlHome()
		{
			Logger.Trace("Hotkey клик 'Ctrl Home'");
			SendKeys.SendWait("^{HOME}");
		}

		public static void CtrlShiftLeft()
		{
			Logger.Trace("Hotkey клик 'Ctrl Shift Left'");
			SendKeys.SendWait("^+{LEFT}");
		}

		public static void CtrlShiftRight()
		{
			Logger.Trace("Hotkey клик 'Ctrl Shift Right'");
			SendKeys.SendWait("^+{RIGHT}");
		}

		public static void ShiftRight(int count = 1)
		{
			Logger.Trace("Hotkey клик 'Shift Right Right Right'");
			SendKeys.SendWait("+{RIGHT " + count + "}");
		}

		public static void End()
		{
			Logger.Trace("Hotkey клик 'End'");
			SendKeys.SendWait("{END}");
		}

		public static void CtrlE()
		{
			Logger.Trace("Hotkey клик 'Ctrl e'");
			SendKeys.SendWait("^{e}");
		}

		public static void F9()
		{
			Logger.Trace("Hotkey клик 'F9'");
			SendKeys.SendWait("{F9}");
		}

		public static void CtrlShiftHome()
		{
			Logger.Trace("Hotkey клик 'Ctrl Shift HOME'");
			SendKeys.SendWait("+^{HOME}");
		}

		public static void CtrlInsert()
		{
			Logger.Trace("Hotkey клик 'Ctrl INSERT'");
			SendKeys.SendWait("^{INSERT}");
		}

		public static void Home()
		{
			Logger.Trace("Hotkey клик 'HOME'");
			SendKeys.SendWait("{HOME}");
		}
	}
}
