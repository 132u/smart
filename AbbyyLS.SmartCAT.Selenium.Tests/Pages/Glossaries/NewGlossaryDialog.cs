using System;
using System.Linq;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class NewGlossaryDialog : GlossariesPage, IAbstractPage<NewGlossaryDialog>
	{
		public NewGlossaryDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewGlossaryDialog GetPage()
		{
			var glossaryCreationPage = new NewGlossaryDialog(Driver);
			InitPage(glossaryCreationPage, Driver);

			return glossaryCreationPage;
		}

		public new void LoadPage()
		{
			if (!IsNewGlossaryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог создания глоссария");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Открыть список клиентов при создании глоссария
		/// </summary>
		public NewGlossaryDialog OpenClientsList()
		{
			CustomTestContext.WriteLine("Открыть список клиентов при создании глоссария");
			ClientsList.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(DROPDOWN_LIST));

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления языка
		/// </summary>
		/// <param name="dropdown">порядковый номер</param>
		public NewGlossaryDialog ClickDeleteLanguageButton(int dropdown = 1)
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления языка.");
			DeleteLanguageButton = Driver.SetDynamicValue(How.XPath, DELETE_LANGUAGE_BUTTON, dropdown.ToString());
			DeleteLanguageButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество языков
		/// </summary>
		public int GetGlossaryLanguageCount()
		{
			CustomTestContext.WriteLine("Получить количество языков в глоссарии.");

			return Driver.GetElementsCount(By.XPath(LANGUAGES_DROPDOWNS)) - 1;
		}

		/// <summary>
		/// Открыть список групп проектов
		/// </summary>
		public NewGlossaryDialog ClickProjectGroupsList()
		{
			CustomTestContext.WriteLine("Открыть список групп проектов при создании глоссария.");
			ProjectGroupsDropDown.Click();
			Driver.WaitUntilElementIsDisplay(By.XPath(MULTISELECT_LIST));

			return GetPage();
		}

		/// <summary>
		/// Выбрать группу проектов списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewGlossaryDialog SelectProjectGroup(string projectGroupName)
		{
			CustomTestContext.WriteLine("Выбрать группу проектов списке");
			ProjectGroupsItem = Driver.SetDynamicValue(How.XPath, PROJECT_GROUPS_ITEM, projectGroupName);
			ProjectGroupsItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести название глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public NewGlossaryDialog FillGlossaryName(string glossaryName)
		{
			CustomTestContext.WriteLine("Ввести {0} название глоссария", glossaryName);
			GlossaryName.SetText(glossaryName);

			return GetPage();
		}

		/// <summary>
		/// Ввести комментарий
		/// </summary>
		/// <param name="comment">текст комментария</param>
		public NewGlossaryDialog FillComment(string comment)
		{
			CustomTestContext.WriteLine("Ввести комментарий {0}.", comment);
			GlossaryComment.SetText(comment);

			return GetPage();
		}

		/// <summary>
		/// Раскрыть дропдаун выбора языка
		/// </summary>
		/// <param name="dropdownNumber">номер дропдауна</param>
		public NewGlossaryDialog ExpandLanguageDropdown(int dropdownNumber)
		{
			CustomTestContext.WriteLine("Раскрыть дропдаун выбора для {0} языка.", dropdownNumber);
			var languageDropdown = Driver.SetDynamicValue(How.XPath, LANGUAGE_DROPDOWN, dropdownNumber.ToString());

			languageDropdown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать язык в дропдауне
		/// </summary>
		/// <param name="language">язык</param>
		public NewGlossaryDialog SelectLanguage(Language language)
		{
			CustomTestContext.WriteLine("Выбрать язык {0} в дропдауне.", language);
			var languageOption = Driver.SetDynamicValue(How.XPath, LANGUAGES_LIST, language.ToString());
			languageOption.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add
		/// </summary>
		public NewGlossaryDialog ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add.");
			AddButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения глоссария
		/// </summary>
		public GlossaryPage ClickSaveGlossaryButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения глоссария.");
			GlossarySaveButton.Click();

			return new GlossaryPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения глоссария, ожидая сообщение об ошибке
		/// </summary>
		public NewGlossaryDialog ClickSaveGlossaryButtonExpectingError()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения глоссария, ожидая сообщение об ошибке");
			GlossarySaveButton.Click();

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог создания нового глоссария
		/// </summary>
		public bool IsNewGlossaryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_GLOSSARY_BUTTON), timeout: 30);
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'A glossary with this name already exists'
		/// </summary>
		public bool IsExistNameErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, что появилось сообщение 'A glossary with this name already exists'.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EXIST_NAME), timeout: 15);
		}

		/// <summary>
		/// Проверить, что язык присутствует в дропдауне.
		/// </summary>
		/// <param name="language">язык</param>
		public bool IsLanguageExistInDropdown(Language language)
		{
			CustomTestContext.WriteLine("Проверить, что язык {0} присутствует в дропдауне.", language);

			return Driver.GetIsElementExist(By.XPath(LANGUAGES_LIST.Replace("*#*", language.ToString())));
		}

		/// <summary>
		/// Проверить, появилось ли сообщение 'Specify glossary name'
		/// </summary>
		public bool IsSpecifyGlossaryNameErrorDisplayed()
		{
			CustomTestContext.WriteLine("Проверить, появилось ли сообщение 'Specify glossary name'");

			return Driver.WaitUntilElementIsDisplay(By.XPath(SPECIFY_GLOSSARY_NAME_ERROR));
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public bool IsClientExistInList(string clientName)
		{
			CustomTestContext.WriteLine("Проверить, что клиент {0} есть в списке при создании глоссария.", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST));

			return clientList.Any(client => client.Text.Contains(clientName));
		}

		/// <summary>
		/// Проверить, что группа проектов есть в списке при создании глоссария
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public bool IsProjectGroupExistInList(string projectGroupName)
		{
			CustomTestContext.WriteLine("Проверить, что группа проектов {0} присутствует в списке при создании глоссария.", projectGroupName);

			return Driver.GetElementList(By.XPath(MULTISELECT_LIST)).Any(e => e.Text == projectGroupName);
		}

		#endregion
		
		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = CLIENT_LIST)]
		protected IWebElement ClientsList { get; set; }

		[FindsBy(How = How.XPath, Using = DROPDOWN_LIST)]
		protected IWebElement ClientsListDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUPS_LIST)]
		protected IWebElement ProjectGroupsDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = MULTISELECT_LIST)]
		protected IWebElement ProjectGroupsList { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_COMMENT)]
		protected IWebElement GlossaryComment { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		[FindsBy(How = How.XPath, Using = GLOSSARY_SAVE_BUTTON)]
		protected IWebElement GlossarySaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_EMPTY_NAME)]
		protected IWebElement ErrorEmptyName { get; set; }

		[FindsBy(How = How.XPath, Using = DELETE_LANGUAGE_BUTTON)]
		protected IWebElement DeleteLanguageButton { get; set; }

		[FindsBy(How = How.XPath, Using = ERROR_EXIST_NAME)]
		protected IWebElement ErrorExistName { get; set; }

		protected IWebElement ProjectGroupsItem { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string LANGUAGES_LIST = "//body/span[contains(@class,'js-dropdown')]//span[@title='*#*']";
		protected const string GLOSSARY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[@class='l-editgloss__nmtext']";
		protected const string GLOSSARY_COMMENT = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__cont last']//textarea";
		protected const string LANGUAGE_DROPDOWN = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]/span[2]";
		protected const string LANGUAGE_DROPDOWN_SELECT = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]//select";
		protected const string ADD_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'addLanguage')]";
		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[contains(@data-bind, 'click: save')]//a";
		protected const string ERROR_EMPTY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@data-message-id,'glossary-name-required')]";
		protected const string DELETE_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][1]//span[*#*]//i[contains(@data-bind, 'deleteLanguage')]";
		protected const string LANGUAGES_DROPDOWNS = "//div[@class='l-editgloss__contrbox'][1]//span[@class='g-iblock l-editgloss__control l-editgloss__lang']";
		protected const string LANGUAGE_DROPDOWN_OPTION = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]";
		protected const string ERROR_EXIST_NAME = "//div[contains(@class,'edit-glossary')][2]//p[contains(@data-message-id,'glossary-exists')]";
		protected const string SPECIFY_GLOSSARY_NAME_ERROR = "//div[contains(@class, 'js-popup-edit-glossary')][2]//p[@data-message-id='glossary-name-required']";

		protected const string CLIENT_LIST= "//select[contains(@data-bind,'clientsList')]//following-sibling::span";
		protected const string DROPDOWN_LIST= "//body/span[contains(@class,'js-dropdown')]";
		protected const string PROJECT_GROUPS_LIST = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][3]//div";
		protected const string NEW_GLOSSARY_DIALOG = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string MULTISELECT_LIST = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";
		protected const string PROJECT_GROUPS_ITEM = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[text()='*#*']";

		#endregion
	}
}
