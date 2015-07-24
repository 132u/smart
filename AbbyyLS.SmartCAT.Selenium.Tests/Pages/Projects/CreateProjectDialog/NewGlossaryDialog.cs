using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewGlossaryDialog : NewProjectCreateBaseDialog, IAbstractPage<NewGlossaryDialog>
	{
		public new NewGlossaryDialog GetPage()
		{
			var newGlossaryDialog = new NewGlossaryDialog();
			InitPage(newGlossaryDialog);

			return newGlossaryDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_NAME)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог создания глоссария в процессе создания проекта.");
			}
		}

		public NewGlossaryDialog SetGlossaryName(string glossaryName)
		{
			Logger.Debug("Ввести название глоссария {0}.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return GetPage();
		}

		public NewProjectSelectGlossariesDialog ClickSaveButton()
		{
			Logger.Debug("Нажать кнопку сохранения глоссария.");
			SaveButton.Click();

			Driver.WaitUntilElementIsDisappeared(By.XPath(SAVE_BUTTON));

			return new NewProjectSelectGlossariesDialog().GetPage();
		}

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		protected const string GLOSSARY_NAME = "//div[contains(@class,'js-popup-edit-glossary')][2]//input[contains(@class,'l-editgloss__nmtext')]";
		protected const string SAVE_BUTTON = "//div[contains(@class,'js-popup-edit-glossary')][2]//span[@data-bind='click: save']";
	}
}
