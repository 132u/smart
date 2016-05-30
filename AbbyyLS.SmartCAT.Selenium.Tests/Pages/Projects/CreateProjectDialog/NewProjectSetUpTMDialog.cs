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

		public new NewProjectSetUpTMDialog LoadPage()
		{
			if (!IsNewProjectSetUpTMDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не удалось перейти к следующему шагу создания проекта (выбор ТМ)");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Выбрать первую ТМ в списке
		/// </summary>
		public NewProjectSetUpTMDialog SelectFirstTM()
		{
			CustomTestContext.WriteLine("Выбрать первую ТМ в списке.");
			TMTableFirstItem.Click();
			
			return LoadPage();
		}

		/// <summary>
		/// Выбрать память перевода.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSetUpTMDialog ClickTranslationMemoryCheckbox(string translationMemory)
		{
			CustomTestContext.WriteLine("Выбрать память перевода {0}.", translationMemory);
			TranslationMemoryCheckbox = Driver.SetDynamicValue(How.XPath, TRANSLATION_MEMORY_CHECKBOX, translationMemory);
			TranslationMemoryCheckbox.ScrollAndClick();

			return LoadPage();
		}

		/// <summary>
		/// Ввести название памяти перевода в поиск.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSetUpTMDialog SearchTranslationMemory(string translationMemory)
		{
			CustomTestContext.WriteLine("Ввести название памяти перевода {0} в поиск.", translationMemory);
			Driver.WaitUntilElementIsClickable(By.XPath(SEARCH));
			Search.SetText(translationMemory);

			return LoadPage();
		}
		
		/// <summary>
		/// Нажать кнопку Cancel в окне выбора TM
		/// </summary>
		public TranslationMemoryAdvancedSettingsSection ClickCancelButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Cancel в окне выбора TM");
			CancelButton.Click();

			return new TranslationMemoryAdvancedSettingsSection(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Нажать кнопку Add
		/// </summary>
		public NewProjectSettingsPage ClickAddButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Add.");
			AddButton.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		/// <summary>
		/// Выбрать память перевода.
		/// </summary>
		/// <param name="translationMemory">память перевода</param>
		public NewProjectSettingsPage SelectExisitingTranslationMemory(string translationMemory)
		{
			CustomTestContext.WriteLine("Выбрать память перевода {0}.", translationMemory);
			SearchTranslationMemory(translationMemory);
			ClickTranslationMemoryCheckbox(translationMemory);
			ClickAddButton();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

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
		/// Проверить, что первая ТМ выбрана
		/// </summary>
		public bool IsFirstTMSelected()
		{
			CustomTestContext.WriteLine("Проверить, что первая ТМ выбрана");

			return TMTableFirstItem.Selected;
		}

		#endregion

		#region Объявление элементов страницы

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

		[FindsBy(How = How.XPath, Using = SEARCH)]
		protected IWebElement Search { get; set; }
		
		protected IWebElement TranslationMemoryCheckbox { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string CREATE_TM_BTN = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
		protected const string TM_ITEM = "//div[@data-bind='foreach: filteredTranslationMemories']//div[@data-bind='text: name' and text()='*#*']";
		protected const string UPLOAD_TM_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-upload')]";
		protected const string NEW_TM_NAME_INPUT = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'value: name')]";
		protected const string ADD_BUTTON = "//a[contains(@data-bind, 'addTranslationMemories')]";
		protected const string CANCEL_BUTTON = "(//h2[text()='Translation Memory']//ancestor::div[@class='g-popupbox js-popupbox']//a[@data-bind='click: close'])[1]";

		protected const string TRANSLATION_MEMORY_CHECKBOX = "//div[contains(text(),'*#*')]/../../../..//label//input";
		protected const string SEARCH = "(//input[@name='searchName'])[1]";

		#endregion
	}
}
