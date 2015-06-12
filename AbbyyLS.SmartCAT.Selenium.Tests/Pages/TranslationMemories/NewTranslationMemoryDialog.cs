using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class NewTranslationMemoryDialog : TranslationMemoriesPage, IAbstractPage<NewTranslationMemoryDialog>
	{
		public new NewTranslationMemoryDialog GetPage()
		{
			var tmCreationPage = new NewTranslationMemoryDialog();
			InitPage(tmCreationPage);

			return tmCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_TM_BUTTON), timeout: 20))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась форма создания ТМ.");
			}
		}

		/// <summary>
		/// Ввести имя новой ТМ
		/// </summary>
		public NewTranslationMemoryDialog SetTranslationMemoryName(string name)
		{
			Logger.Debug("Ввести имя новой ТМ");
			NameField.SetText(name);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку открытия списка исходных языков
		/// </summary>
		public NewTranslationMemoryDialog ClickOpenSourceLanguageList()
		{
			Logger.Debug("Кликнуть кнопку открытия списка исходных языков");
			SourceLanguagesList.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать исходный язык
		/// </summary>
		public NewTranslationMemoryDialog SelectSourceLanguage(Language language)
		{
			Logger.Debug("Выбрать исходный язык {0}", language);
			SourceLanguage = Driver.SetDynamicValue(How.XPath, SOURCE_LANG, ((int)language).ToString());
			SourceLanguage.Click();

			return GetPage();
		}

		/// <summary>
		/// Кликнуть список языков перевода
		/// </summary>
		public NewTranslationMemoryDialog ClickTargetLanguageList()
		{
			Logger.Debug("Кликнуть список языков перевода");
			TargetLanguagesList.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык перевода
		/// </summary>
		public NewTranslationMemoryDialog SelectTargetLanguage(Language language)
		{
			Logger.Debug("Выбрать язык перевода {0}", language);
			TargetLanguage = Driver.SetDynamicValue(How.XPath, TARGET_LANG, ((int)language).ToString());
			TargetLanguage.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку открытия списка клиентов
		/// </summary>
		public NewTranslationMemoryDialog ClickOpenClientsList()
		{
			Logger.Debug("Нажать кнопку открытия списка клиентов");
			CreateClientDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения ТМ
		/// </summary>
		public T ClickSaveTranslationMemory<T>() where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку сохранения ТМ");
			SaveTranslationMemoryButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку отмены сохранения ТМ
		/// </summary>
		public TranslationMemoriesPage ClickCancelTranslationMemoryCreation()
		{
			Logger.Debug("Нажать кнопку отмены сохранения ТМ");
			CancelTranslationMemoryCreation.Click();
			
			return new TranslationMemoriesPage().GetPage();
		}

		/// <summary>
		/// Проверить отображения списка клиентов
		/// </summary>
		public NewTranslationMemoryDialog AssertClientsListDisplayed()
		{
			Logger.Trace("Проверить отображения списка клиентов");
			
			Assert.IsTrue(CreateClientList.Displayed,
				"Произошла ошибка:\n не отображен список клиентов.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент присутствует в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog AssertClientExistInTmCreationDialog(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} имеется в списке клиентов при создании ТМ", clientName);

			Assert.IsTrue(getIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} не отображен в списке клиентов.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент отсутствует в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog AssertClientNotExistInTmCreationDialog(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} отсутствует в списке клиентов при создании ТМ", clientName);

			Assert.IsFalse(getIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} имеется в списке клиентов.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Загрузить ТМX документ во время создания создания ТМ
		/// </summary>
		public NewTranslationMemoryDialog UploadFile(string pathToFile)
		{
			Logger.Debug(string.Format("Загрузить ТМX документ {0} во время создания создания ТМ", pathToFile));
			UploadFileField.UploadFile(pathToFile);

			return GetPage();
		}

		/// <summary>
		/// Открыть список групп проектов при создании ТМ
		/// </summary>
		public NewTranslationMemoryDialog OpenProjectGroupsList()
		{
			Logger.Debug("Открыть список групп проектов  при создании ТМ.");
			ProjectGroupsDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список групп проектов открылся при создании ТМ
		/// </summary>
		public NewTranslationMemoryDialog AssertProjectGroupListDisplayed()
		{
			Logger.Trace("Проверить, что список групп проектов открылся при создании ТМ.");

			Assert.IsTrue(ProjectGroupsList.Displayed,
				"Произошла ошибка:\n список групп проектов не открылся при создании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов присутствует в списке при создании ТМ
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewTranslationMemoryDialog AssertProjectGroupExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов присутствует в списке при создании ТМ.");
			var projectGroupsList = Driver.GetElementList(By.XPath(CREATE_TM_PROJECT_GROUPS_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsTrue(projectGroupExist, "Произошла ошибка:\n группа проектов {0} отсутствует в списке при создании ТМ.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов отсутствует в списке при создании ТМ
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewTranslationMemoryDialog AssertProjectGroupNotExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов отсутствует в списке при создании ТМ.");
			var projectGroupsList = Driver.GetElementList(By.XPath(CREATE_TM_PROJECT_GROUPS_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsFalse(projectGroupExist, "Произошла ошибка:\n группа проектов {0} присутствует в списке при создании ТМ.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что имеется ошибка при создании ТМ с существующим именем
		/// </summary>
		public NewTranslationMemoryDialog AssertExistNameErrorAppear()
		{
			Logger.Trace("Проверить, что имеется ошибка при создании ТМ с существующим именем");

			Assert.IsTrue(ErrorTranslationMemoryNameExist.Displayed,
				"Произошла ошибка:\n не появилась ошибка создания ТМ с существующим именем");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что имеется ошибка при создании ТМ с пустым именем
		/// </summary>
		public NewTranslationMemoryDialog AssertNoNameErrorAppear()
		{
			Logger.Trace("Проверить, что имеется ошибка при создании ТМ с пустым именем");

			Assert.IsTrue(ErrorTranslationMemoryWithoutName.Displayed,
				"Произошла ошибка:\n не появилась ошибка создания ТМ с пустым именем");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что имеется ошибка при создании ТМ без языка перевода
		/// </summary>
		public NewTranslationMemoryDialog AssertNoTargetErrorAppear()
		{
			Logger.Trace("Проверить, что имеется ошибка при создании ТМ без языка перевода");

			Assert.IsTrue(ErrorTranslationMemoryWithoutTarget.Displayed,
				"Произошла ошибка:\n не появилась ошибка при создании ТМ без языка перевода");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что имеется ошибка о загрузке не TMX файла
		/// </summary>
		public NewTranslationMemoryDialog AssertNotTmxFileErrorAppear()
		{
			Logger.Trace("Проверить, что имеется ошибка о загрузке не TMX файла");

			Assert.IsTrue(ErrorTranslationMemoryWithNotTmx.Displayed,
				"Произошла ошибка:\n не появилась ошибка о загрузке не TMX файла");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент присутствует в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		private static bool getIsClientExist(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} присутствует в списке", clientName);
			var clientList = Driver.GetElementList(By.XPath(CREATE_TM_CLIENT_ITEM));

			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		[FindsBy(How = How.XPath, Using = TM_NAME_FIELD)]
		protected IWebElement NameField { get; set; }

		[FindsBy(How = How.XPath, Using = OPEN_TRG_LANG)]
		protected IWebElement TargetLanguagesList { get; set; }

		[FindsBy(How = How.XPath, Using = OPEN_SRC_LANG)]
		protected IWebElement SourceLanguagesList { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT)]
		protected IWebElement CreateClientDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT_LIST)]
		protected IWebElement CreateClientList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_FILTER)]
		protected IWebElement ProjectGroupsDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_PROJECT_GROUPS_LIST)]
		protected IWebElement ProjectGroupsList { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TM_BUTTON)]
		protected IWebElement SaveTranslationMemoryButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_TM_CREATION_BUTTON)]
		protected IWebElement CancelTranslationMemoryCreation { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_CREATE_TM_EXIST_NAME)]
		protected IWebElement ErrorTranslationMemoryNameExist { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_CREATE_TM_NO_NAME)]
		protected IWebElement ErrorTranslationMemoryWithoutName { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_CREATE_TM_NO_TARGET)]
		protected IWebElement ErrorTranslationMemoryWithoutTarget { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_CREATE_TM_NO_TMX_FILE)]
		protected IWebElement ErrorTranslationMemoryWithNotTmx { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_FILE_FIELD)]
		protected IWebElement UploadFileField { get; set; }

		protected IWebElement SourceLanguage { get; set; }

		protected IWebElement TargetLanguage { get; set; }

		protected const string CREATE_TM_PROJECT_GROUPS_LIST = ".//div[contains(@class,'ui-multiselect-menu')][2]/ul//span[2]";
		protected const string PROJECT_GROUP_FILTER= "//select[contains(@data-bind,'allDomainsList')]//following-sibling::div";
		protected const string CREATE_TM_PROJECT_GROUP = ".//div[contains(@class,'ui-multiselect-text')]//span[contains(text(), 'Select project group')]";
		protected const string CREATE_TM_CLIENT = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span";
		protected const string CREATE_TM_CLIENT_LIST = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span[contains(@class,'active')]";
		protected const string CREATE_TM_CLIENT_ITEM = "//select[contains(@data-bind,'allClientsList')]/option";

		protected const string TM_NAME_FIELD = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'name')]";
		protected const string SAVE_TM_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//span[contains(@data-bind, 'click: save')]";
		protected const string CANCEL_TM_CREATION_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//a[string()='Cancel']";

		protected const string OPEN_SRC_LANG = "//div[contains(@class,'js-popup-create-tm')][2]//select[contains(@data-bind,'SourceLanguagesList')]/following-sibling::span";
		protected const string OPEN_TRG_LANG = "//div[contains(@class,'js-popup-create-tm')][2]//select[contains(@data-bind,'TargetLanguagesList')]/following-sibling::div";
		protected const string TARGET_LANG = "//div[contains(@class,'ui-multiselect')]//ul[@class='ui-multiselect-checkboxes ui-helper-reset']//li//input[@value='*#*']";
		protected const string SOURCE_LANG = "//span[contains(@class,'js-dropdown__item')][@data-id='*#*']";

		protected const string ERROR_CREATE_TM_EXIST_NAME = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(text(),'The name should be unique.')]";
		protected const string ERROR_CREATE_TM_NO_NAME = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id, 'name-required')]";
		protected const string ERROR_CREATE_TM_NO_TARGET = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id,'target-language-required')]";
		protected const string ERROR_CREATE_TM_NO_TMX_FILE = "//div[contains(@class,'js-popup-create-tm')][2]//div[contains(@class,'createtm__error')]//p[contains(@data-message-id,'invalid-file-extension')]";

		protected const string UPLOAD_FILE_FIELD = ".//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";
	}
}
