using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpMTDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpMTDialog>
	{
		public NewProjectSetUpMTDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSetUpMTDialog LoadPage()
		{
			if (!IsNewProjectSetUpMTDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не удалось перейти к четвертому шагу создания проекта (выбор МТ)");
			}

			return this;
		}

		/// <summary>
		/// Проверить, открылся ли диалог выбора MT (шаг 4 создания проекта)
		/// </summary>
		public bool IsNewProjectSetUpMTDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(MT_TABLE));
		}

		[FindsBy(How = How.XPath, Using = FINISH_BTN)]
		protected IWebElement FinishButton { get; set; }

		protected const string MT_TABLE = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-mts-body')]//tbody";
		protected const string FINISH_BTN = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-finish js-upload-btn')]";
	}
}
