using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class TermAlreadyExistsDialog : WorkspacePage, IAbstractPage<TermAlreadyExistsDialog>
	{
		public TermAlreadyExistsDialog(WebDriver driver) : base(driver)
		{
		}

		public new TermAlreadyExistsDialog LoadPage()
		{
			if (!IsTermAlreadyExistsDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог сохранения уже существующего термина");
			}

			return this;
		}

		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку 'Сохранить изменения'.
		/// </summary>
		public GlossaryPage ClickSaveChangesButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку 'Сохранить изменения'.");
			SaveChangesButton.Click();

			return new GlossaryPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, открылся диалог сохранения уже существующего термина
		/// </summary>
		public bool IsTermAlreadyExistsDialogOpened()
		{
			CustomTestContext.WriteLine("Проверить, открылся диалог сохранения уже существующего термина");

			return Driver.WaitUntilElementIsDisplay(By.XPath(ALREADY_EXIST_TERM_ERROR));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = SAVE_CHANGES_BUTTON)]
		protected IWebElement SaveChangesButton { get; set; }

		#endregion

		#region Описание XPath элементов

		protected const string ALREADY_EXIST_TERM_ERROR = "//span[contains(text(),'The term already exists')]";
		protected const string SAVE_CHANGES_BUTTON = "//span[text()='The term already exists']/../..//following-sibling::div//a[contains(text(), 'Save')]//parent::div";

		#endregion
	}
}
