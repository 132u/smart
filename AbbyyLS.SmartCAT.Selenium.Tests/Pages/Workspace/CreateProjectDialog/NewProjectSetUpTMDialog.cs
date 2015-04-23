using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace.CreateProjectDialog
{
	public class NewProjectSetUpTMDialog : ProjectsPage, IAbstractPage<NewProjectSetUpTMDialog>
	{
		public new NewProjectSetUpTMDialog GetPage()
		{
			var newProjectSetUpTMDialog = new NewProjectSetUpTMDialog();
			InitPage(newProjectSetUpTMDialog);
			LoadPage();

			return newProjectSetUpTMDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_TM_BTN_XPATH), 10))
			{
				Assert.Fail("Произошла ошибка:\n не удалось перейти к следующему шагу создания проекта (выбор ТМ).");
			}
		}

		/// <summary>
		/// Проверить, пустая ли таблица с ТМ
		/// </summary>
		public bool IsTMTableNotEmpty()
		{
			Logger.Trace("Проверить пустая ли таблица с ТМ.");

			return Driver.WaitUntilElementIsEnabled(By.XPath(TM_TABLE_FIRST_ITEM_XPATH));
		}

		/// <summary>
		/// Проверить, удалось ли добавить в проект ТМ (если ТМ не было вообще)
		/// </summary>
		public NewProjectSetUpTMDialog AssertIsTMTableNotEmpty()
		{
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(TM_TABLE_FIRST_ITEM_XPATH), 15),
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
		public CreateTMDialog ClickCreateTMBtn()
		{
			Logger.Debug("Нажать кнопку 'Создать ТМ'.");
			CreateTMBtn.Click();
			var createTMDialog = new CreateTMDialog();

			return createTMDialog.GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Далее"
		/// </summary>
		public NewProjectSelectGlossariesDialog ClickNextBtn()
		{
			Logger.Debug("Нажать кнопку 'Далее'.");
			NextBtn.Click();
			var newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog();

			return newProjectSelectGlossariesDialog.GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Готово"
		/// </summary>
		public ProjectsPage ClickFinishBtn()
		{
			Logger.Debug("Нажать кнопку 'Готово'.");
			FinishBtn.Click();

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_BTN_XPATH)]
		protected IWebElement CreateTMBtn { get; set; }

		[FindsBy(How = How.XPath, Using = TM_TABLE_FIRST_ITEM_XPATH)]
		protected IWebElement TMTableFirstItem { get; set; }

		[FindsBy(How = How.XPath, Using = NEXT_BTN_XPATH)]
		protected IWebElement NextBtn { get; set; }

		[FindsBy(How = How.XPath, Using = FINISH_BTN_XPATH)]
		protected IWebElement FinishBtn { get; set; }

		protected const string CREATE_TM_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";
		protected const string TM_TABLE_FIRST_ITEM_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//table[contains(@class,'js-tms-popup-table')]//tbody//tr[1]//td[1]//input";
		protected const string NEXT_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
		protected const string FINISH_BTN_XPATH = CREATE_PROJECT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]";
	}
}
