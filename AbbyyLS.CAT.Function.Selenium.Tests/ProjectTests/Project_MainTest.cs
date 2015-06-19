using System;
using System.Text;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа основных тестов проекта с использованием корпаративного аккаунта
	/// </summary>
	[Category("Standalone")]
	public class Project_MainTest<TWebDriverSettings> : NewProjectTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Старт тестов
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			// Переходим к странице воркспейса
			GoToUrl(RelativeUrlProvider.Workspace);
		}

		/// <summary>
		/// метод для тестирования отмены назначения документа пользователю
		/// </summary>
		[Test]
		public void ReassignDocumentToUserTest()
		{
			//Создать пустой проект
			CreateProject(ProjectUniqueName);
			//Добавление документа
			ImportDocumentProjectSettings(PathProvider.DocumentFile, ProjectUniqueName);
			//Назначение задачи на пользователя
			AssignTask(ProjectUniqueName);
			// Выбрать документ
			ProjectPage.SelectDocument(1);
			ProjectPage.ClickAssignRessponsibleBtn();
			ProjectPage.WaitProgressDialogOpen();
			// Нажать Отмену назначения
			ProjectPage.ClickCancelAssignBtn();
			// Подтвердить
			ProjectPage.ConfirmClickYes();
			// TODO проверить без sleep
			Thread.Sleep(2000);

			// Проверить, изменился ли статус на not Assigned
			Assert.IsTrue(ProjectPage.GetAssignName() != NickName,
				"Имя в дропдауне назначения пользователя не изменилось");

			// Назначить ответственного в окне Progress
			ProjectPage.ClickUserNameCell();
			ProjectPage.ClickAssignUserListUser(NickName);

			// Нажать на Assign
			ProjectPage.ClickAssignBtn();
			// Дождаться появления Cancel
			ProjectPage.WaitCancelAssignBtnDisplay();
			// Нажать на Close
			ProjectPage.CloseAssignDialogClick();
		}
	}
}