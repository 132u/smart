using System.Linq;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class NewTranslationMemoryDialog : TranslationMemoriesPage, IAbstractPage<NewTranslationMemoryDialog>
	{
		public new NewTranslationMemoryDialog GetPage()
		{
			var tmCreationPage = new NewTranslationMemoryDialog();
			InitPage(tmCreationPage);
			LoadPage();

			return tmCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsPresent(By.XPath(SAVE_TM_BUTTON_XPATH)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась форма создания ТМ.");
			}
		}

		/// <summary>
		/// Нажатие кнопки открытия списка клиентов
		/// </summary>
		public NewTranslationMemoryDialog ClickOpenClientsList()
		{
			Logger.Debug("Нажать кнопку открытия списка клиентов");
			CreateClientDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить отображения списка клиентов
		/// </summary>
		public NewTranslationMemoryDialog AssertClientsListDisplayed()
		{
			Logger.Trace("Проверить отображения списка клиентов");
			
			Assert.IsTrue(CreateClientList.Displayed,
				"Произошла ошибка:\n не отображен список клиентов.");

			return GetPage();
		}

		/// <summary>
		/// Проеверить, что клиент имеется в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog AssertClientExistInTmCreationDialog(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} имеется в списке клиентов при создании ТМ", clientName);

			Assert.IsTrue(GetIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} не отображен в списке клиентов.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проеверить, что клиент отсутствует в списке клиентов при создании ТМ
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewTranslationMemoryDialog AssertClientNotExistInTmCreationDialog(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} отсутствует в списке клиентов при создании ТМ", clientName);

			Assert.IsFalse(GetIsClientExist(clientName),
				"Произошла ошибка:\n клиент {0} имеется в списке клиентов.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public bool GetIsClientExist(string clientName)
		{
			var clientList = Driver.GetElementList(By.XPath(CREATE_TM_CLIENT_ITEM_XPATH));

			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT_XPATH)]
		protected IWebElement CreateClientDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT_LIST_XPATH)]
		protected IWebElement CreateClientList { get; set; }

		protected const string CREATE_TM_CLIENT_XPATH = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span";
		protected const string CREATE_TM_CLIENT_LIST_XPATH = CREATE_TM_CLIENT_XPATH + "[contains(@class,'active')]";
		protected const string CREATE_TM_CLIENT_ITEM_XPATH = "//select[contains(@data-bind,'allClientsList')]/option";
		protected const string SAVE_TM_BUTTON_XPATH = CREATE_TM_DIALOG_XPATH + "//span[contains(@data-bind, 'click: save')]";
	}
}
