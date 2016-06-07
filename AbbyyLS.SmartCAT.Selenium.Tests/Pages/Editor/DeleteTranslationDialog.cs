using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class DeleteTranslationDialog: EditorPage, IAbstractPage<DeleteTranslationDialog>
	{
		public DeleteTranslationDialog(WebDriver driver) : base(driver)
		{
		}

		public new DeleteTranslationDialog LoadPage()
		{
			if (!IsDeleteTranslationDialogOpened())
			{
				throw new XPathLookupException("Произошла ошибка:\n не открылся диалог удаления перевода.");
			}

			return this;
		}
		
		#region Простые методы страницы

		/// <summary>
		/// Нажать на кнопку Yes.
		/// </summary>
		public EditorPage ClickYesButton()
		{
			CustomTestContext.WriteLine("Нажать на кнопку Yes.");
			YesButton.Click();

			return new EditorPage(Driver).LoadPage();
		}

		#endregion

		#region Методы, проверяющие состояние страницы

		/// <summary>
		/// Проверить, что открылся диалог удаления перевода.
		/// </summary>
		public bool IsDeleteTranslationDialogOpened()
		{
			return Driver.WaitUntilElementIsDisplay(By.XPath(YES_BUTTON));
		}

		#endregion

		#region Объявление элементов страницы

		[FindsBy(How = How.XPath, Using = YES_BUTTON)]
		protected IWebElement YesButton { get; set; }

		#endregion

		#region Описание XPath элементов страницы

		protected const string YES_BUTTON = "//span[@id='button-1021-btnInnerEl']";

		#endregion
	}
}
