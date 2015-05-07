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
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_TM_BTN), 10))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к следующему шагу создания проекта (выбор ТМ).");
			}
		}

		/// <summary>
		/// Проверить, пустая ли таблица с ТМ
		/// </summary>
		public bool IsTMTableEmpty()
		{
			Logger.Trace("Проверить пустая ли таблица с ТМ.");

			return Driver.WaitUntilElementIsEnabled(By.XPath(TM_TABLE_FIRST_ITEM));
		}

		/// <summary>
		/// Проверить, удалось ли добавить в проект ТМ (если ТМ не было вообще)
		/// </summary>
		public NewProjectSetUpTMDialog AssertIsTMTableNotEmpty()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_TABLE_FIRST_ITEM), 15),
				"Произошла ошибка:\n не удалось добавить ТМ при создании проекта.");

			return GetPage();
		}

		/// <summary>
		/// Выбрать первую ТМ из таблицы
		/// </summary>
		public NewProjectSetUpTMDialog ClickTMTableFirstItem()
		{
			Logger.Debug("Выбрать первую ТМ из таблицы.");
			TMTableFirstItem.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Создать ТМ"
		/// </summary>
		public NewProjectCreateTMDialog ClickCreateTMButton()
		{
			Logger.Debug("Нажать кнопку 'Создать ТМ'.");
			CreateTMButton.Click();
			var createTMDialog = new NewProjectCreateTMDialog();

			return createTMDialog.GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSelectGlossariesDialog ClickNextButton()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextButton.Click();
			var newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog();

			return newProjectSelectGlossariesDialog.GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectsPage ClickFinishButton()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			CreateProjectFinishButton.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_BTN)]
		protected IWebElement CreateTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = TM_TABLE_FIRST_ITEM)]
		protected IWebElement TMTableFirstItem { get; set; }

		protected const string CREATE_TM_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
	}
}
