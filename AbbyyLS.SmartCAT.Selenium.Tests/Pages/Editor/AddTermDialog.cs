using NUnit.Framework;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor
{
	public class AddTermDialog : BaseObject, IAbstractPage<AddTermDialog>
	{
		public AddTermDialog GetPage()
		{
			var addTermDialog = new AddTermDialog();
			InitPage(addTermDialog);

			return addTermDialog;
		}

		public void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SOURCE_TERM)))
			{
				Assert.Fail("Произошла ошибка:\n не открылся диалог создания нового термина.");
			}
		}

		/// <summary>
		/// Ввести текст в source
		/// </summary>
		public AddTermDialog FillSourceTerm(string sourceTerm)
		{
			Logger.Debug("Ввести {0} в source.", sourceTerm);
			SourceTerm.SetText(sourceTerm);

			return GetPage();
		}

		/// <summary>
		/// Ввести текст в target
		/// </summary>
		public AddTermDialog FillTargetTerm(string targetTerm)
		{
			Logger.Debug("Ввести {0} в target.", targetTerm);
			TargetTerm.SetText(targetTerm);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку Add в диалоге создания термина
		/// </summary>
		public EditorPage ClickAddButton()
		{
			Logger.Debug("Нажать кнопку Add в диалоге создания термина");
			AddButton.Click();

			return new EditorPage().GetPage();
		}

		[FindsBy(How = How.XPath, Using = SOURCE_TERM)]
		protected IWebElement SourceTerm { get; set; }

		[FindsBy(How = How.XPath, Using = TARGET_TERM)]
		protected IWebElement TargetTerm { get; set; }

		[FindsBy(How = How.XPath, Using = ADD_BUTTON)]
		protected IWebElement AddButton { get; set; }

		protected const string SOURCE_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'sourceTerm')]";
		protected const string TARGET_TERM = "//div[contains(@id, 'term-window')]//input[contains(@name, 'targetTerm')]";
		protected const string ADD_BUTTON = "//div[contains(@id, 'term-window')]//span[contains(string(), 'Add')]";

	}
}