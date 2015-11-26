using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpTMDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpTMDialog>
	{
		public NewProjectSetUpTMDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSetUpTMDialog GetPage()
		{
			var newProjectSetUpTMDialog = new NewProjectSetUpTMDialog(Driver);
			InitPage(newProjectSetUpTMDialog, Driver);

			return newProjectSetUpTMDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewProjectSetUpTMDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не удалось перейти к следующему шагу создания проекта (выбор ТМ)");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Создать ТМ"
		/// </summary>
		public NewProjectCreateTMDialog ClickCreateTMButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Создать ТМ'.");
			CreateTMButton.Click();

			return new NewProjectCreateTMDialog(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать первую ТМ в списке
		/// </summary>
		public NewProjectSetUpTMDialog SelectFirstTM()
		{
			CustomTestContext.WriteLine("Выбрать первую ТМ в списке.");
			TMTableFirstItem.Click();
			
			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Upload TM'
		/// </summary>
		public NewProjectCreateTMDialog ClickUploadTMButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Upload TM'.");
			UploadTMButton.Click();

			return new NewProjectCreateTMDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add в окне выбора TM
		/// </summary>
		public NewProjectSettingsPage ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add в окне выбора TM");
			AddButton.Click();

			return new NewProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку Cancel в окне выбора TM
		/// </summary>
		public NewProjectSettingsPage ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel в окне выбора TM");
			CancelButton.Click();
			WaitUntilDialogBackgroundDisappeared();

			return new NewProjectSettingsPage(Driver).GetPage();
		}

		/// <summary>
		/// Проверить, открылся ли шаг выбора TM в диалоге создания проекта
		/// </summary>
		public bool IsNewProjectSetUpTMDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(ADD_BUTTON));
		}

		/// <summary>
		/// Проверить, что ТМ представлена в списке при создании проекта
		/// </summary>
		public bool IsTranslationMemoryExist(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} представлена в списке при создании проекта.", translationMemoryName);

			return Driver.GetIsElementExist(By.XPath(TM_ITEM.Replace("*#*", translationMemoryName)));
		}

		/// <summary>
		/// Проверить, что диалог создания TM закрылся
		/// </summary>
		public bool IsNewProjectCreateTMDialogDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог создания TM закрылся");

			return Driver.WaitUntilElementIsDisappeared(By.XPath(NEW_TM_NAME_INPUT));
		}

		/// <summary>
		/// Проверить, что первая ТМ выбрана
		/// </summary>
		public bool IsFirstTMSelected()
		{
			CustomTestContext.WriteLine("Проверить, что первая ТМ выбрана");

			return TMTableFirstItem.Selected;
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_BTN)]
		protected IWebElement CreateTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_TABLE_FIRST_ITEM)]
		protected IWebElement TMTableFirstItem { get; set; }
		
		[FindsBy(How = How.XPath, Using = UPLOAD_TM_BUTTON)]
		protected IWebElement UploadTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		[FindsBy(How = How.XPath, Using = CANCEL_BUTTON)]
		protected IWebElement CancelButton { get; set; }

		protected IWebElement TranslationMemoryItem { get; set; }

		protected const string CREATE_TM_BTN = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
		protected const string TM_ITEM = "//div[@data-bind='foreach: filteredTranslationMemories']//div[@data-bind='text: name' and text()='*#*']";
		protected const string UPLOAD_TM_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-upload')]";
		protected const string NEW_TM_NAME_INPUT = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'value: name')]";
		protected const string ADD_BUTTON = "//a[@data-bind='click: addTranslationMemories']";
		protected const string CANCEL_BUTTON = "//a[@data-bind='click: close']";
	}
}
