using System.Linq;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class NewGlossaryDialog : GlossariesPage, IAbstractPage<NewGlossaryDialog>
	{
		public new NewGlossaryDialog GetPage()
		{
			var glossaryCreationPage = new NewGlossaryDialog();
			InitPage(glossaryCreationPage);
			LoadPage();

			return glossaryCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(SAVE_GLOSSARY_BUTTON_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница создания глоссария.");
			}
		}

		/// <summary>
		/// Открыть список клиентов
		/// </summary>
		public NewGlossaryDialog OpenClientsList()
		{
			Logger.Debug("Открыть список клиентов");
			ClientsList.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список клиентов открылся
		/// </summary>
		public NewGlossaryDialog AssertClientsListOpened()
		{
			Logger.Trace("Проверить, что список клиентов открылся");
			
			Assert.IsTrue(ClientsListDropDown.Displayed,
				"Произошла ошибка:\n список клиентов не открылся.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewGlossaryDialog AssertClientExistInList(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} есть в списке", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST_XPATH));

			Assert.IsTrue(clientList.First().Text.Contains(clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewGlossaryDialog AssertClientNotExistInList(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} есть в списке", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST_XPATH));

			Assert.IsFalse(clientList.Any(e => e.GetAttribute("innerHTML") == clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CLIENT_LIST_XPATH)]
		protected IWebElement ClientsList { get; set; }

		[FindsBy(How = How.XPath, Using = DROPDOWN_LIST_XPATH)]
		protected IWebElement ClientsListDropDown { get; set; }

		protected const string CLIENT_LIST_XPATH = "//select[contains(@data-bind,'clientsList')]//following-sibling::span";
		protected const string DROPDOWN_LIST_XPATH = "//body/span[contains(@class,'js-dropdown')]";
		protected const string SAVE_GLOSSARY_BUTTON_XPATH = GLOSSARY_CREATION_DIALOG_XPATH + "//span[@data-bind='click: save']";
	}
}
