using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog
{
	public class DublicateFileErrorDialog : DocumentUploadBaseDialog, IAbstractPage<DublicateFileErrorDialog>
	{
		public DublicateFileErrorDialog(WebDriver driver) : base(driver)
		{
		}

		public new DublicateFileErrorDialog LoadPage()
		{
			if (!IsDublicateFileErrorDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появилось сообщение о ошибке.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать кнопку Ок.
		/// </summary>
		public AddFilesStep ClickOkButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Ок.");
			OkButton.Click();

			return new AddFilesStep(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// проверить, что открылся диалог с сообщением, что файл уже загружен.
		/// </summary>
		public bool IsDublicateFileErrorDialogOpened()
		{
			return Driver.WaitUntilElementIsAppear(By.XPath(UPDATE_BUTTON));
		}

		/// <summary>
		/// Проверить, что имя файла отображается в диалоге.
		/// </summary>
		/// <param name="fileName">имя файла</param>
		public bool IsFileNameDisplayed(string fileName)
		{
			CustomTestContext.WriteLine("Проверить, что имя файла отображается в диалоге.");

			return Driver.WaitUntilElementIsDisplay(By.XPath(DUPLICATE_FILE_NAME.Replace("*#*", fileName)));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = OK_BUTTON)]
		protected IWebElement OkButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPDATE_BUTTON)]
		protected IWebElement LoadButton { get; set; }

		[FindsBy(How = How.XPath, Using = LOAD_BUTTON)]
		protected IWebElement UploadButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string DUPLICATE_FILE_NAME = "//div[contains(@class, 'duplicate-file')]//span[contains(text(),'*#*')]";
		protected const string UPDATE_BUTTON = "//div[contains(@class,'duplicate-settings-popup')]//label[contains(@data-bind, 'update')]//input";
		protected const string LOAD_BUTTON = "//div[contains(@class,'duplicate-settings-popup')]//label[contains(@data-bind, 'load')]//input";
		protected const string OK_BUTTON = "//button[contains(@data-bind, 'save')]";

		#endregion
	}
}
