using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class SelectTaskDialog : EditorPage, IAbstractPage<SelectTaskDialog>
	{
		public SelectTaskDialog(WebDriver driver) : base(driver)
		{
		}

		public new SelectTaskDialog GetPage()
		{
			var selectTaskDialog = new SelectTaskDialog(Driver);
			InitPage(selectTaskDialog, Driver);

			return selectTaskDialog;
		}

		public new void LoadPage()
		{
			if (!IsSelectTaskDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог с выбором задания в редакторе.");
			}
		}

		/// <summary>
		/// Нажать на кнопку "Перевод"
		/// </summary>
		public SelectTaskDialog ClickTranslateButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Перевод'.");
			TranslateButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Менеджер"
		/// </summary>
		public SelectTaskDialog ClickManagerButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Менеджер'.");
			ManagerButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Editing"
		/// </summary>
		public SelectTaskDialog ClickEditingButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Editing'.");
			EditingButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать на кнопку "Продолжить"
		/// </summary>
		public EditorPage ClickContinueButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Продолжить'.");
			ContinueButton.Click();

			return new EditorPage(Driver).GetPage();
		}

		/// <summary>
		/// Посчитать количество этапов в диалоге выбора задачи.
		/// </summary>
		public int GetTaskCount()
		{
			CustomTestContext.WriteLine("Посчитать количество этапов в диалоге выбора задачи.");

			return Driver.GetElementsCount(By.XPath(TASK_LIST));
		}

		/// <summary>
		/// Выбрать режим работы
		/// </summary>
		/// <param name="mode">режим</param>
		public EditorPage SelectTask(TaskMode mode = TaskMode.Translation)
		{
			switch (mode)
			{
				case TaskMode.Translation:
					ClickTranslateButton();
					break;

				case TaskMode.Manager:
					ClickManagerButton();
					break;

				case TaskMode.Editing:
					ClickEditingButton();
					break;

				default:
					throw new Exception(string.Format("Передан аргумент, который не предусмотрен! Значение аргумента:'{0}'", mode.ToString()));
			}

			var editorPage = ClickContinueButton();

			return editorPage;
		}

		/// <summary>
		/// Проверить, открылся ли диалог выбора задачи
		/// </summary>
		public bool IsSelectTaskDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(TRANSLATE_BTN_XPATH), 30);
		}

		[FindsBy(How = How.XPath, Using = TRANSLATE_BTN_XPATH)]
		protected IWebElement TranslateButton { get; set; }

		[FindsBy(How = How.XPath, Using = MANAGER_BTN_XPATH)]
		protected IWebElement ManagerButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONTINUE_BTN_XPATH)]
		protected IWebElement ContinueButton { get; set; }

		[FindsBy(How = How.XPath, Using = EDITING_BUTTON)]
		protected IWebElement EditingButton { get; set; }

		protected const string TRANSLATE_BTN_XPATH = "//span[contains(string(), 'Translation')]";
		protected const string MANAGER_BTN_XPATH = "//span[contains(@id, 'manager')]";
		protected const string CONTINUE_BTN_XPATH = "//span[contains(string(), 'Continue')]";
		protected const string EDITING_BUTTON = "//span[contains(string(), 'Editing')]";
		protected const string TASK_LIST = "//div[contains(@class, 'x-segmented-button-row')]";
	}
}
