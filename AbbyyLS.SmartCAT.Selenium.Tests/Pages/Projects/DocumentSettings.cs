using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DocumentSettings : ProjectsPage, IAbstractPage<DocumentSettings>
	{
		public DocumentSettings(WebDriver driver) : base(driver)
		{
		}

		public new DocumentSettings GetPage()
		{
			var documentSettings = new DocumentSettings(Driver);
			InitPage(documentSettings, Driver);

			return documentSettings;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(TITLE)))
			{
				Assert.Fail("Произошла ошибка:\n не появился диалог настроек документа.");
			}
		}

		/// <summary>
		/// Задать имя документа
		/// </summary>
		public DocumentSettings SetDocumentName(string name)
		{
			CustomTestContext.WriteLine("Задать имя документа: '{0}'", name);
			Name.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения
		/// </summary>
		public T ClickSaveButton<T>(WebDriver driver) where T : class, IAbstractPage<T>
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения");
			SaveButton.Click();

			var instance = Activator.CreateInstance(typeof(T), new object[] { driver }) as T;
			return instance.GetPage();
		}

		/// <summary>
		/// Выбрать МТ в настройках документа
		/// </summary>
		public DocumentSettings SelectMachineTranslation(MachineTranslationType machineTranslationType)
		{
			
			var machineTranslationCheckbox = Driver.SetDynamicValue(How.XPath, MT_CHECKBOX, machineTranslationType.Description());

			CustomTestContext.WriteLine("Проверить, что Мachine Тranslation {0} не выбрано.", machineTranslationType);

			if (!machineTranslationCheckbox.Selected)
			{
				CustomTestContext.WriteLine("Выбрать Мachine Тranslation {0} в настройках документа.", machineTranslationType);
				machineTranslationCheckbox.Click();
			}

			return GetPage();
		}

		/// <summary>
		/// Выбрать глоссарий по имени
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public DocumentSettings ClickGlossaryByName(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий с именем {0}.", glossaryName);
			GlossaryCheckbox = Driver.SetDynamicValue(How.XPath, GLOSSARY_BY_NAME_XPATH, glossaryName);
			GlossaryCheckbox.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Навести курсор на таблицу глоссариев в диалоге настроек документа
		/// </summary>
		public DocumentSettings HoverGlossaryTableDocumentSettingsDialog()
		{
			CustomTestContext.WriteLine("Навести курсор на таблицу глоссариев в диалоге настроек документа.");
			GlossaryTable.HoverElement();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = NAME_INPUT)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = MT_CHECKBOX)]
		protected IWebElement MTCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_TABLE)]
		protected IWebElement GlossaryTable { get; set; }

		protected  IWebElement GlossaryCheckbox { get; set; }

		protected const string TITLE = "(//h2[text()='Document Settings'])[3]";
		protected const string NAME_INPUT = "//div[contains(@class,'document-settings')][3]//input[contains(@data-bind,'value: name')]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'g-popup-bd js-popup-bd js-popup-single-target-document-settings')][2]//span[contains(@data-bind,'click: save')]";
		protected const string MT_CHECKBOX = "//div[@class='g-popup-bd js-popup-bd js-popup-single-target-document-settings'][2]//tbody[contains(@data-bind, 'machineTranslators')]//p[text()='*#*']/parent::td/..//td//input";
		protected const string GLOSSARY_BY_NAME_XPATH = "(//h2[text()='Document Settings']//..//..//table[contains(@class,'l-corpr__tbl')]//tbody[@data-bind='foreach: glossaries']//tr[contains(string(), '*#*')])[1]//td//input";
		protected const string GLOSSARY_TABLE = "//div[contains(@class, 'single-target-document-settings')][2]//tbody[contains(@data-bind, 'glossaries')]";
	}
}