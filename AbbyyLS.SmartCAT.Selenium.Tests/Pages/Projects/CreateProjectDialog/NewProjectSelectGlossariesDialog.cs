using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectSelectGlossariesDialog : NewProjectCreateBaseDialog, IAbstractPage<NewProjectSelectGlossariesDialog>
	{
		public NewProjectSelectGlossariesDialog(WebDriver driver) : base(driver)
		{
		}

		public new NewProjectSelectGlossariesDialog GetPage()
		{
			var newProjectSelectGlossariesDialog = new NewProjectSelectGlossariesDialog(Driver);
			InitPage(newProjectSelectGlossariesDialog, Driver);

			return newProjectSelectGlossariesDialog;
		}

		public new void LoadPage()
		{
			if (!IsNewProjectSelectGlossariesDialogOpened())
			{
				throw new XPathLookupException(
					"Произошла ошибка:\n не удалось перейти к третьему шагу создания проекта (выбор глоссария)");
			}
		}

		/// <summary>
		/// Нажать кнопку 'Create Glossary'
		/// </summary>
		public NewGlossaryDialog ClickCreateGlossary()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Create Glossary'.");
			CreateGlossaryButton.Click();

			return new NewGlossaryDialog(Driver).GetPage();
		}

		/// <summary>
		/// Выбрать первый глоссарий
		/// </summary>
		public NewProjectSelectGlossariesDialog ClickFirstGlossary()
		{
			CustomTestContext.WriteLine("Выбрать первый глоссарий.");
			FirstGlossaryInput.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что выбран первый глоссарий
		/// </summary>
		public bool IsFirstGlossarySelected()
		{
			CustomTestContext.WriteLine("Проверить, что выбран первый глоссарий.");

			return FirstGlossaryInput.Selected;
		}

		/// <summary>
		/// Проверить, открылся ли диалог выбора глоссариев (шаг 3 создания проекта)
		/// </summary>
		public bool IsNewProjectSelectGlossariesDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(CREATE_GLOSSARY_BUTTON));
		}

		[FindsBy(How = How.XPath, Using = CREATE_GLOSSARY_BUTTON)]
		protected IWebElement CreateGlossaryButton { get; set; }

		[FindsBy(How = How.XPath, Using = FIRST_GLOSSARY_INPUT)]
		protected IWebElement FirstGlossaryInput { get; set; }

		protected const string CREATE_GLOSSARY_BUTTON = "//div[contains(@class,'js-popup-create-project')][2]//span[contains(@class,'js-glossary-create')]";
		protected const string FIRST_GLOSSARY_INPUT = "//div[contains(@class,'js-popup-create-project')][2]//table[contains(@class,'js-glossaries')]//tbody//tr[1]/*/span[contains(@class,'js-chckbx')]//input";
	}
}
