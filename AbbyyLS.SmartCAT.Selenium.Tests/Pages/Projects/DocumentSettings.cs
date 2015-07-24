using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DocumentSettings : ProjectsPage, IAbstractPage<DocumentSettings>
	{
		public new DocumentSettings GetPage()
		{
			var documentSettings = new DocumentSettings();
			InitPage(documentSettings);

			return documentSettings;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(NAME_INPUT)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог настроек документа.");
			}
		}

		/// <summary>
		/// Задать имя документа
		/// </summary>
		public DocumentSettings SetDocumentName(string name)
		{
			Logger.Debug("Задать имя документа: '{0}'", name);
			Name.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения
		/// </summary>
		public ProjectsPage ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку сохранения");
			SaveButton.Click();

			return new ProjectsPage().GetPage();
		}

		/// <summary>
		/// Выбрать МТ {0} в настройках документа
		/// </summary>
		public DocumentSettings SelectMachineTranslation(MachineTranslationType machineTranslationType)
		{
			Logger.Debug("Выбрать Мachine Тranslation {0} в настройках документа.");
			var machineTranslationCheckbox = Driver.SetDynamicValue(How.XPath, MT_CHECKBOX, machineTranslationType.Description());

			if (!machineTranslationCheckbox.Selected)
			{
				machineTranslationCheckbox.Click();
			}

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = NAME_INPUT)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = MT_CHECKBOX)]
		protected IWebElement MTCheckbox { get; set; }

		protected const string NAME_INPUT = "//div[contains(@class,'js-popup-document-settings')][2]//input[contains(@data-bind,'value: name')]";

		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-document-settings')][2]//span[contains(@data-bind,'click: save')]";

		protected const string MT_CHECKBOX = "//div[@class='g-popup-bd js-popup-bd js-popup-document-settings'][2]//tbody[contains(@data-bind, 'machineTranslators')]//p[text() = '*#*']//preceding::td[1]//input";
	}
}