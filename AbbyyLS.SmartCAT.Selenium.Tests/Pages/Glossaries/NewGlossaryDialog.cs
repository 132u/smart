using System.Linq;

using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class NewGlossaryDialog : GlossariesPage, IAbstractPage<NewGlossaryDialog>
	{
		public new NewGlossaryDialog GetPage()
		{
			var glossaryCreationPage = new NewGlossaryDialog();
			InitPage(glossaryCreationPage);

			return glossaryCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_GLOSSARY_BUTTON), timeout: 30))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог создания глоссария.");
			}
		}

		/// <summary>
		/// Открыть список клиентов при создании глоссария
		/// </summary>
		public NewGlossaryDialog OpenClientsList()
		{
			Logger.Debug("Открыть список клиентов при создании глоссария");
			ClientsList.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список клиентов открылся
		/// </summary>
		public NewGlossaryDialog AssertClientsListOpened()
		{
			Logger.Trace("Проверить, что список клиентов открылся.");
			
			Assert.IsTrue(ClientsListDropDown.Displayed,
				"Произошла ошибка:\n список клиентов не открылся.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewGlossaryDialog AssertClientExistInList(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} есть в списке при создании глоссария.", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST));

			Assert.IsTrue(clientList.First().Text.Contains(clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов при создании глоссария.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewGlossaryDialog AssertClientNotExistInList(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} есть в списке при создании глоссария.", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST));

			Assert.IsFalse(clientList.Any(e => e.GetAttribute("innerHTML") == clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов при создании глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Открыть список групп проектов
		/// </summary>
		public NewGlossaryDialog ClickProjectGroupsList()
		{
			Logger.Debug("Открыть список групп проектов при создании глоссария.");
			ProjectGroupsDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать группу проектов списке
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewGlossaryDialog SelectProjectGroup(string projectGroupName)
		{
			Logger.Debug("Выбрать группу проектов списке");

			ProjectGroupsItem = Driver.SetDynamicValue(How.XPath, PROJECT_GROUPS_ITEM, projectGroupName);
			ProjectGroupsItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список групп проектов открылся
		/// </summary>
		public NewGlossaryDialog AssertProjectGroupsListOpened()
		{
			Logger.Trace("Проверить, что список групп проектов открылся при создании глоссария.");

			Assert.IsTrue(ProjectGroupsList.Displayed,
				"Произошла ошибка:\n список групп проектов не открылся при создании глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов есть в списке при создании глоссария
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewGlossaryDialog AssertProjectGroupExistInList(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} присутствует в списке при создании глоссария.", projectGroupName);
			var projectGroupsList = Driver.GetElementList(By.XPath(MULTISELECT_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsTrue(projectGroupExist, "Произошла ошибка:\n  группа проектов {0} отсутствует в списке при создании глоссария.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов отсутствует в списке при создании глоссария
		/// </summary>
		public NewGlossaryDialog AssertProjectGroupNotExistInList(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} отсутствует в списке при создании глоссария.", projectGroupName);
			var projectGroupsList = Driver.GetElementList(By.XPath(MULTISELECT_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsFalse(projectGroupExist, "Произошла ошибка:\n  группа проектов {0} присутствует в списке при создании глоссария.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Ввести название глоссария
		/// </summary>
		public NewGlossaryDialog FillGlossaryName(string glossaryName)
		{
			Logger.Debug("Ввести {0} название глоссария", glossaryName);
			GlossaryName.SetText(glossaryName);

			return GetPage();
		}

		/// <summary>
		/// Ввести комментарий
		/// </summary>
		public NewGlossaryDialog FillComment(string comment)
		{
			Logger.Debug("Ввести комментарий {0}.", comment);
			GlossaryComment.SetText(comment);

			return GetPage();
		}

		/// <summary>
		/// Раскрыть дропдаун выбора языка
		/// </summary>
		/// <param name="dropdownNumber">номер дропдауна</param>
		public NewGlossaryDialog ExpandLanguageDropdown(int dropdownNumber)
		{
			Logger.Debug("Раскрыть дропдаун выбора для {0} языка.", dropdownNumber);
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
			Logger.Debug("Выбрать язык {0} в дропдауне.", language);
			var languageOption = Driver.SetDynamicValue(How.XPath, LANGUAGES_LIST, language.ToString());

			languageOption.ScrollAndClick();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add
		/// </summary>
		public NewGlossaryDialog ClickAddButton()
		{
			Logger.Debug("Нажать кнопку Add.");
			AddButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения глоссария
		/// </summary>
		public T ClickSaveGlossaryButton<T>() where T : class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку сохранения глоссария.");
			GlossarySaveButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение о пустом имени глоссария
		/// </summary>
		public NewGlossaryDialog AssertEmptyNamyErrorDisplay()
		{
			Logger.Trace("Проверить, что появилось сообщение о пустом имени глоссария.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EMPTY_NAME)),
				"Произошла ошибка:\n не появилось сообщение о пустом имени глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что появилось сообщение 'A glossary with this name already exists'
		/// </summary>
		public NewGlossaryDialog AssertExistNameErrorDisplay()
		{
			Logger.Trace("Проверить, что появилось сообщение 'A glossary with this name already exists'.");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(ERROR_EXIST_NAME), timeout: 15),
				"Произошла ошибка:\n сообщение 'A glossary with this name already exists' не появилось .");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку удаления языка
		/// </summary>
		public NewGlossaryDialog ClickDeleteLanguageButton(int dropdown = 1)
		{
			Logger.Debug("Нажать кнопку удаления языка.");
			var deleteButton = Driver.SetDynamicValue(How.XPath, DELETE_LANGUAGE_BUTTON, dropdown.ToString());

			deleteButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить количество языков
		/// </summary>
		public int GetGlossaryLanguageCount()
		{
			Logger.Trace("Получить количество языков в глоссарии.");

			return Driver.GetElementsCount(By.XPath(LANGUAGES_DROPDOWNS)) - 1;
		}

		/// <summary>
		/// Проверить, что язык отсутствует в дропдауне.
		/// </summary>
		public void AssertLanguageNotExistInDropdown(Language language)
		{
			Logger.Trace("Проверить, что язык {0} отсутствует в дропдауне.", language);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(LANGUAGES_LIST.Replace("*#*", language.ToString()))),
				"Произошла ошибка:\n языка {0} присутствует в дропдауне.", language);
		}
		
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

		protected const string LANGUAGES_LIST = "//body/span[contains(@class,'js-dropdown')]//span[@title='*#*']";
		protected const string GLOSSARY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//input[@class='g-bold l-editgloss__nmtext']";
		protected const string GLOSSARY_COMMENT = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__cont last']//textarea";
		protected const string LANGUAGE_DROPDOWN = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]/span[2]";
		protected const string LANGUAGE_DROPDOWN_SELECT = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]//select";
		protected const string ADD_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-bluebtn addlang enabled']";
		protected const string GLOSSARY_SAVE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@class='g-btn g-redbtn ']";
		protected const string ERROR_EMPTY_NAME = ".//div[contains(@class,'js-popup-edit-glossary')][2]//p[contains(@data-message-id,'glossary-name-required')]";
		protected const string DELETE_LANGUAGE_BUTTON = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][1]//span[*#*]//em";
		protected const string LANGUAGES_DROPDOWNS = "//div[@class='l-editgloss__contrbox'][1]//span[@class='g-iblock l-editgloss__control l-editgloss__lang']";
		protected const string LANGUAGE_DROPDOWN_OPTION = "//div[contains(@class,'edit-glossary')][2]//span[*#*][contains(@class, 'l-editgloss__lang')]";
		protected const string ERROR_EXIST_NAME = "//div[contains(@class,'edit-glossary')][2]//p[contains(@data-message-id,'glossary-exists')]";

		protected const string CLIENT_LIST= "//select[contains(@data-bind,'clientsList')]//following-sibling::span";
		protected const string DROPDOWN_LIST= "//body/span[contains(@class,'js-dropdown')]";
		protected const string PROJECT_GROUPS_LIST = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][3]//div";
		protected const string NEW_GLOSSARY_DIALOG = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string MULTISELECT_LIST = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";
		protected const string PROJECT_GROUPS_ITEM = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[text()='*#*']";
	}
}
