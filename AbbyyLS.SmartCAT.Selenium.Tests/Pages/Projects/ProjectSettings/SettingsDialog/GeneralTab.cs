using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class GeneralTab : ProjectSettingsDialog, IAbstractPage<GeneralTab>
	{
		public GeneralTab(WebDriver driver) : base(driver)
		{
		}

		public new GeneralTab GetPage()
		{
			var generalTab = new GeneralTab(Driver);
			InitPage(generalTab, Driver);

			return generalTab;
		}

		public new void LoadPage()
		{
			if (!IsGeneralTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылась вкладка General.");
			}
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку контроля качества.
		/// </summary>
		public QualityAssuranceSettings ClickQualityAssuranceSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку контроля качества.");
			SetUpQASettingsButton.Click();

			return new QualityAssuranceSettings(Driver).GetPage();
		}

		/// <summary>
		/// Поменять имя проекта.
		/// </summary>
		public GeneralTab EditName(string name)
		{
			CustomTestContext.WriteLine("Поменять имя проекта на {0}.", name);
			Name.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Поменять крайний срок.
		/// </summary>
		public GeneralTab EditDeadlineManually(string deadline)
		{
			CustomTestContext.WriteLine("Поменять крайний срок {0}.", deadline);
			Deadline.SetText(deadline);

			return GetPage();
		}

		/// <summary>
		/// Поменять описание проекта.
		/// </summary>
		public GeneralTab EditDescription(string description)
		{
			CustomTestContext.WriteLine("Поменять описание проекта на {0}.", description);
			Description.SetText(description);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась вкладка General.
		/// </summary>
		public bool IsGeneralTabOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(DEADLINE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = DEADLINE)]
		protected IWebElement Deadline { get; set; }

		[FindsBy(How = How.XPath, Using = SET_UP_QA_SETTINGS_BUTTON)]
		protected IWebElement SetUpQASettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = DESCRIPTION)]
		protected IWebElement Description { get; set; }

		#endregion
		
		#region Описания XPath элементов

		protected const string DEADLINE = "//input[contains(@class, 'hasDatepicker')]";
		protected const string SET_UP_QA_SETTINGS_BUTTON = "//div[contains(@class,'popup-edit')][2]//div[contains(@data-bind, 'setupQaSettings')]";
		protected const string NAME = "(//div[contains(@class,'js-popup-edit')])[2]//input[contains(@data-bind, 'value: name')]";
		protected const string DESCRIPTION = "(//div[contains(@class,'js-popup-edit')])[2]//textarea[contains(@data-bind, 'value: description')]";

		#endregion
	}
}
