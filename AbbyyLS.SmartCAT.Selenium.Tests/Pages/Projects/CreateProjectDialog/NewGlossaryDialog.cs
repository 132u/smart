using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewGlossaryDialog : NewProjectCreateBaseDialog, IAbstractPage<NewGlossaryDialog>
	{
		public NewGlossaryDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewGlossaryDialog LoadPage()
		{
			if (!IsNewGlossaryDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не открылся диалог создания глоссария в процессе создания проекта");
			}

			return this;
		}

		public NewGlossaryDialog SetGlossaryName(string glossaryName)
		{
			CustomTestContext.WriteLine("Ввести название глоссария {0}.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку сохранения глоссария
		/// </summary>
		public NewProjectSelectGlossariesDialog ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку сохранения глоссария");
			SaveButton.Click();

			return new NewProjectSelectGlossariesDialog(Driver).LoadPage();
		}

		/// <summary>
		/// Проверить, открылся ли диалог создания глоссария в процессе создания проекта
		/// </summary>
		public bool IsNewGlossaryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_NAME));
		}

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		protected const string GLOSSARY_NAME = "//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'l-editgloss__nmtext')]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-edit-glossary')][2]//span[@data-bind='click: save']";
	}
}
