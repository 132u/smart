using System;
using NUnit.Framework;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <remarks>
	/// Методы для тестирования Проектов
	/// </remarks>

	public class NewProjectTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>


		/// <param name="browserName">Название браузера</param>
		public NewProjectTest(string browserName)
			: base(browserName)
		{
			
		}

		public string ProjectNameCheck;
		public string DuplicateProjectName;

		// Path документа для экспорта (для определения названия документа в сообщении об экспорте)

		// В сообщении об экспорте при экспорте нескольких документов должно быть Documents
		protected const string EXPORT_NOTIFIER_DOWNLOAD_DOCUMENTS = "Documents";

		/// <summary>
		/// Старт тестов, переменные
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			// Не закрывать браузер
			QuitDriverAfterTest = false;

		}

		/// <summary>
		/// Создать проект, добавить документ
		/// </summary>
		/// <param name="filePath"></param>
		protected void CreateProjectImportDocument(string filePath)
		{
			//Создать пустой проект		  
			CreateProject(ProjectName);

			//Добавление документа
			ImportDocumentProjectSettings(filePath, ProjectName);
		}

		/// <summary>
		/// Проверить, что проект есть в списке
		/// </summary>
		/// <param name="projectName">Имя проекта, которое ищем в списке проектов</param>
		protected void CheckProjectInList(string projectName)
		{
			//проверка, что проект с именем projectName есть на странице
			Assert.IsTrue(
				GetIsExistProject(projectName), 
				"Ошибка: проекта " + projectName + " нет в списке");
		}

		/// <summary>
		/// Проверить, что проекта нет (проверка без лишнего ожидания)
		/// </summary>
		/// <param name="projectNameCheck">имя проекта</param>
		/// <returns>нет в списке</returns>
		protected bool GetIsNotExistProject(string projectNameCheck)
		{
			setDriverTimeoutMinimum();
			// Проекта нет?
			var isNotExist = !GetIsExistProject(projectNameCheck);
			setDriverTimeoutDefault();

			return isNotExist;
		}

		/// <summary>
		/// Проверка, есть ли ошибка существующего имени
		/// </summary>
		/// <param name="shouldErrorExist">условие для результата проверки - должна ли быть ошибка</param>
		protected void AssertErrorDuplicateName(bool shouldErrorExist = true)
		{
			if (!shouldErrorExist)
			{
				// Т.к. ожидаем, что ошибка не появится, опускаем таймаут
				setDriverTimeoutMinimum();
			}

			// Проверить, что поле Имя отмечено ошибкой
			bool isExistErrorInput = WorkspaceCreateProjectDialog.GetIsNameInputError();
			// Проверить, что есть сообщение, что имя существует
			bool isExistErrorMessage = WorkspaceCreateProjectDialog.GetIsExistErrorMessageNameExists();

			string errorMessage = "\n";
			bool isError = false;
			// Ошибка должна появиться
			if (shouldErrorExist)
			{
				if (!isExistErrorInput)
				{
					isError = true;
					errorMessage += "Ошибка: поле Название не отмечено ошибкой\n";
				}
				if (!isExistErrorMessage)
				{
					isError = true;
					errorMessage += "Ошибка: не появилось сообщение о существующем имени";
				}
			}
			// Ошибка НЕ должна появиться
			else
			{
				if (isExistErrorInput)
				{
					isError = true;
					errorMessage += "Ошибка: поле Название отмечено ошибкой\n";
				}
				if (isExistErrorMessage)
				{
					isError = true;
					errorMessage += "Ошибка: появилось сообщение о существующем имени";
				}
			}

			// Проверить условие
			Assert.IsFalse(isError, errorMessage);

			if (!shouldErrorExist)
			{
				setDriverTimeoutDefault();
			}
		}
	}
}
