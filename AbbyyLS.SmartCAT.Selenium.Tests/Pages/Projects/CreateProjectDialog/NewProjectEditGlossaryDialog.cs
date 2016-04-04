using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog
{
	public class NewProjectEditGlossaryDialog : NewProjectSettingsPage, IAbstractPage<NewProjectEditGlossaryDialog>
	{
		public NewProjectEditGlossaryDialog(WebDriver driver)
			: base(driver)
		{
		}

		public new NewProjectEditGlossaryDialog LoadPage()
		{
			if (!IsNewProjectEditGlossaryDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог редактирования глоссария.");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Ввести название глоссария
		/// </summary>
		/// <param name="glossaryName">название глоссария</param>
		public NewProjectEditGlossaryDialog FillGlossaryName(string glossaryName)
		{
			CustomTestContext.WriteLine("Ввести название глоссария {0}.", glossaryName);
			GlossaryName.SetText(glossaryName);

			return LoadPage();
		}

		/// <summary>
		/// Нажать кнопку Save.
		/// </summary>
		public NewProjectSettingsPage ClickSaveButton()
		{
			CustomTestContext.WriteLine("Нажать кнопку Save.");
			SaveButton.Click();

			return new NewProjectSettingsPage(Driver).LoadPage();
		}

		#endregion

		#region Составные методы страницы

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся ли диалог редактирования глоссария
		/// </summary>
		public bool IsNewProjectEditGlossaryDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(GLOSSARY_NAME));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = GLOSSARY_NAME)]
		protected IWebElement GlossaryName { get; set; }

		[FindsBy(How = How.XPath, Using = SAVE_BUTTON)]
		protected IWebElement SaveButton { get; set; }

		#endregion

		#region Описания XPath элементов

		protected const string GLOSSARY_NAME = "//div[contains(@class, 'glossary-edit-popup')][2]//input[contains(@data-bind, 'name')]";
		protected const string SAVE_BUTTON = "//div[contains(@class, 'glossary-edit-popup')][2]//a[contains(@data-bind, 'click: save')]";

		#endregion
	}
}
