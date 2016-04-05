using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.AssignmentPages;

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

		public new GeneralTab LoadPage()
		{
			if (!IsGeneralTabOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n Не открылась вкладка General.");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку удаления даты дедлайна.
		/// </summary>
		public GeneralTab ClickRemoveDateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку удаления даты дедлайна.");
			RemoveDeadlineButton.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Открыть календарь.
		/// </summary>
		public GeneralTab OpenCalendar()
		{
			CustomTestContext.WriteLine("Открыть календарь.");
			ShowCalendarIcon.Click();

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку контроля качества.
		/// </summary>
		public QualityAssuranceSettings ClickQualityAssuranceSettingsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку контроля качества.");
			SetUpQASettingsButton.Click();

			return new QualityAssuranceSettings(Driver).LoadPage();
		}

		/// <summary>
		/// Поменять имя проекта.
		/// </summary>
		public GeneralTab EditName(string name)
		{
			CustomTestContext.WriteLine("Поменять имя проекта на {0}.", name);
			Name.SetText(name);

			return LoadPage();
		}

		/// <summary>
		/// Поменять крайний срок.
		/// </summary>
		public GeneralTab EditDeadlineManually(string deadline)
		{
			CustomTestContext.WriteLine("Поменять крайний срок {0}.", deadline);
			Deadline.SetText(deadline);

			return LoadPage();
		}

		/// <summary>
		/// Поменять описание проекта.
		/// </summary>
		public GeneralTab EditDescription(string description)
		{
			CustomTestContext.WriteLine("Поменять описание проекта на {0}.", description);
			Description.SetText(description);

			return LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылась вкладка General.
		/// </summary>
		public bool IsGeneralTabOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(SET_UP_QA_SETTINGS_BUTTON));
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

		[FindsBy(How = How.XPath, Using = REMOVE_DEADLINE_BUTTON)]
		protected IWebElement RemoveDeadlineButton { get; set; }

		[FindsBy(How = How.XPath, Using = DATEPICKER)]
		protected IWebElement DatePicker { get; set; }

		[FindsBy(How = How.XPath, Using = SHOW_CALENDAR_ICON)]
		protected IWebElement ShowCalendarIcon { get; set; }

		#endregion
		
		#region Описания XPath элементов

		protected const string SHOW_CALENDAR_ICON = "//datetimepicker//span[contains(@data-bind, 'showCalendarIcon')]";
		protected const string DATEPICKER = "//datetimepicker//input";
		protected const string REMOVE_DEADLINE_BUTTON = "//datetimepicker//span[contains(@class, 'icon-close')]";
		protected const string DEADLINE = "//input[contains(@class, 'hasDatepicker')]";
		protected const string SET_UP_QA_SETTINGS_BUTTON = "//div[contains(@class,'popup-edit')][2]//div[contains(@data-bind, 'setupQaSettings')]";
		protected const string NAME = "(//div[contains(@class,'js-popup-edit')])[2]//input[contains(@data-bind, 'value: name')]";
		protected const string DESCRIPTION = "(//div[contains(@class,'js-popup-edit')])[2]//textarea[contains(@data-bind, 'value: description')]";

		#endregion
	}
}
