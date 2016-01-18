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

		/// <summary>
		/// Переход на страницу авторизации sign-in.
		/// </summary>
		public void GoToSignInPage()
		{
			CustomTestContext.WriteLine("Переход на страницу авторизации: {0}.", ConfigurationManager.Url + RelativeUrlProvider.SignIn);

			Driver.Navigate().GoToUrl(ConfigurationManager.Url + RelativeUrlProvider.SignIn);
		}

		/// <summary>
		/// Переход на страницу курсеры.
		/// </summary>
		public void GoToCoursera()
		{
			CustomTestContext.WriteLine("Переход на страницу курсеры.", ConfigurationManager.CourseraUrl);

			Driver.Navigate().GoToUrl(ConfigurationManager.CourseraUrl);
		}

		/// <summary>
		/// Переход на страницу регистрации компаний /corp-reg.
		/// </summary>
		public void GoToCompanyRegistration()
		{
			CustomTestContext.WriteLine("Переход на страницу регистрации компаний: {0}.", ConfigurationManager.Url + RelativeUrlProvider.CorpReg);

			Driver.Navigate().GoToUrl(ConfigurationManager.Url + RelativeUrlProvider.CorpReg);
		}

		/// <summary>
		/// Переход на страницу регистрации фрилансеров /freelance-reg.
		/// </summary>
		public void GoToFreelanceRegistratioin()
		{
			CustomTestContext.WriteLine("Переход на страницу регистрации фрилансеров: {0}.", ConfigurationManager.Url + RelativeUrlProvider.FreelanceRegistratioin);

			Driver.Navigate().GoToUrl(ConfigurationManager.Url + RelativeUrlProvider.FreelanceRegistratioin);
		}

		/// <summary>
		/// Переход на страницу администрирования.
		/// </summary>
		public void GoToAdminUrl()
		{
			CustomTestContext.WriteLine("Переход на страницу администрирования: {0}", ConfigurationManager.AdminUrl);

			Driver.Navigate().GoToUrl(ConfigurationManager.AdminUrl);
		}

		/// <summary>
		/// Переход на страницу Workspace.
		/// </summary>
		public void GoToWorkspaceUrl(string workspaceUrl)
		{
			CustomTestContext.WriteLine("Переход на страницу Workspace: {0}.", workspaceUrl);

			Driver.Navigate().GoToUrl(workspaceUrl);
		}
	}
}
