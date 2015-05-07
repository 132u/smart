using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.TranslationMemories
{
	public class NewTranslationMemoryDialog : TranslationMemoriesPage, IAbstractPage<NewTranslationMemoryDialog>
	{
		public new NewTranslationMemoryDialog GetPage()
		{
			var tmCreationPage = new NewTranslationMemoryDialog();
			InitPage(tmCreationPage);

			return tmCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_TM_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась форма создания ТМ.");
			}
		}

		/// <summary>
		/// Нажать кнопку открытия списка клиентов
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
		/// Проверить, что клиент присутствует в списке клиентов при создании ТМ
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
		/// Проверить, что клиент присутствует в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public bool GetIsClientExist(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} присутствует в списке", clientName);
			var clientList = Driver.GetElementList(By.XPath(CREATE_TM_CLIENT_ITEM));

			return clientList.Any(e => e.GetAttribute("innerHTML") == clientName);
		}

		/// <summary>
		/// Открыть список групп проектов при создании ТМ
		/// </summary>
		public NewTranslationMemoryDialog OpenProjectGroupsList()
		{
			Logger.Debug("Открыть список групп проектов  при создании ТМ.");
			ProjectGroupsDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список групп проектов открылся при создании ТМ
		/// </summary>
		public NewTranslationMemoryDialog AssertProjectGroupListDisplayed()
		{
			Logger.Trace("Проверить, что список групп проектов открылся при создании ТМ.");

			Assert.IsTrue(ProjectGroupsList.Displayed,
				"Произошла ошибка:\n список групп проектов не открылся при создании ТМ.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов присутствует в списке при создании ТМ
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewTranslationMemoryDialog AssertProjectGroupExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов присутствует в списке при создании ТМ.");
			var projectGroupsList = Driver.GetElementList(By.XPath(CREATE_TM_PROJECT_GROUPS_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsTrue(projectGroupExist, "Произошла ошибка:\n группа проектов {0} отсутствует в списке при создании ТМ.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов отсутствует в списке при создании ТМ
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewTranslationMemoryDialog AssertProjectGroupNotExist(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов отсутствует в списке при создании ТМ.");
			var projectGroupsList = Driver.GetElementList(By.XPath(CREATE_TM_PROJECT_GROUPS_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsFalse(projectGroupExist, "Произошла ошибка:\n группа проектов {0} присутствует в списке при создании ТМ.", projectGroupName);

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT)]
		protected IWebElement CreateClientDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_CLIENT_LIST)]
		protected IWebElement CreateClientList { get; set; }

		[FindsBy(How = How.XPath, Using = PROJECT_GROUP_FILTER)]
		protected IWebElement ProjectGroupsDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = CREATE_TM_PROJECT_GROUPS_LIST)]
		protected IWebElement ProjectGroupsList { get; set; }

		protected const string CREATE_TM_PROJECT_GROUPS_LIST = ".//div[contains(@class,'ui-multiselect-menu')][2]/ul//span[2]";
		protected const string PROJECT_GROUP_FILTER= "//select[contains(@data-bind,'allDomainsList')]//following-sibling::div";
		protected const string CREATE_TM_PROJECT_GROUP = ".//div[contains(@class,'ui-multiselect-text')]//span[contains(text(), 'Select project group')]";
		protected const string CREATE_TM_CLIENT = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span";
		protected const string CREATE_TM_CLIENT_LIST = "//select[contains(@data-bind,'allClientsList')]//following-sibling::span[contains(@class,'active')]";
		protected const string CREATE_TM_CLIENT_ITEM = "//select[contains(@data-bind,'allClientsList')]/option";
		protected const string SAVE_TM_BUTTON = ".//div[contains(@class,'js-popup-create-tm')][2]//span[contains(@data-bind, 'click: save')]";
	}
}
