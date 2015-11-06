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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_TM_BTN)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к следующему шагу создания проекта (выбор ТМ).");
			}
		}
		/// <summary>
		/// Проверить, что ТМ представлена в списке при создании проекта
		/// </summary>
		public NewProjectSetUpTMDialog AssertTranslationMemoryExist(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} представлена в списке при создании проекта.", translationMemoryName);
			TranslationMemoryItem = Driver.SetDynamicValue(How.XPath, TM_ITEM, translationMemoryName);

			Assert.IsTrue(TranslationMemoryItem.Enabled,
				"Произошла ошибка:\n ТМ {0} не представлена в списке при создании проекта.", translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что диалог создания TM закрылся
		/// </summary>
		public NewProjectSetUpTMDialog AssertNewProjectCreateTMDialogDisappeared()
		{
			CustomTestContext.WriteLine("Проверить, что диалог создания TM закрылся");

			Assert.IsTrue(Driver.WaitUntilElementIsDisappeared(By.XPath(NEW_TM_NAME_INPUT)),
				"Произошла ошибка:\n диалог создания TM не закрылся");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что первая ТМ выбрана
		/// </summary>
		public NewProjectSetUpTMDialog AssertFirstTMSelected()
		{
			CustomTestContext.WriteLine("Проверить, что первая ТМ выбрана.");

			Assert.IsTrue(TMTableFirstItem.Selected,
				"Произошла ошибка:\n первая ТМ не выбрана.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что ТМ отсутствует при создании проекта
		/// </summary>
		public NewProjectSetUpTMDialog AssertTranslationMemoryNotExist(string translationMemoryName)
		{
			CustomTestContext.WriteLine("Проверить, что ТМ {0} отсутствует при создании проекта.", translationMemoryName);

			Assert.IsFalse(Driver.GetIsElementExist(By.XPath(TM_ITEM.Replace("*#*", translationMemoryName))),
				"Произошла ошибка:\n ТМ {0} присутствует при создании проекта.", translationMemoryName);

			return GetPage();
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
		public NewProjectSetUpTMDialog ClickFirstTMRow()
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
		/// Вернуть, что хотя бы одна ТМ существует в таблице
		/// </summary>
		public bool FirstTMRowExist()
		{
			CustomTestContext.WriteLine("Вернуть, что хотя бы одна ТМ существует в таблице.");

			return Driver.GetIsElementExist(By.XPath(TM_TABLE_FIRST_ITEM));
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_BTN)]
		protected IWebElement CreateTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_TABLE_FIRST_ITEM)]
		protected IWebElement TMTableFirstItem { get; set; }
		
		[FindsBy(How = How.XPath, Using = UPLOAD_TM_BUTTON)]
		protected IWebElement UploadTMButton { get; set; }

		protected IWebElement TranslationMemoryItem { get; set; }

		protected const string CREATE_TM_BTN = "//div[contains(@class,'js-popup-create-project')][2]//div[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
		protected const string TM_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr//td[contains(@class,'js-name')][text()='*#*']";
		protected const string UPLOAD_TM_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-upload')]";
		protected const string NEW_TM_NAME_INPUT = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'value: name')]";
	}
}
