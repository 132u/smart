using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	class LicensesTab : UsersAndRightsBasePage, IAbstractPage<LicensesTab>
	{
		public LicensesTab(WebDriver driver) : base(driver)
		{
		}

		public new LicensesTab GetPage()
		{
			var licensesTab = new LicensesTab(Driver);
			InitPage(licensesTab, Driver);

			return licensesTab;
		}

		public new void LoadPage()
		{
			if (!IsLicensesTabOpened())
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
		/// Проверить, что открылась вкладка Лицензий
		/// </summary>
		public bool IsLicensesTabOpened()
		{
			return LicensesTableHeader.Displayed;
		}

		#endregion


		#region Вспомогательные методы


		#endregion


		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = LICENSES_TABLE_HEADER)]
		protected IWebElement LicensesTableHeader { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string LICENSES_TABLE_HEADER = "//div[contains(@class, 'lic__tabs')]";

		#endregion
	}
}
