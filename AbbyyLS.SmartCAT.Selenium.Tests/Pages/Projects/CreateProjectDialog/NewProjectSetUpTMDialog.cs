using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpTMDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpTMDialog>
	{
		public new NewProjectSetUpTMDialog GetPage()
		{
			var newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();
			InitPage(newProjectSetUpTMDialog);

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
		/// Подтвердить, существует ли ТМ при созданиие проекта
		/// </summary>
		public NewProjectSetUpTMDialog AssertTranslationMemoryExist(string translationMemoryName)
		{
			TranslationMemoryItem = Driver.SetDynamicValue(How.XPath, TM_ITEM, translationMemoryName);

			Assert.IsTrue(TranslationMemoryItem.Enabled,
				"Произошла ошибка:\n ТМ {0} не существует при создании проекта.", translationMemoryName);

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Создать ТМ"
		/// </summary>
		public NewProjectCreateTMDialog ClickCreateTMButton()
		{
			Logger.Debug("Нажать кнопку 'Создать ТМ'.");
			CreateTMButton.Click();

			return new NewProjectCreateTMDialog().GetPage();
		}

		/// <summary>
		/// Подтвердить, что кнопка "Готова" доступна
		/// </summary>
		public NewProjectSetUpTMDialog AssertFinishButtonEnabled()
		{
			Logger.Debug("Подтвердить, что кнопка 'Готово' доступна");

			Assert.IsTrue(Driver.WaitUntilElementIsEnabled(By.XPath(CREATE_PROJECT_FINISH_BUTTON)) && Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_PROJECT_FINISH_BUTTON)),
				"Ошибка: \n кнопка 'Готово' недоступна.");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectsPage ClickFinishButton()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.HoverElement();
			CreateProjectFinishButton.JavaScriptClick();

			return new ProjectsPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_BTN)]
		protected IWebElement CreateTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_TABLE_FIRST_ITEM)]
		protected IWebElement TMTableFirstItem { get; set; }

		protected IWebElement TranslationMemoryItem { get; set; }

		protected const string CREATE_TM_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
		protected const string TM_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr//td[contains(@class,'js-name')][text()='*#*']";
	}
}
