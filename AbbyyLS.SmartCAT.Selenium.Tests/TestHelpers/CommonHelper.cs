using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class CommonHelper : BaseObject
	{
		//TODO: рассмотреть вариант сделать класс абстрактным и унаследовать от него все хэлперы

		public WebDriver Driver { get; private set; }

		public CommonHelper(WebDriver driver)
		{
			Driver = driver;
		}

		public void GoToSignInPage()
		{
			Driver.Navigate().GoToUrl(ConfigurationManager.Url + RelativeUrlProvider.SignIn);
		}

		public void GoToCompanyRegistration()
		{
			Driver.Navigate().GoToUrl(ConfigurationManager.Url + RelativeUrlProvider.CorpReg);
		}

		public void GoToAdminUrl()
		{
			Driver.Navigate().GoToUrl(ConfigurationManager.AdminUrl);
		}

		public void GoToWorkspaceUrl(string workspaceUrl)
		{
			Driver.Navigate().GoToUrl(workspaceUrl);
		}
	}
}
