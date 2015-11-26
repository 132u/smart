using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectCreateTMDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectCreateTMDialog>
	{
		public NewProjectCreateTMDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectCreateTMDialog GetPage()
		{
			var newProjectCreateTMDialog = new NewProjectCreateTMDialog(Driver);
			InitPage(newProjectCreateTMDialog, Driver);

			return newProjectCreateTMDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewProjectCreateTmDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не появился диалог создания ТМ");
			}
		}

		/// <summary>
		/// Ввести имя новой ТМ
		/// </summary>
		/// <param name="newTMName">имя ТМ</param>
		public NewProjectCreateTMDialog SetNewTMName(string newTMName)
		{
			CustomTestContext.WriteLine("Ввести имя новой ТМ: {0}.", newTMName);
			NewTMNameInput.SetText(newTMName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку "Сохранить"
		/// </summary>
		public NewProjectSetUpTMDialog ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку 'Сохранить'.");
			SaveTMButton.Click();

			return new NewProjectSetUpTMDialog(Driver).GetPage();
		}

		public NewProjectCreateTMDialog UploadTmxFile(string tmxFilePath)
		{
			CustomTestContext.WriteLine("Загрузить файл {0}.", tmxFilePath);
			Driver.ExecuteScript("$(\"input:file\").removeClass(\"g-hidden\").css(\"opacity\", 100)");

			UploadTmxInput.SendKeys(tmxFilePath);

			Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").filenameLink.text($(\".js-import-file-form .js-control\").data(\"controller\").fileInput.val());");
			Driver.ExecuteScript("$(\".js-import-file-form .js-control\").data(\"controller\").trigger(\"valueChanged\");");

			return GetPage();
		}

		/// <summary>
		/// Проверить, открыт ли диалог создания ТМ
		/// </summary>
		public bool IsNewProjectCreateTmDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(NEW_TM_NAME_INPUT));
		}

		[FindsBy(How = How.XPath, Using = NEW_TM_NAME_INPUT)]
		protected IWebElement NewTMNameInput { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_TM_BTN)]
		protected IWebElement SaveTMButton { get; set; }

		[FindsBy(How = How.XPath, Using = UPLOAD_TMX_INPUT)]
		protected IWebElement UploadTmxInput { get; set; }

		protected const string NEW_TM_NAME_INPUT = "//div[contains(@class,'js-popup-create-tm')][2]//input[contains(@data-bind,'value: name')]";
		protected const string SAVE_TM_BTN = "//div[contains(@class,'js-popup-create-tm')][2]//a[contains(@class,'js-tour-tm-save')]";
		protected const string UPLOAD_TMX_INPUT = ".//div[contains(@class,\"js-popup-create-tm\")][2]//input[@type=\"file\"]";
	}
}
