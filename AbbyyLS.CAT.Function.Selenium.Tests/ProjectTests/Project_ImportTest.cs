using System.Threading;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки импорта проекта
	/// </summary>
	public class Project_ImportTest : NewProjectTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public Project_ImportTest(string browserName)
			: base(browserName)
		{
		}

		/// <summary>
		/// Старт тестов
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			// Переходим к странице воркспейса
			GoToWorkspace();
		}

		/// <summary>
		/// метод тестирования загрузки rtf формата (неподдерживаемый формат)
		/// </summary>
		[Test]
		public void ImportWrongFileTest()
		{
			//1 шаг - заполнение данных о проекте
			FirstStepProjectWizard(ProjectName);

			//процесс добавления файла
			ImportDocumentCreateProject(TestFile.AudioFile);
			Thread.Sleep(1000);

			// Проверить, что появилось сообщение о неверном формате загружаемого документа
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
				"Ошибка: не появилось сообщение о неверном формате загружаемого документа");
		}

		/// <summary>
		/// метод тестирования загрузки нескольких файлов при создании проекта (docx+ttx)
		/// </summary>
		[Test]
		public void ImportSomeFilesTest()
		{
			//1 шаг - заполнение данных о проекте
			FirstStepProjectWizard(ProjectName);
			// Загрузить документ
			ImportDocumentCreateProject(TestFile.DocumentFile);
			// Загрузить второй документ
			ImportDocumentCreateProject(_ttxFile);

			// Проверить, что не появилось сообщение о неверном формате загружаемого документа
			Assert.IsFalse(
				WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
				"Ошибка: появилось сообщение о неверном формате загружаемого документа");
		}

		/// <summary>
		/// Импорт документа формата ttx (допустимый формат)
		/// </summary>
		[Test]
		public void ImportTtxFileTest()
		{
			//1 шаг - заполнение данных о проекте
			FirstStepProjectWizard(ProjectName);
			// Загрузить документ
			ImportDocumentCreateProject(_ttxFile);

			// Проверить, что не появилось сообщение о неверном формате загружаемого документа
			Assert.IsFalse(
				WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
				"Ошибка: появилось сообщение о неверном формате загружаемого документа");
		}

		/// <summary>
		/// Импорт документа формата txt (допустимый формат)
		/// </summary>
		[Test]
		public void ImportTxtFileTest()
		{
			//1 шаг - заполнение данных о проекте
			FirstStepProjectWizard(ProjectName);
			// Загрузить документ
			ImportDocumentCreateProject(_txtFile);

			// Проверить, что не появилось сообщение о неверном формате загружаемого документа
			Assert.IsFalse(
				WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
				"Ошибка: появилось сообщение о неверном формате загружаемого документа");
		}

		/// <summary>
		/// Импорт документа формата Srt (допустимый формат)
		/// </summary>
		[Test]
		public void ImportSrtFileTest()
		{
			//1 шаг - заполнение данных о проекте
			FirstStepProjectWizard(ProjectName);
			// Загрузить документ
			ImportDocumentCreateProject(_srtFile);

			// Проверить, что не появилось сообщение о неверном формате загружаемого документа
			Assert.IsFalse(
				WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
				"Ошибка: появилось сообщение о неверном формате загружаемого документа");
		}

		/// <summary>
		/// Импорт документа в созданный проект без файла
		/// </summary>
		[Test]
		public void ImportDocumentAfterCreationTest()
		{
			// Создать проект
			CreateProject(ProjectName);
			// загрузить документ			
			ImportDocumentProjectSettings(TestFile.DocumentFile, ProjectName);

			// Проверить, что в проекте есть документ
			Assert.IsTrue(
				ProjectPage.GetIsExistDocument(1),
				"Ошибка: на странице проекта нет документа");
		}
		
		/// <summary>
		/// Загрузка в проект документа, который уже был загружен
		/// </summary>
		[Test]
		public void ImportDuplicateDocumentTest()
		{
			// Создать проект, загрузить документ
			CreateProjectImportDocument(TestFile.DocumentFile);
			// Кликнуть по Импорт
			ProjectPage.ClickImportBtn();
			//ждем когда загрузится окно для загрузки документа 
			ProjectPage.WaitImportDialogDisplay();

			// Заполнить диалог загрузки
			ProjectPage.UploadFileOnProjectPage(TestFile.DocumentFile);

			// Проверить появление оповещения об ошибке
			Assert.IsTrue(
				ProjectPage.WaitImportDocumentErrorMessage(),
				"Ошибка: не появилось сообщение о повторном файле");
		}

		/// <summary>
		/// Загрузить документ в форме создания проекта
		/// </summary>
		/// <param name="documentName">имя документа</param>
		protected void ImportDocumentCreateProject(string documentName)
		{
			WorkspaceCreateProjectDialog.UploadFileToNewProject(documentName);
		}
	}
}