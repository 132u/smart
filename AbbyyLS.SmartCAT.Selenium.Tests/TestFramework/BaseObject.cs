using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public class BaseObject
	{
		public static void InitPage<T>(T pageClass, WebDriver driver) where T : BaseObject, IAbstractPage<T>
		{
			pageClass.LoadPage();
			driver.WaitPageTotalLoad();
			PageFactory.InitElements(driver, pageClass);
		}
	}
}

