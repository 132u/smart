using System;

using NLog;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests
{
	[SetUpFixture]
	public class GlobalSetup
	{
		[SetUp]
		public static void SetUp()
		{
			try
			{
				ConfigurationManager.InitializeRelatedToUserFields();
				ConfigurationManager.InitializeRelatedToServerFields();
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Произошла ошибка в GlobalSetup", ex);
				throw;
			}
		}
		public static Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
