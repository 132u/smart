using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class NewTranslationMemoryDialog : WorkspacePage, IAbstractPage<NewTranslationMemoryDialog>
	{
		public NewTranslationMemoryDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewTranslationMemoryDialog LoadPage()
		{
			if (!IsNewTranslationMemoryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не загрузилась форма создания ТМ");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести имя новой ТМ
		/// </summary>
		/// <param name="translationMemoryName">имя памяти перевода</param>
		public NewTranslationMemoryDialog SetTranslationMemoryName(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Ввести имя новой ТМ");
			NameField.SetText(translationMemoryName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать на поле названия ТМ
		/// </summary>
		public NewTranslationMemoryDialog ClickTranslationMemoryName()
		{
			CustomTestContext.WriteLine("Нажать на поле названия ТМ.");
			NameField.Click();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть / свернуть список исходных языков
		/// </summary>
		public NewTranslationMemoryDialog ClickSourceLanguagesList()
		{
			CustomTestContext.WriteLine("Раскрыть / свернуть список исходных языков");
			SourceLanguagesList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать исходный язык.
		/// </summary>
		/// <param name="language">исходный язык</param>
		public NewTranslationMemoryDialog SelectSourceLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать исходный язык {0}.", language);
			SourceLanguage = Driver.SetDynamicValue(How.XPath, SOURCE_LANGUAGES, ((int)language).ToString());
			SourceLanguage.Click();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть /свернуть список языков перевода
		/// </summary>
		public NewTranslationMemoryDialog ClickTargetLanguagesList()
		{
			CustomTestContext.WriteLine("Раскрыть / свернуть список языков перевода");
			TargetLanguagesList.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать язык перевода.
		/// </summary>
		/// <param name="language">язык перевода</param>
		public NewTranslationMemoryDialog SelectTargetLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык перевода {0}.", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANGUAGES, ((int)language).ToString());
			TargetLanguage.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть список клиентов
		/// </summary>
		public NewTranslationMemoryDialog OpenClientsList()
		{
			CustomTestContext.WriteLine("Раскрыть список клиентов");
			Driver.WaitUntilElementIsClickable(By.XPath(CLIENT));
			CreateClientDropDown.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(CLIENT_LIST));

			return LoadPage();
		}

		/// <summary>
		/// Выбрать клиента в дропдауне.
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog ClickClientOption(string clientName)
		{
			CustomTestContext.WriteLine("Выбрать клиента {0} в дропдауне.", clientName);
			ClientOption = Driver.SetDynamicValue(How.XPath, CLIENT_OPTION, clientName);
			ClientOption.Click();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проектов в дропдауне.
		/// </summary>
		/// <param name="projectGroup">имя группы проектов</param>
		public NewTranslationMemoryDialog ClickProjectGroupOption(string projectGroup)
		{
			CustomTestContext.WriteLine("Выбрать группу проектов {0} в дропдауне.", projectGroup);
			ProjectOption = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_OPTION, projectGroup);
			ProjectOption.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения ТМ
		/// </summary>
		public TranslationMemoriesPage ClickSaveTranslationMemory()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения ТМ");
			SaveTranslationMemoryButton.JavaScriptClick();

			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения ТМ, ожидая ошибку
		/// </summary>
		public NewTranslationMemoryDialog ClickSaveTranslationMemoryExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения ТМ, ожидая ошибку");
			SaveTranslationMemoryButton.JavaScriptClick();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку отмены сохранения ТМ
		/// </summary>
		public TranslationMemoriesPage ClickCancelTMCreation()
		{
			CustomTestContext.WriteLine("Нажать кнопку отмены сохранения ТМ");
			CancelTranslationMemoryCreation.JavaScriptClick();
			
			return new TranslationMemoriesPage(Driver).LoadPage();
		}

		/// <summary>
		/// Загрузить ТМX документ во время создания создания ТМ.
		/// </summary>
		/// <param name="pathToFile">путь к файлу</param>
		public NewTranslationMemoryDialog UploadFile(string pathToFile)
		{
			makeInputDialogVisible();
			SetFileName(pathToFile);
			initializeHiddenElementForValidation();
			fileNameValidation();

			return LoadPage();
		}

		/// <summary>
		/// Раскрыть список групп проектов при создании ТМ
		/// </summary>
		public NewTranslationMemoryDialog OpenProjectGroupsList()
		{
			CustomTestContext.WriteLine("Открыть список групп проектов  при создании ТМ.");
			ProjectGroupsDropDown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="filePath">путь до файла</param>
		public NewTranslationMemoryDialog SetFileName(string filePath)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", filePath);
			UploadFileField.SendKeys(filePath);

			return LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog SelectClient(string clientName)
		{
			OpenClientsList();
			ClickClientOption(clientName);
			ClickTranslationMemoryName();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проектов
		/// </summary>
		/// <param name="projectGroup">имя клиента</param>
		public NewTranslationMemoryDialog SelectProjectGroup(string projectGroup)
		{
			OpenProjectGroupsList();
			ClickProjectGroupOption(projectGroup);
			ClickTranslationMemoryName();

			return LoadPage();
		}
		/// <summary>
		/// Выбрать target язык
		/// </summary>
		/// <param name="language">язык таргета</param>
		public NewTranslationMemoryDialog SetTargetLanguage(Language language)
		{
			ClickTargetLanguagesList();
			SelectTargetLanguage(language);
			ClickTargetLanguagesList();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать source язык
		/// </summary>
		/// <param name="language">язык таргета</param>
		public NewTranslationMemoryDialog SetSourceLanguage(Language language)
		{
			ClickSourceLanguagesList();
			SelectSourceLanguage(language);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, загрузился ли диалог создания TM
		/// </summary>
		public bool IsNewTranslationMemoryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_BUTTON), timeout: 20);
		}

		/// <summary>
		/// Проверить, что клиент присутствует в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public bool IsClientExistInList(string clientName)
		{
			CustomTestContext.WriteLine("Проверить, что клиент {0} присутствует в списке клиентов при создании ТМ", clientName);
			var clientList = Driver.GetElementList(By.XPath(CLIENT_ITEM));

			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		/// <summary>
		/// Проверить, что список групп проектов открылся при создании ТМ
		/// </summary>
		public bool IsProjectGroupsListDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что список групп проектов открылся при создании ТМ");

			return ProjectGroupsList.Displayed;
		}

		/// <summary>
		/// Проверить, что группа проектов присутствует в списке при создании ТМ
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public bool IsProjectGroupExistInList(string projectGroupName)
		{
			CustomTestContext.WriteLine("Проверить, что группа проектов {0} присутствует в списке при создании ТМ", projectGroupName);
			var projectGroupsList = Driver.GetElementList(By.XPath(PROJECT_GROUPS_LIST));

			return projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);
		}

		/// <summary>
		/// Проверить, что появилась ошибка при создании ТМ с существующим именем
		/// </summary>
		public bool IsExistingNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка при создании ТМ с существующим именем");
			
			return ErrorTranslationMemoryNameExist.Displayed;
		}

		/// <summary>
		/// Проверить, что появилась ошибка при создании ТМ с пустым именем
		/// </summary>
		public bool IsEmptyNameErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилась ошибка при создании ТМ с пустым именем");

			return ErrorTranslationMemoryWithoutName.Displayed;
		}

		/// <summary>
		/// Проверить, что имеется ошибка при создании ТМ без языка перевода
		/// </summary>
		public bool IsNoTargetErrorMessageDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что имеется ошибка при создании ТМ без языка перевода");

			return ErrorTranslationMemoryWithoutTarget.Displayed;
		}

		/// <summary>
		/// Проверить, что имеется ошибка о загрузке файла с неподходящим расширением (не TMX файл)
		/// </summary>
		public bool IsWrongFormatErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что имеется ошибка о загрузке файла с неподходящим расширением (не TMX файл)");

			return ErrorTranslationMemoryWithNotTmx.Displayed;
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private NewTranslationMemoryDialog makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");

			return LoadPage();
		}

		/// <summary>
		/// Инициализировать скрытый элемнт, необходимый для загрузки документа
		/// </summary>
		private NewTranslationMemoryDialog initializeHiddenElementForValidation()
		{
			CustomTestContext.WriteLine("Инициализировать скрытый элемнт, необходимый для загрузки документа");
			Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").filenameLink.text($(\".js-import-file-form .js-control\").data(\"controller\").fileInput.val());");

			return LoadPage();
		}

		/// <summary>
		/// Выполнить скрипт для прохождения валидации импорта
		/// </summary>
		private NewTranslationMemoryDialog fileNameValidation()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для прохождения валидации импорта");
			Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").trigger(\"valueChanged\");");

			return LoadPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = TM_NAME_FIELD)]
		protected IWebElement NameField { get; set; }

		[FindsBy(How = How.XPath, Using = OPEN_TARGET_LANGUAGES)]
		protected IWebElement TargetLanguagesList { get; set; }

		[FindsBy(How = How.XPath, Using = OPEN_SOURCE_LANGUAGES)]
		protected IWebElement SourceLanguagesList { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT)]
		protected IWebElement CreateClientDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_LIST)]
		protected IWebElement CreateClientList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_FILTER)]
		protected IWebElement ProjectGroupsDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUPS_LIST)]
		protected IWebElement ProjectGroupsList { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveTranslationMemoryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelTranslationMemoryCreation { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_EXISTING_NAME)]
		protected IWebElement ErrorTranslationMemoryNameExist { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NO_NAME)]
		protected IWebElement ErrorTranslationMemoryWithoutName { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NO_TARGET)]
		protected IWebElement ErrorTranslationMemoryWithoutTarget { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_NO_TMX_FILE)]
		protected IWebElement ErrorTranslationMemoryWithNotTmx { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_FIELD)]
		protected IWebElement UploadFileField { get; set; }
		protected IWebElement SourceLanguage { get; set; }
		protected IWebElement TargetLanguage { get; set; }
		protected IWebElement ClientOption { get; set; }
		protected IWebElement ProjectOption { get; set; }
		#endregion

		#region Описания XPath элементов

		protected const string PROJECT_GROUPS_LIST = ".//div[contains(@class,'ui-multiselect-menu')][2]/ul//span[2]";
		protected const string PROJECT_GROUP_FILTER= "//select[contains(@data-bind,'allDomainsList')]//following-sibling::div";
		protected const string PROJECT_GROUP_OPTION = "//div[contains(@class, 'js-popup-create-tm')][2]//span//input[contains(@title,'*#*')]";

		protected const string CLIENT = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span";
		protected const string CLIENT_LIST = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span[contains(@class,'active')]";
		protected const string CLIENT_ITEM = "//select[contains(@data-bind,'allClientsList')]/option";
		protected const string CLIENT_OPTION = "//span[contains(@class, 'js-dropdown__list boxtype')]//span[contains(@title,'*#*')]";

		protected const string TM_NAME_FIELD = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'name')]";
		protected const string SAVE_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@data-bind, 'click: save')]";
		protected const string CANCEL_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//a[string()='Cancel']";

		protected const string OPEN_SOURCE_LANGUAGES = "//div[contains(@class,'js-popup-create-tm')][2]//select[contains(@data-bind,'SourceLanguagesList')]/following-sibling::span";
		protected const string OPEN_TARGET_LANGUAGES = "//div[contains(@class,'js-popup-create-tm')][2]//select[contains(@data-bind,'TargetLanguagesList')]/following-sibling::div";
		protected const string TARGET_LANGUAGES = "//div[contains(@class,'ui-multiselect')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";
		protected const string SOURCE_LANGUAGES = "//span[contains(@class,'js-dropdown__item')][@data-id='*#*']";

		protected const string ERROR_EXISTING_NAME = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(text(),'A translation memory with this name already exists.')]";
		protected const string ERROR_NO_NAME = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_NO_TARGET = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id,'target-language-required')]";
		protected const string ERROR_NO_TMX_FILE = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id,'invalid-file-extension')]";

		protected const string UPLOAD_FILE_FIELD = ".//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";

		#endregion
	}
}
