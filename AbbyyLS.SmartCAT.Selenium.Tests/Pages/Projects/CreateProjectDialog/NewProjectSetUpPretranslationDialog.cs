using OpenQA.Selenium;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSetUpPretranslationDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSetUpPretranslationDialog>
	{
		public NewProjectSetUpPretranslationDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSetUpPretranslationDialog LoadPage()
		{
			if (!IsNewProjectSetUpPresentationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не удалось перейти к этапу Pretranslation");
			}

			return this;
		}

		/// <summary>
		/// Проверить, открыть ли диалог Presentation
		/// </summary>
		public bool IsNewProjectSetUpPresentationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(PRETRANSLATION_HEADER));
		}

		protected const string PRETRANSLATION_HEADER = "//span[contains(text(), 'Pretranslation')]";
	}
}
