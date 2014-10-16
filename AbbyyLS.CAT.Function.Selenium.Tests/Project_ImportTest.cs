using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

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

		static string _filesForImportCorrectPath = Path.Combine(@"..\TestingFiles\", "FilesForImportCorrect");
		static string _filesForConfirmPath = Path.Combine(@"..\TestingFiles\", "FilesForConfirm");
		static string _filesForImportErrorPath = Path.Combine(@"..\TestingFiles\", "FilesForImportError");

		static string[] filesForImportCorrect = Directory.GetFiles(_filesForImportCorrectPath);
		static string[] filesForConfirm = Directory.GetFiles(_filesForConfirmPath);
		static string[] filesForImportError = Directory.GetFiles(_filesForImportErrorPath);

		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
		public void Setup()
		{
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
			ImportDocumentCreateProject(AudioFile);
			Thread.Sleep(1000);

			// Проверить, что появилось сообщение о неверном формате загружаемого документа
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
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
			ImportDocumentCreateProject(DocumentFile);
			// Загрузить второй документ
			ImportDocumentCreateProject(_ttxFile);

			// Проверить, что не появилось сообщение о неверном формате загружаемого документа
			Assert.IsFalse(WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
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
			Assert.IsFalse(WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
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
			Assert.IsFalse(WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
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
			Assert.IsFalse(WorkspaceCreateProjectDialog.GetIsExistErrorFormatDocumentMessage(),
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
			ImportDocumentProjectSettings(DocumentFile, ProjectName);

			// Проверить, что в проекте есть документ
			Assert.IsTrue(ProjectPage.GetIsExistDocument(1),
				"Ошибка: на странице проекта нет документа");
		}

		/// <summary>
		/// метод для тестирования импорта разбираемых на сегменты файлов из заданной папки в существующий проект
		/// </summary>
		/// <param name="filePath">путь в файлу, импортируемого в проект</param>
		[Test]
		[TestCaseSource("filesForImportCorrect")]
		public void ImportFilesAfterCreationCorrectTest(string filePath)
		{
			// Создать проект, загрузить документ, проверить сегменты
			CreateReadyProject(ProjectName, false, false, filePath);

			/* TODO проверить
			CreateProjectImportDocument(filePath);
			//Назначение задачи на пользователя
			AssignTask();

			// Зайти в редактор документа
			Driver.FindElement(By.XPath(".//a[contains(@class,'js-editor-link')]")).Click();

			// Дождаться загрузки страницы
			Wait.Until((d) => d.Title.Contains("Editor"));

			// Проверить, существует ли хотя бы один сегмент
			Assert.IsTrue(IsElementPresent(By.CssSelector(
				"#segments-body div table tr:nth-child(1)"
				)));*/
		}

		///// <summary>
		///// метод для тестирования импорта не разбираемых на сегменты файлов из заданной папки в существующий проект
		///// </summary>
		///// <param name="filePath">путь в файлу, импортируемого в проект</param>
		//[Test]
		//[TestCaseSource("filesForImportError")]
		//public void ImportFilesAfterCreationErrorTest(string filePath)
		//{
		//	// Создать проект, загрузить документ, проверить сегменты
		//	CreateReadyProject(ProjectName, false, false, filePath);

		//	// TODO проверить - почему такая проверка в конце
		//	////Создать пустой проект
		//	//CreateProject(ProjectName);

		//	////Добавление документа
		//	//ImportDocumentProjectSettings(filePath, ProjectName);

		//	////Назначение задачи на пользователя
		//	//AssignTask();

		//	//// Строчка нужного проекта
		//	//Driver.FindElement(By.LinkText(ProjectName)).Click();
		//	//// Зайти в редактор документа
		//	//Driver.FindElement(By.XPath(".//a[contains(@class,'js-editor-link')]")).Click();

		//	//// Дождаться загрузки страницы
		//	//Wait.Until((d) => d.Title.Contains("Editor"));

		//	//// Проверить, существует ли хотя бы один сегмент
		//	//Assert.IsTrue(IsElementPresent(By.CssSelector(
		//	//	"#segments-body div table tr:nth-child(1)"
		//	//	)));
		//}

		/// <summary>
		/// Загрузка в проект документа, который уже был загружен
		/// </summary>
		[Test]
		public void ImportDuplicateDocumentTest()
		{
			// Создать проект, загрузить документ
			CreateProjectImportDocument(DocumentFile);
			// Кликнуть по Импорт
			ProjectPage.ClickImportBtn();

			//ждем когда загрузится окно для загрузки документа 
			ProjectPage.WaitImportDialogDisplay();
			//Процесс добавления файла

			// Нажать Add
			ProjectPage.ClickAddDocumentInImport();
			// Заполнить диалог загрузки
			FillAddDocumentForm(DocumentFile);

			// Проверить появление оповещения об ошибке
			Assert.IsTrue(ProjectPage.WaitImportDocumentErrorMessage(),
				"Ошибка: не появилось сообщение о повторном файле");
		}



		/// <summary>
		/// Загрузить документ в форме создания проекта
		/// </summary>
		/// <param name="documentName">имя документа</param>
		protected void ImportDocumentCreateProject(string documentName)
		{
			//процесс добавления файла
			WorkspaceCreateProjectDialog.ClickAddDocumentBtn();
			FillAddDocumentForm(documentName);
		}
	}
}