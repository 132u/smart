using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Support
{
	public class HelpPage : WorkspacePage, IAbstractPage<HelpPage>
	{
		public HelpPage(WebDriver driver) : base(driver)
		{
		}

		public new HelpPage LoadPage()
		{
			if (!IsHelpPageOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти на страницу 'Справка'"); 
			}

			return this;
		}

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылась ли страница
		/// </summary>
		public bool IsHelpPageOpened()
		{
			Driver.WaitUntilElementIsDisplay(By.XPath(FRAME_CONTENT));
			Driver.SwitchTo().Frame(Driver.FindElement(By.XPath(FRAME_CONTENT)));

			if (Driver.WaitUntilElementIsDisplay(By.XPath(ABOUT_SMART_CAT)))
			{
				Driver.SwitchTo().DefaultContent();
				return true;
			}

			return false;
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = FRAME_CONTENT)]
		protected IWebElement FrameContent { get; set; }

		[FindsBy(How = How.XPath, Using = ABOUT_SMART_CAT)]
		protected IWebElement AboutSmartCat { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string FRAME_CONTENT = "//frame[contains(@name,'hmcontent')]";
		protected const string ABOUT_SMART_CAT = "//h1[contains(@class, 'p_Heading1')]//span['About SmartCAT']";

		#endregion
	}
}