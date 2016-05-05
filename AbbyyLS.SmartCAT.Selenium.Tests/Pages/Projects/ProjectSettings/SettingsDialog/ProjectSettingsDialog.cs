using System;
using System.Globalization;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog
{
	public class ProjectSettingsDialog : ProjectSettingsPage, IAbstractPage<ProjectSettingsDialog>
	{
		public ProjectSettingsDialog(WebDriver driver) : base(driver)
		{
		}

		public new ProjectSettingsDialog LoadPage()
		{
			if (IsLanguagesSettingsDialogClosed() && !IsSettingsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог настроек проекта");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на вкладку 'Workflow'.
		/// </summary>
		public ProjectSettingsDialog ClickWorkflowTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку 'Workflow'.");
			WorkflowTab.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на вкладку General.
		/// </summary>
		public GeneralTab ClickGeneralTab()
		{
			CustomTestContext.WriteLine("Нажать на вкладку General.");
			GeneralTab.Click();

			return new GeneralTab(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на дропдаун целевого языка.
		/// </summary>
		public ProjectSettingsDialog ClickTargetLanguagesDropdown()
		{
			CustomTestContext.WriteLine("Нажать на дропдаун целевого языка.");
			TargetLanguagesDropdown.Click();

			return LoadPage();
		}
		
		/// <summary>
		/// Выбрать целевой язык.
		/// </summary>
		/// <param name="language">целевой язык</param>
		public ProjectSettingsDialog ClickTargetLanguagesOption(Language language)
		{
			CustomTestContext.WriteLine(" Выбрать целевой язык {0}.", language);
			TargetLanguageOption = Driver.SetDynamicValue(How.XPath, TARGET_LANGUAGE_OPTION, language.ToString());
			TargetLanguageOption.ScrollDown();
			TargetLanguageOption.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать дропдаун клиента.
		/// </summary>
		public ProjectSettingsDialog ClickClientDropdown()
		{
			CustomTestContext.WriteLine("Нажать дропдаун клиента.");
			ClientDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на клиента в дропдауне.
		/// </summary>
		/// <param name="client">название клиента</param>
		public ProjectSettingsDialog ClickClientOption(string client)
		{
			CustomTestContext.WriteLine("Нажать на клиента в дропдауне.");
			ClientOption = Driver.SetDynamicValue(How.XPath, CLIENT_OPTION, client);
			ClientOption.Scroll();
			ClientOption.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать дропдаун групы проекта.
		/// </summary>
		public ProjectSettingsDialog ClickProjectGroupDropdown()
		{
			CustomTestContext.WriteLine("Нажать дропдаун групы проекта.");
			ProjectGroupDropdown.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать на поле крайнего срока.
		/// </summary>
		public DatePicker ClickDatetimePickerIcon()
		{
			CustomTestContext.WriteLine("Нажать на поле крайнего срока.");
			DatetimePickerIcon.Click();

			return new DatePicker(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Quality Assurance Settings'.
		/// </summary>
		public QualityAssuranceSettings ClickQualityAssuranceSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Quality Assurance Settings'.");
			SetUpQASettingsButton.Click();

			return new QualityAssuranceSettings(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать на группу проекта в дропдауне.
		/// </summary>
		/// <param name="projectGroup">название группы проекта</param>
		public ProjectSettingsDialog ClickProjectGroupOption(string projectGroup)
		{
			CustomTestContext.WriteLine("Нажать на групу проекта в дропдауне.");
			ProjectGroupOption = Driver.SetDynamicValue(How.XPath, PROJECT_GROUP_OPTION, projectGroup);
			ProjectGroupOption.Scroll();
			ProjectGroupOption.Click();

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле описания проекта
		/// </summary>
		/// <param name="description">описание</param>
		public ProjectSettingsDialog FillDescription(string description)
		{
			CustomTestContext.WriteLine("Заполнить поле описания проекта '{0}'.", description);
			Description.SetText(description);

			return LoadPage();
		}

		/// <summary>
		/// Заполнить поле имени проекта
		/// </summary>
		/// <param name="name">имя</param>
		public ProjectSettingsDialog FillName(string name)
		{
			CustomTestContext.WriteLine("Вписать в поле имени проекта: '{0}'.", name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Получить название клиента
		/// </summary>
		public string GetClientName()
		{
			CustomTestContext.WriteLine("Получить название клиента.");

			return ClientValue.Text;
		}

		/// <summary>
		/// Получить название группы проекта
		/// </summary>
		public string GetProjectGroupName()
		{
			CustomTestContext.WriteLine("Получить название группы проекта.");

			return ProjectGroupValue.Text;
		}

		/// <summary>
		/// Получить описание проекта
		/// </summary>
		public string GetProjectDescription()
		{
			CustomTestContext.WriteLine("Получить описание проекта.");
			
			return Description.GetAttribute("value");
		}

		/// <summary>
		/// Получить дату в дедлайне.
		/// </summary>
		public DateTime? GetDeadine()
		{
			CustomTestContext.WriteLine("Получить дату в дедлайне.");
			var date = DeadlineValue.Text.Split(' ')[0];

			try
			{
				return DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
			}
			catch
			{
				throw new Exception("Произошла ошибка: не удалось конвертировать строку '" + date + "' в дату.");
			}
		}


		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать целевой язык.
		/// </summary>
		/// <param name="language">целевой язык</param>
		public ProjectSettingsDialog SelectTargetLanguages(Language language)
		{
			ClickTargetLanguagesDropdown();
			ClickTargetLanguagesOption(language);
			ClickTargetLanguagesDropdown();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать клиента.
		/// </summary>
		/// <param name="client">название клиента</param>
		public ProjectSettingsDialog SelectClient(string client)
		{
			ClickClientDropdown();
			ClickClientOption(client);
			ClickClientDropdown();

			return LoadPage();
		}

		/// <summary>
		/// Выбрать группу проекта.
		/// </summary>
		/// <param name="projectGroup">название группы проекта</param>
		public ProjectSettingsDialog SelectProjectGroup(string projectGroup)
		{
			ClickProjectGroupDropdown();
			ClickProjectGroupOption(projectGroup);
			ClickProjectGroupDropdown();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save и дождаться закрытия окна
		/// </summary>
		public ProjectSettingsPage SaveSettings()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save и дождаться закрытия окна");
			SaveButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку 'Set up' для настройки новых добавленных языков
		/// </summary>
		public NewLanguageSettingsDialog ClickSetUpLanguagesOptionsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Set up' для настройки новых добавленных языков.");
			SetUpLanguagesOptionsButton.Click();

			return new NewLanguageSettingsDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save и дождаться закрытия окна
		/// </summary>
		public ProjectsPage SaveSettingsExpectingProjectsPage()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save и дождаться закрытия окна");
			SaveButton.Click();

			return new ProjectsPage(Driver).LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel и дождаться закрытия окна
		/// </summary>
		public ProjectSettingsPage CancelSettingsChanges()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel и дождаться закрытия окна");
			CancelButton.Click();

			return new ProjectSettingsPage(Driver).LoadPage();
		}
		
		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог настроек проекта
		/// </summary>
		public bool IsSettingsDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SETTINGS_DIALOG));
		}

		/// <summary>
		/// Проверить, что закрылся диалог настроек языков
		/// </summary>
		public bool IsLanguagesSettingsDialogClosed()
		{
			return Driver.WaitUntilElementIsDisappeared(By.XPath(LANGUAGES_SETTINGS_DIALOG));
		}

		/// <summary>
		/// Проверить, что 'Workflow Setup' присутствует в настройках проекта
		/// </summary>
		public bool IsWorkflowSetupExistInSettings()
		{
			CustomTestContext.WriteLine("Проверить, что 'Workflow Setup' присутствует в настройках проекта");

			return Driver.WaitUntilElementIsDisplay(By.XPath(WORKFLOW_TAB));
		}

		/// <summary>
		/// Проверить, что диалог подтверждения удаления задачи появился.
		/// </summary>
		public bool IsConfirmDeleteDialogDislpayed()
		{
			CustomTestContext.WriteLine("Проверить, что диалог подтверждения удаления задачи появился");

			return Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_DELETE_DIALOG));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = WORKFLOW_TAB)]
		protected IWebElement WorkflowTab { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENTS_DROPDOWN)]
		protected IWebElement ClientDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_DROPDOWN)]
		protected IWebElement ProjectGroupDropdown { get; set; }

		protected IWebElement DeleteTaskButton { get; set; }

		[FindsBy(How = How.XPath, Using = GEBERAL_TAB)]
		protected IWebElement GeneralTab { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_LANGUAGES_DROPDOWN)]
		protected IWebElement TargetLanguagesDropdown { get; set; }

		[FindsBy(How = How.XPath, Using = DESCRIPTION)]
		protected IWebElement Description { get; set; }

		[FindsBy(How = How.XPath, Using = CLIENT_VALUE)]
		protected IWebElement ClientValue { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_VALUE)]
		protected IWebElement ProjectGroupValue { get; set; }

		[FindsBy(How = How.XPath, Using = DEADLINE_VALUE)]
		protected IWebElement DeadlineValue { get; set; }

		[FindsBy(How = How.XPath, Using = DATETIME_PICKER_ICON)]
		protected IWebElement DatetimePickerIcon { get; set; }

		[FindsBy(How = How.XPath, Using = SET_UP_QA_SETTINGS_BUTTON)]
		protected IWebElement SetUpQASettingsButton { get; set; }

		[FindsBy(How = How.XPath, Using = SET_UP_LANGUAGES_ADDITIONAL_OPTIONS)]
		protected IWebElement SetUpLanguagesOptionsButton { get; set; }

		[FindsBy(How = How.XPath, Using = NAME)]
		protected IWebElement Name { get; set; }


		protected IWebElement TargetLanguageOption { get; set; }

		protected IWebElement ClientOption { get; set; }
		protected IWebElement ProjectGroupOption { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CANCEL_BUTTON = "//div[contains(@class,'js-popup-edit')][2]//a[contains(@class,'js-popup-close')]";
		protected const string SETTINGS_DIALOG = "(//div[contains(@class,'js-popup-edit')])[2]";
		protected const string WORKFLOW_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'workflowTab')]";
		protected const string DELETE_TASK_BUTTON = "(//div[contains(@class,'js-popup-edit')][2]//a[contains(@data-bind,'deleteWorkflowStage')])[*#*]";
		protected const string CONFIRM_DELETE_DIALOG = "//div[contains(@class,'js-popup-confirm')]";
		protected const string SAVE_BUTTON = "//div[contains(@class, 'js-popup-edit') and contains(@style, 'display: block')]//div[@class='g-popupbox__ft']//div//a";
		protected const string WORKFLOW_LIST = "//div[contains(@class,'js-popup-edit')][2]//tbody[@data-bind='foreach: workflowStages']//tr//td[2]//span//span";
		protected const string GEBERAL_TAB = "(//div[contains(@class,'js-popup-edit')])[2]//a[contains(@data-bind,'general')]";
		protected const string TARGET_LANGUAGES_DROPDOWN = "//select[contains(@data-bind, 'targetLanguageIds')]/following-sibling::div//i";
		protected const string TARGET_LANGUAGE_OPTION = "//span[text()='*#*']";

		protected const string CLIENTS_DROPDOWN = "//select[contains(@data-bind, 'clientsList')]/following-sibling::span//i";
		protected const string CLIENT_OPTION = "//span[@title='*#*']";
		protected const string CLIENT_VALUE = "//select[contains(@data-bind, 'clientsList')]/following-sibling::span/span";

		protected const string PROJECT_GROUP_DROPDOWN = "//select[contains(@data-bind, 'domainsList')]/following-sibling::span//i";
		protected const string PROJECT_GROUP_OPTION = "//span[@title='*#*']";
		protected const string PROJECT_GROUP_VALUE = "//select[contains(@data-bind, 'domainsList')]/following-sibling::span/span";

		protected const string DESCRIPTION = "(//textarea[contains(@data-bind, 'description')])[2]";
		protected const string DATETIME_PICKER_ICON = "//span[@class= 'g-datetimepicker__icon']";
		protected const string DEADLINE_VALUE = "//div[contains(@class,'js-popup-edit')]//span[contains(@data-bind, 'formatDateTime')]";

		protected const string SET_UP_QA_SETTINGS_BUTTON = "(//div[contains(@data-bind, 'setupQaSettings')])[2]";
		protected const string SET_UP_LANGUAGES_ADDITIONAL_OPTIONS = "//div[contains(@style, 'display: block')]//a[contains(@data-bind, 'showLanguagesPopup')]";
		protected const string LANGUAGES_SETTINGS_DIALOG = "//div[contains(@class, 'js-popup-languages') and contains(@style, 'display: block')]";

		protected const string NAME = "(//div[contains(@class,'js-popup-edit')])[2]//input[@data-bind = 'value: name']";

		#endregion
	}
}