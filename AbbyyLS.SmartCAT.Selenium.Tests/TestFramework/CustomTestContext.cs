using System;
using System.Diagnostics;

using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	class CustomTestContext
	{
		public static void WriteLine(string message, params object[] arguments)
		{
			StackTrace stackTrace = new StackTrace();
			var m = string.Format(message, arguments);

			TestContext.WriteLine(string.Format("{0}  {1}: {2} ({3})", DateTime.Now, stackTrace.GetFrame(1).GetMethod().ReflectedType.Name, m, stackTrace.GetFrame(1).GetMethod().Name));
		}
	}
}
