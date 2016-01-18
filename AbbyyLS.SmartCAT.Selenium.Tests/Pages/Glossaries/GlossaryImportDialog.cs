﻿using System.IO;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class GlossaryImportDialog : WorkspacePage, IAbstractPage<GlossaryImportDialog>
	{
		public GlossaryImportDialog(WebDriver driver) : base(driver)
		{
		}

		public new GlossaryImportDialog GetPage()
		{
			var glossaryImportDialog = new GlossaryImportDialog(Driver);
			InitPage(glossaryImportDialog, Driver);

			return glossaryImportDialog;
		}

		public new void LoadPage()
		{
			if (!IsGlossaryImportDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог импорта глоссария");
			}
		}

		#region Простые методы

		/// <summary>
		/// Нажать кнопку Import в диалоге импорта глоссария
		/// </summary>
		public GlossarySuccessImportDialog ClickImportButtonInImportDialog()
		{
			CustomTestContext.WriteLine("Нажать кнопку Import в диалоге импорта глоссария.");
			ImportButton.Click();

			return new GlossarySuccessImportDialog(Driver).GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Replace All' в диалоге импорта глоссария
		/// </summary>
		public GlossaryImportDialog ClickReplaceTermsButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Replace All' в диалоге импорта глоссария");
			ReplaceAllButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		public GlossaryImportDialog SetFileName(string pathFile)
		{
			CustomTestContext.WriteLine("Ввести путь к файлу {0} в поле импорта.", pathFile);
			ImportGlossaryInput.SendKeys(pathFile);

			return GetPage();
		}

		#endregion

		#region Составные методы

		/// <summary>
		/// Ввести путь к файлу в поле импорта
		/// </summary>
		/// <param name="pathFile">путь к файлу</param>
		public GlossaryImportDialog ImportGlossary(string pathFile)
		{
			makeInputDialogVisible();
			SetFileName(pathFile);
			setFileNameForValidation(pathFile);

			return GetPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открыт ли диалог импорта глоссария
		/// </summary>
		public bool IsGlossaryImportDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(IMPORT_BUTTON)) &&
				(Driver.WaitUntilElementIsClickable(By.XPath(REPLACE_ALL_BUTTON)) != null) &&
				(Driver.WaitUntilElementIsClickable(By.XPath(IMPORT_BUTTON)) != null) &&
				Driver.WaitUntilElementIsDisappeared(By.XPath(IMPORT_IN_PROGRESS_MESSAGE), timeout: 20);
		}

		#endregion

		#region Вспомогательные методы

		/// <summary>
		/// Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста
		/// </summary>
		private GlossaryImportDialog makeInputDialogVisible()
		{
			CustomTestContext.WriteLine("Выполнить скрипт для того, чтобы сделать диалог импорта видимым для теста");
			Driver.ExecuteScript("arguments[0].style[\"display\"] = \"block\";" + "arguments[0].style[\"visibility\"] = \"visible\";",
			ImportGlossaryInput);

			return GetPage();
		}

		/// <summary>
		/// Выполнить скрипт для прохождения валидации импорта
		/// </summary>
		/// <param name="pathFile">путь до файла</param>
		private GlossaryImportDialog setFileNameForValidation(string pathFile)
		{
			CustomTestContext.WriteLine("Выполнить скрипт для прохождения валидации импорта");
			Driver.ExecuteScript("document.getElementsByClassName('g-iblock l-editgloss__filelink js-filename-link')[0].innerHTML = '" + Path.GetFileName(pathFile) + "'");

			return GetPage();
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = IMPORT_GLOSSARY_INPUT)]
		protected IWebElement ImportGlossaryInput { get; set; }

		[FindsBy(How = How.XPath, Using = IMPORT_BUTTON)]
		protected IWebElement ImportButton { get; set; }

		[FindsBy(How = How.XPath, Using = REPLACE_ALL_BUTTON)]
		protected IWebElement ReplaceAllButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string IMPORT_GLOSSARY_INPUT = "//form[contains(@action,'Enterprise/Glossaries/Import')]//input[contains(@class,'js-submit-input')]";
		protected const string IMPORT_BUTTON = "//div[contains(@class,'js-popup-import')][2]//div[contains(@class,'js-import-button')]";
		protected const string REPLACE_ALL_BUTTON = "//div[contains(@class,'js-popup-import')][2]//input[contains(@name,'needToClear')][@value='true']//following-sibling::em";
		protected const string IMPORT_IN_PROGRESS_MESSAGE = "//div[contains(@class, 'js-please-wait')]";

		#endregion
	}
}
