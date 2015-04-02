using System.Threading;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа основных тестов проекта с использованием персонального аккаунта
	/// </summary>
	public class Project_MainPersAccTest<TWebDriverSettings> : NewProjectTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			GoToUrl(RelativeUrlProvider.Workspace, "Personal");
		}

		/// <summary>
		/// Создание проекта без файла с помощью персонального аккаунта
		/// </summary>
		[Test]
		public void CreateProjectNoFilePersAcc()
		{
			// Создать проект
			CreateProject(ProjectUniqueName, "", true);
			//проверить что проект появился с списке проектов
			CheckProjectInList(ProjectUniqueName);
		}

		/// <summary>
		/// Создание проекта с существующим именем с помощью персонального аккаунта
		/// </summary>
		[Test]
		public void CreateProjectDuplicateNameTestPersAcc()
		{
			// Создать проект
			CreateProject(ProjectUniqueName);
			Thread.Sleep(1000);
			// Начать создание проекта с этим же именем
			FirstStepProjectWizard(ProjectUniqueName);
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить, что появилась ошибка и поле Имя выделено ошибкой - ASSERT внутри
			AssertErrorDuplicateName();
		}

		/// <summary>
		/// Проверка невозможности создания проекта в большим именем(>100 символов) с помощью персонального аккаунта
		/// </summary>
		[Test]
		public void CreateProjectBigNameTestPersAcc()//Переделать тест, теперь поле с ограничением символов
		{
			string bigName = ProjectUniqueName + "12345678901234567890123456789012345678901234567890123456789012345678901";
			// Проверить, что создалось имя длиннее 100 символов
			Assert.IsTrue(bigName.Length > 100, "Измените тест: длина имени должна быть больше 100");
			// Создать проект с превышающим лимит именем
			CreateProjectWithoutCheckExist(bigName);
			// Проверить, что проект не сохранился
			Assert.IsTrue(GetIsNotExistProject(bigName), "Ошибка: проект с запрещенно большим именем создался");
		}

		/// <summary>
		/// Тест, что Assign task отсутствует на стр списка проектов для песронального аккаунта
		/// </summary>
		[Test]
		public void AssignTaskBtnTest()
		{
			//Создать пустой проект
			CreateProject(ProjectUniqueName);

			//Добавление документа
			ImportDocumentProjectSettings(PathProvider.DocumentFile, ProjectUniqueName, "Personal");

			//Проверка , что Assign task отсутствует на стр списка проектов
			Assert.IsFalse(ProjectPage.GetIsAssignRessponsibleBtnExist(), "Ошибка: Assign task отобраается на стр списка проектов для песронального аккаунта");
		}

		/// <summary>
		/// Удаление документа из проекта с помощью персонального аккаунта
		/// </summary>
		[Test]
		public void DeleteDocumentFromProjectPersAcc()
		{
			//Создать пустой проект		  
			CreateProject(ProjectUniqueName);

			//Добавление документа
			ImportDocumentProjectSettings(
				PathProvider.DocumentFile,
				ProjectUniqueName, 
				"Personal");

			// Выбрать документ
			SelectDocumentInProject(1);
			// Нажать удалить
			ProjectPage.ClickDeleteBtn();
			
			// Подтвердить
			ProjectPage.ConfirmClickYes();
			
			// Проверить, что документа нет
			Assert.IsFalse(
				ProjectPage.GetIsExistDocument(1),
				"Ошибка: документ не удалился");
		}

		/// <summary>
		/// Отмена создания проекта на первом шаге с помощью персонального аккаунта
		/// </summary>
		[Test]
		public void CancelFirstTestPersAcc()
		{
			// Нажать <Create>
			WorkspacePage.ClickCreateProject();
			// Ждем загрузки формы
			WorkspaceCreateProjectDialog.WaitDialogDisplay();

			// Нажать Отмену
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			// Проверить, что форма создания проекта закрылась
			Assert.IsTrue(WorkspaceCreateProjectDialog.WaitDialogDisappear(), "Ошибка: не закрылась форма создания проекта");
		}
	}
}
