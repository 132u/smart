using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries
{
	public class NewGlossaryDialog : GlossariesPage, IAbstractPage<NewGlossaryDialog>
	{
		public new NewGlossaryDialog GetPage()
		{
			var glossaryCreationPage = new NewGlossaryDialog();
			InitPage(glossaryCreationPage);

			return glossaryCreationPage;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(SAVE_GLOSSARY_BUTTON)))
			{
				Assert.Fail("Произошла ошибка:\n не загрузилась страница создания глоссария.");
			}
		}

		/// <summary>
		/// Открыть список клиентов при создании глоссария
		/// </summary>
		public NewGlossaryDialog OpenClientsList()
		{
			Logger.Debug("Открыть список клиентов при создании глоссария");
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
			Logger.Trace("Проверить, что клиент {0} есть в списке при создании глоссария.", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST));

			Assert.IsTrue(clientList.First().Text.Contains(clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов при создании глоссария.", clientName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что клиент есть в списке
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		public NewGlossaryDialog AssertClientNotExistInList(string clientName)
		{
			Logger.Trace("Проверить, что клиент {0} есть в списке при создании глоссария.", clientName);
			var clientList = Driver.GetElementList(By.XPath(DROPDOWN_LIST));

			Assert.IsFalse(clientList.Any(e => e.GetAttribute("innerHTML") == clientName),
				"Произошла ошибка:\n клиент {0} не отображается в списке клиентов при создании глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Открыть список групп проектов
		/// </summary>
		public NewGlossaryDialog OpenProjectGroupsList()
		{
			Logger.Debug("Открыть список групп проектов при создании глоссария.");
			ProjectGroupsDropDown.Click();

			return GetPage();
		}

		/// <summary>
		/// Проверить, что список групп проектов открылся
		/// </summary>
		public NewGlossaryDialog AssertProjectGroupsListOpened()
		{
			Logger.Trace("Проверить, что список групп проектов открылся при создании глоссария.");

			Assert.IsTrue(ProjectGroupsList.Displayed,
				"Произошла ошибка:\n список групп проектов не открылся при создании глоссария.");

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов есть в списке при создании глоссария
		/// </summary>
		/// <param name="projectGroupName">имя группы проектов</param>
		public NewGlossaryDialog AssertProjectGroupExistInList(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} присутствует в списке при создании глоссария.", projectGroupName);
			var projectGroupsList = Driver.GetElementList(By.XPath(MULTISELECT_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsTrue(projectGroupExist, "Произошла ошибка:\n  группа проектов {0} отсутствует в списке при создании глоссария.", projectGroupName);

			return GetPage();
		}

		/// <summary>
		/// Проверить, что группа проектов отсутствует в списке при создании глоссария
		/// </summary>
		public NewGlossaryDialog AssertProjectGroupNotExistInList(string projectGroupName)
		{
			Logger.Trace("Проверить, что группа проектов {0} отсутствует в списке при создании глоссария.", projectGroupName);
			var projectGroupsList = Driver.GetElementList(By.XPath(MULTISELECT_LIST));
			var projectGroupExist = projectGroupsList.Any(e => e.GetAttribute("innerHTML") == projectGroupName);

			Assert.IsFalse(projectGroupExist, "Произошла ошибка:\n  группа проектов {0} присутствует в списке при создании глоссария.", projectGroupName);

			return GetPage();
		}

		public NewGlossaryDialog AssertNewGlossaryDialogAppear()
		{
			Logger.Trace("Проверить, что диалог создания глоссария открылся");
			
			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(NEW_GLOSSARY_DIALOG)),
				"Произошла ошибка:\n диалог создания глоссария не открылся.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CLIENT_LIST)]
		protected IWebElement ClientsList { get; set; }

		[FindsBy(How = How.XPath, Using = DROPDOWN_LIST)]
		protected IWebElement ClientsListDropDown { get; set; }


		[FindsBy(How = How.XPath, Using = PROJECT_GROUPS_LIST)]
		protected IWebElement ProjectGroupsDropDown { get; set; }

		[FindsBy(How = How.XPath, Using = MULTISELECT_LIST)]
		protected IWebElement ProjectGroupsList { get; set; }

		protected const string CLIENT_LIST= "//select[contains(@data-bind,'clientsList')]//following-sibling::span";
		protected const string DROPDOWN_LIST= "//body/span[contains(@class,'js-dropdown')]";
		protected const string SAVE_GLOSSARY_BUTTON= ".//div[contains(@class,'js-popup-edit-glossary')][2]//span[@data-bind='click: save']";
		protected const string PROJECT_GROUPS_LIST = ".//div[contains(@class,'js-popup-edit-glossary')][2]//div[@class='l-editgloss__contrbox'][3]//div";
		protected const string NEW_GLOSSARY_DIALOG = ".//div[contains(@class,'js-popup-edit-glossary')][2]";
		protected const string MULTISELECT_LIST = ".//ul[contains(@class,'ui-multiselect-checkboxes')]//span[contains(@class,'ui-multiselect-item-text')]";
	}
}
