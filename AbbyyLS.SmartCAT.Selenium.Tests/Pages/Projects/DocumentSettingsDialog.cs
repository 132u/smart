using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class DocumentSettingsDialog : ProjectsPage, IAbstractPage<DocumentSettingsDialog>
	{
		public DocumentSettingsDialog(WebDriver driver) : base(driver)
		{
		}

		public new DocumentSettingsDialog LoadPage()
		{
			if (!IsDocumentSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог настроек документа");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Задать имя документа
		/// </summary>
		public DocumentSettingsDialog SetDocumentName(string name)
		{
			CustomTestContext.WriteLine("Задать имя документа: '{0}'", name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения, ожидая страницу настрое проекта
		/// </summary>
		public ProjectSettingsPage ClickSaveButtonExpectingProjectSettingsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения, ожидая страницу настрое проекта");
			SaveButtonProjectSettingsPage.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения, ожидая страницу со списком проектов
		/// </summary>
		public ProjectsPage ClickSaveButtonExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения, ожидая страницу со списком проектов");
			SaveButtonProjectsPage.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Выбрать МТ в настройках документа
		/// </summary>
		public DocumentSettingsDialog SelectMachineTranslation(
			MachineTranslationType machineTranslationType = MachineTranslationType.SmartCATTranslator)
		{

			MachineTranslationCheckbox = Driver.SetDynamicValue(How.XPath, MT_CHECKBOX_INPUT,
				machineTranslationType.Description());

			CustomTestContext.WriteLine("Проверить, что Мachine Тranslation {0} не выбрано.", machineTranslationType);

			if (!MachineTranslationCheckbox.Selected)
			{
				CustomTestContext.WriteLine("Выбрать Мachine Тranslation {0} в настройках документа.", machineTranslationType);
				MachineTranslationCheckbox.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Выбрать глоссарий по имени
		/// </summary>
		/// <param name="glossaryName">имя глоссария</param>
		public DocumentSettingsDialog ClickGlossaryByName(string glossaryName)
		{
			CustomTestContext.WriteLine("Выбрать глоссарий с именем {0}.", glossaryName);
			GlossaryCheckbox = Driver.SetDynamicValue(How.XPath, GLOSSARY_BY_NAME_XPATH, glossaryName);
			GlossaryCheckbox.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Навести курсор на таблицу глоссариев в диалоге настроек документа
		/// </summary>
		public DocumentSettingsDialog HoverGlossaryTableDocumentSettingsDialog()
		{
			CustomTestContext.WriteLine("Навести курсор на таблицу глоссариев в диалоге настроек документа.");
			GlossaryTable.HoverElement();

			return LoadPage();
		}

		/// <summary>
		/// Убрать галочку для Мachine Тranslation
		/// </summary>>
		public DocumentSettingsDialog UnselectMachineTranslation(
			MachineTranslationType machineTranslationType = MachineTranslationType.SmartCATTranslator)
		{
			CustomTestContext.WriteLine("Проверить, что Мachine Тranslation {0} выбрано.", machineTranslationType);
			MachineTranslationCheckbox = Driver.SetDynamicValue(How.XPath, MT_CHECKBOX_INPUT,
				machineTranslationType.Description());

			if (MachineTranslationCheckbox.Selected)
			{
				CustomTestContext.WriteLine("Убрать галочку для Мachine Тranslation {0} в настройках документа.",
					machineTranslationType);
				MachineTranslationCheckbox.Click();
			}

			return LoadPage();
		}

		/// <summary>
		/// Закрыть страницу настроек проекта
		/// </summary>
		/// <param name="projectName">имя проекта</param>
		public ProjectsPage CloseDocumentSettings(string projectName)
		{
			ClickSaveButtonExpectingProjectsPage();
			WaitUntilProjectLoadSuccessfully(projectName);

			return new ProjectsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что стоит галочка в чекбоксе машинного перевода
		/// </summary>
		/// <param name="machineTranslationType">тип машинного перевода</param>
		public bool IsMachineTranslationSelected(
			MachineTranslationType machineTranslationType = MachineTranslationType.SmartCATTranslator)
		{
			CustomTestContext.WriteLine("Проверить, что стоит галочка в чекбоксе машинного перевода типа {0}.",
				machineTranslationType);
			MachineTranslationCheckbox = Driver.SetDynamicValue(How.XPath, MT_CHECKBOX_INPUT,
				machineTranslationType.Description());

			return MachineTranslationCheckbox.Selected;
		}

		/// <summary>
		/// Проверить, открылся ли диалог настроек документа
		/// </summary>
		public bool IsDocumentSettingsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TITLE));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = NAME_INPUT)]
		protected IWebElement Name { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON_PROJECT_SETTINGS_PAGE)]
		protected IWebElement SaveButtonProjectSettingsPage { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON_PROJECTS_PAGE)]
		protected IWebElement SaveButtonProjectsPage { get; set; }

		[FindsBy(How = How.XPath, Using = MT_CHECKBOX_INPUT)]
		protected IWebElement MTCheckbox { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_TABLE)]
		protected IWebElement GlossaryTable { get; set; }

		protected IWebElement GlossaryCheckbox { get; set; }

		protected IWebElement MachineTranslationCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string TITLE = "(//h2[text()='Document Settings'])[3]";
		protected const string NAME_INPUT = "//div[contains(@class,'document-settings')][3]//input[contains(@data-bind,'value: name')]";
		protected const string SAVE_BUTTON_PROJECT_SETTINGS_PAGE = "//div[contains(@class,'single-target-document-settings')][2]//div[contains(@data-bind,'save')]";
		protected const string SAVE_BUTTON_PROJECTS_PAGE = "//div[contains(@class,'single-target-document-settings')][2]//div[contains(@data-bind,'save')]";
		protected const string MT_CHECKBOX_INPUT = "//span[text()='*#*']/../../preceding-sibling::td//input";
		protected const string MT_CHECKBOX = "//span[text()='*#*']/../../preceding-sibling::td";
		protected const string GLOSSARY_BY_NAME_XPATH = "(//h2[text()='Document Settings']//..//..//table[contains(@class,'l-corpr__tbl')]//tbody[@data-bind='foreach: glossaries']//tr[contains(string(), '*#*')])[1]//td//input";
		protected const string GLOSSARY_TABLE = "//div[contains(@class, 'single-target-document-settings')][2]//tbody[contains(@data-bind, 'glossaries')]";

		#endregion
	}
}