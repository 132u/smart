using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	class CrowdsourcersTab : UsersAndRightsBasePage, IAbstractPage<CrowdsourcersTab>
	{
		public CrowdsourcersTab(WebDriver driver) : base(driver)
		{
		}

		public new CrowdsourcersTab GetPage()
		{
			var crowdsourcersTab = new CrowdsourcersTab(Driver);
			InitPage(crowdsourcersTab, Driver);

			return crowdsourcersTab;
		}

		public new void LoadPage()
		{
			if (!IsCrowdsourcersTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог добавления права.");
			}
		}

		#region Простые методы страницы


		#endregion

		#region Составные методы страницы


		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась вкладка Crowdsourcers
		/// </summary>
		public bool IsCrowdsourcersTabOpened()
		{
			return CrowdsourcesTableHeader.Displayed;
		}

		#endregion

		#region Вспомогательные методы
		
		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CROWDSOURCES_TABLE_HEADER)]
		protected IWebElement CrowdsourcesTableHeader { get; set; }

		#endregion
		
		#region Описание XPath элементов

		protected const string CROWDSOURCES_TABLE_HEADER = "//div[contains(@class, 'crowd-management__left')]";

		#endregion
	}
}
