using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights
{
	public class AddAccessRightDialog : UsersAndRightsBasePage, IAbstractPage<AddAccessRightDialog>
	{
		public AddAccessRightDialog(WebDriver driver) : base(driver)
		{
		}

		public new AddAccessRightDialog GetPage()
		{
			var addAccessRightPage = new AddAccessRightDialog(Driver);
			InitPage(addAccessRightPage, Driver);

			return addAccessRightPage;
		}

		public new void LoadPage()
		{
			if (!IsAddRightDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось открыть диалог добавления права.");
			}
		}

		#region Простые методы страницы

		/// <summary>
		/// Выбрать радиокнопку 'Для конкретного клиента'.
		/// </summary>
		public AddAccessRightDialog ClickForSpecificClientRadiobutton()
		{
			CustomTestContext.WriteLine("Выбрать радиокнопку 'Для конкретного клиента'.");
			ForSpecificClientRadioButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать из списка право на создание проектов
		/// </summary>
		/// <param name="right">право</param>
		public AddAccessRightDialog ClickRightRadio(RightsType right)
		{
			CustomTestContext.WriteLine("Выбрать из списка право {0}.", right);

			RightRadio = Driver.SetDynamicValue(How.XPath, RIGHT_RADIO, right.ToString());
			RightRadio.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать радиокнопку 'Для всех проектов'.
		/// </summary>
		public AddAccessRightDialog ClickForAnyProjectRadio()
		{
			CustomTestContext.WriteLine("Выбрать радиокнопку 'Для всех проектов'.");
			ForAnyProjectRadio.Click();

			return GetPage();
		}

		/// <summary>
		/// Выбрать радиокнопку 'Для конкретного проекта'.
		/// </summary>
		public AddAccessRightDialog ClickForSpecificProjectRadio()
		{
			CustomTestContext.WriteLine("Выбрать радиокнопку 'Для конкретного проекта'.");
			ForSpecificProjectRadio.Click();

			return GetPage();
		}
		
		/// <summary>
		/// Нажать кнопку далее (при добавлении прав пользователя)
		/// </summary>
		public AddAccessRightDialog ClickNextButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку далее (при добавлении прав пользователя).");
			NextButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Добавить" (право)
		/// </summary>
		public GroupsAndAccessRightsTab ClickAddRightButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Добавить' (право).");
			AddRightButton.Click();

			return new GroupsAndAccessRightsTab(Driver).GetPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Добавить права
		/// </summary>
		/// <param name="right">тип права</param>
		public GroupsAndAccessRightsTab AddRightToGroupAnyProject(RightsType right)
		{
			ClickRightRadio(right);
			ClickNextButton();
			ClickForAnyProjectRadio();

			var groupsAndAccessRightsTab = ClickAddRightButton();

			return groupsAndAccessRightsTab;
		}

		/// <summary>
		/// Добавить права
		/// </summary>
		/// <param name="right">тип права</param>
		/// <param name="projectName">название проекта</param>

		public GroupsAndAccessRightsTab AddRightToGroupSpecificProject(RightsType right, string projectName)
		{
			ClickRightRadio(right);
			ClickNextButton();
			ClickForSpecificProjectRadio();
			ClickNextButton();
			SelectProject(projectName);

			var groupsAndAccessRightsTab = ClickAddRightButton();

			return groupsAndAccessRightsTab;
		}
		
		/// <summary>
		/// Добавить права
		/// </summary>
		/// <param name="right">тип права</param>
		/// <param name="client">клиент</param>
		public GroupsAndAccessRightsTab AddRightToGroupSpecificClient(RightsType right, string client)
		{
			ClickRightRadio(right);
			ClickNextButton();
			ClickForSpecificClientRadiobutton();
			ClickNextButton();
			SelectProject(client);

			var groupsAndAccessRightsTab = ClickAddRightButton();

			return groupsAndAccessRightsTab;
		}

		/// <summary>
		/// Выбрать проект
		/// </summary>
		/// <param name="projectName">название проекта</param>
		public AddAccessRightDialog SelectProject(string projectName)
		{
			CustomTestContext.WriteLine("Выбрать проект {0}.", projectName);
			ProjectsDropdown.Click();
			ProjectOption = Driver.SetDynamicValue(How.XPath, PROJECT_OPTION, projectName);
			ProjectOption.Click();

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог добавления прав
		/// </summary>
		public bool IsAddRightDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_ACCESS_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = ADD_RIGHT_BTN_XPATH)]
		protected IWebElement AddRightButton { get; set; }

		[FindsBy(How = How.XPath, Using = FOR_ANY_PROJECT_RADIO_XPATH)]
		protected IWebElement ForAnyProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = FOR_SPECIFIC_PROJECT_RADIO_XPATH)]
		protected IWebElement ForSpecificProjectRadio { get; set; }

		[FindsBy(How = How.XPath, Using = FOR_SPECIFIC_CLIENT_RADIOBUTTON)]
		protected IWebElement ForSpecificClientRadioButton { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextButton { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECTS_DROPDOWN)]
		protected IWebElement ProjectsDropdown { get; set; }
		
		protected IWebElement RightRadio { get; set; }
		protected IWebElement ProjectOption { get; set; }
		
		#endregion

		#region Описание XPath элементов

		protected const string ADD_ACCESS_DIALOG = "//div[contains(@class,'js-add-access-right-popup')][2]";
		protected const string ADD_RIGHT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'visible : canFinishWizard, click : finishWizard')]//a[string() = 'Add']";
		protected const string NEXT_BTN_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'click : moveToNextStep')]//a[string() = 'Next']";
		protected const string FOR_ANY_PROJECT_RADIO_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'hasUnrestrictedAccessScope')]";
		protected const string FOR_SPECIFIC_PROJECT_RADIO_XPATH = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'specificObjectsList')]";
		protected const string FOR_SPECIFIC_CLIENT_RADIOBUTTON = "//div[contains(@class, 'add-access-right-popup')][2]//div[contains(@data-bind, 'clientsList')]";
		protected const string RIGHT_RADIO = "//input[@id='*#*']/../../label[@class='g-radiobtn']";
		protected const string PROJECTS_DROPDOWN = "//div[contains(@class, 'add-access-right-popup')][2]//span[contains(@class, 'js-dropdown')]";
		protected const string PROJECT_OPTION = "//span[@title='*#*']";

		#endregion
	}
}
