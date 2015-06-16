using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects
{
	public class ResponsiblesDialog : ProjectsPage, IAbstractPage<ResponsiblesDialog>
	{
		public new ResponsiblesDialog GetPage()
		{
			var responsiblesDialog = new ResponsiblesDialog();
			InitPage(responsiblesDialog);

			return responsiblesDialog;
		}

		public new void LoadPage()
		{
			if (!Driver.WaitUntilElementIsDisplay(By.XPath(RESPONSIBLES_TABLE)))
			{
				Assert.Fail("Произошла ошибка:\n не удалось открыть диалог выбора исполнителя.");
			}
		}

		/// <summary>
		/// Открыть выпадющий список для задачи с заданным номером
		/// </summary>
		/// <param name="taskRowNumber"> номер задачи</param>
		public ResponsiblesDialog OpenTaskResponsibles(int taskRowNumber = 1)
		{
			Logger.Trace("Открыть выпадающий список для задачи с номером строки {0}", taskRowNumber);

			ResponsiblesDropbox = Driver.SetDynamicValue(How.XPath, RESPONSIBLES_DROPBOX, taskRowNumber.ToString());
			ResponsiblesDropbox.Click();

			return GetPage();
		}

		/// <summary>
		/// Подтвердить, что открылся выпадающий список для задачи
		/// </summary>
		/// <param name="taskRowNumber">номер задачи</param>
		public ResponsiblesDialog AssertTaskResponsiblesListDisplay(int taskRowNumber = 1)
		{
			Logger.Trace("Подтвердить, что открылся выпадающий список для задачи с номером строки {0}", taskRowNumber);

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(RESPONSIBLES_DROPBOX_OPTION.Replace("*#*", taskRowNumber.ToString()))),
				"Произошла ошибка:\n список исполнителей не открылся");

			return GetPage();
		}

		/// <summary>
		/// Получить список исполнителей
		/// </summary>
		public List<string> GetResponsibleUsersList()
		{
			Logger.Trace("Получить список исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(RESPONSIBLES_LIST.Replace("*#*","")));

			return (from element in elementUsersList
					where !element.Contains("Group: ")
					select element.Replace("  ", " ")).ToList();
		}

		/// <summary>
		/// Получить список групп исполнителей
		/// </summary>
		public List<string> GetResponsibleGroupsList()
		{
			Logger.Trace("Получить список групп исполнителей.");

			var elementUsersList = Driver.GetTextListElement(By.XPath(RESPONSIBLES_LIST.Replace("*#*", "")));

			return (from element in elementUsersList
					where element.Contains("Group: ")
					select element.Replace("  ", " ")).ToList();
		}

		/// <summary>
		/// Выбрать из выпадающего списка пользователя или группу по имени
		/// </summary>
		/// <param name="name">Имя пользователя или группы</param>
		/// <param name="isGroup">Выбор группы</param>
		public ResponsiblesDialog SelectResponsible(string name, bool isGroup)
		{
			Logger.Debug("Выбрать из выпадающего списка {1}. Это группа: {2}", name, isGroup);

			name = name.Replace(" ", "  ");
			var fullName = isGroup ? "Group: " + name : name;

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(RESPONSIBLES_LIST.Replace("*#*", fullName))),
				"Произошла ошибка:\n пользователь {0} не найден в выпадающем"
				+ " списке при назначении исполнителя на задачу", fullName);

			ResponsiblesDropbox.SelectOptionByText(fullName);

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Назначить'
		/// </summary>
		/// <param name="taskNumber">номер задачи</param>
		public ResponsiblesDialog ClickAssignButton(int taskNumber = 1)
		{
			Logger.Debug("Нажать кнопку 'Назначить'");

			AssignButton = Driver.SetDynamicValue(How.XPath, ASSIGN_BUTTON, taskNumber.ToString());
			AssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку 'Закрыть'
		/// </summary>
		/// <returns></returns>
		public T ClickCloseAssignDialog<T>() where T: class, IAbstractPage<T>, new()
		{
			Logger.Debug("Нажать кнопку 'Закрыть'");
			CloseButton.Click();

			return new T().GetPage();
		}

		/// <summary>
		/// Нажать кнопку отмены назначения исполнителя задачи
		/// </summary>
		/// <param name="taskNumber"> номер задачи </param>
		public ResponsiblesDialog ClickCancelAssignButton(int taskNumber = 1)
		{
			Logger.Trace("Нажать кнопку отмены назначения исполнителя для задачи с номером '{0}'", taskNumber);

			CancelAssignButton = Driver.SetDynamicValue(How.XPath, CANCEL_ASSIGN_BUTTON, taskNumber.ToString());
			CancelAssignButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Дождаться поялвения кнопки подтверждения удаления назначения пользователя
		/// </summary>
		public ResponsiblesDialog WaitCancelConfirmButtonDisplay()
		{
			Logger.Trace("Дождаться появления кнопки подтверждения удаления назначения пользователя");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CONFIRM_CANCEL_BUTTON)),
				"Произошла ошибка:\n не появилась кнопка подтверждения удаления назначения пользователя");

			return GetPage();
		}

		/// <summary>
		/// Нажать кнопку подтверждения удаления назначения пользователя
		/// </summary>
		public ResponsiblesDialog ClickConfirmCancelButton()
		{
			Logger.Trace("Нажать кнопку подтверждения удаления назначения пользователя");
			ConfirmCancelButton.Click();

			return GetPage();
		}

		/// <summary>
		/// Получить статус назначения на задачу
		/// </summary>
		public string GetAssignStatus()
		{
			Logger.Trace("Получить статус назначения на задачу");

			return AssignStatus.Text;
		}

		/// <summary>
		/// Проверить, что кнопка отмены назначения на задачу появилась
		/// </summary>
		public ResponsiblesDialog AssertCancelAssignButtonExist(int taskNumber = 1)
		{
			Logger.Trace("Проверить, что кнопка отмены назначения на задачу появилась");

			Assert.IsTrue(Driver.WaitUntilElementIsDisplay(By.XPath(CANCEL_ASSIGN_BUTTON.Replace("*#*", taskNumber.ToString()))),
				"Произошла ошибка:\n кнопка отмены назначения на задачу не появилась.");

			return GetPage();
		}

		[FindsBy(How = How.XPath, Using = CLOSE_BUTTON)]
		protected IWebElement CloseButton { get; set; }

		[FindsBy(How = How.XPath, Using = CONFIRM_CANCEL_BUTTON)]
		protected IWebElement ConfirmCancelButton { get; set; }

		[FindsBy(How = How.XPath, Using = ASSIGN_STATUS)]
		protected IWebElement AssignStatus { get; set; }

		protected IWebElement ResponsiblesDropbox { get; set; }

		protected IWebElement AssignButton { get; set; }

		protected IWebElement CancelAssignButton { get; set; }

		protected const string RESPONSIBLES_TABLE = "(//table[contains(@class, 'js-progress-table')]//table)[2]";
		protected const string RESPONSIBLES_DROPBOX = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[*#*]//td[contains(@class, 'assineer')]//select";
		protected const string RESPONSIBLES_LIST = "(//div[contains(@class, 'js-popup-assign')])[2]//td[contains(@class, 'assineer')]//select//option[contains(text() , '*#*')]";
		protected const string ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//a[contains(@data-bind, 'click: assign')])[*#*]";
		protected const string CLOSE_BUTTON = "(//div[contains(@class, 'js-popup-assign')])[2]//span[contains(@class, 'js-popup-close')]";
		protected const string CANCEL_ASSIGN_BUTTON = "((//table[contains(@class, 'js-progress-table')]//table)[2]//span[contains(@class,'js-assigned-cancel')])[*#*]";
		protected const string CONFIRM_CANCEL_BUTTON = "//div[contains(@class,'l-confirm')]//span[span[input[@value='Cancel Assignment']]]";
		protected const string ASSIGN_STATUS = "(//span[@data-bind='text: status()'])[2]";
		protected const string RESPONSIBLES_DROPBOX_OPTION = "(//table[contains(@class, 'js-progress-table')]//table)[2]//tr[1]//td[contains(@class, 'assineer')]//select/option[1]";
	}
}
