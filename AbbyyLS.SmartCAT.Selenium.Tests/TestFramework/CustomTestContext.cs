using System;

using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	class CustomTestContext
	{
		public static void WriteLine(string message, params object[] arguments)
		{
			var m = string.Format(message, arguments);
			TestContext.WriteLine(string.Format("{0} - {1}", DateTime.Now, m));
		}
	}
}
