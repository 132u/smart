using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using OpenQA.Selenium.Interactions;

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

		protected string ResultFilePath;

		public string ProjectNameCheck;
		public string DuplicateProjectName;

		public string _documentFileWrong;
		public string _ttxFile;
		public string _txtFile;
		public string _srtFile;

		public string _xliffTC10;

		//protected string _filesForImportCorrectPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportCorrect");
		//protected string _filesForConfirmPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForConfirm");
		//protected string _filesForImportErrorPath = Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportError");

		//protected static string[] filesForImportCorrect = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportCorrect"));
		//protected static string[] filesForConfirm = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForConfirm"));
		//protected static string[] filesForImportError = Directory.GetFiles(Path.GetFullPath(@"..\..\..\TestingFiles\FilesForImportError"));

		// Path документа для экспорта (для определения названия документа в сообщении об экспорте)
		protected string _exportFilePath;
		// В сообщении об экспорте при экспорте нескольких документов должно быть Documents
		protected const string EXPORT_NOTIFIER_DOWNLOAD_DOCUMENTS = "Documents";

		/// <summary>
		/// Старт тестов, переменные
		/// </summary>
		[SetUp]
		public void SetupTest()
		{
			_documentFileWrong = Path.Combine(PathTestFiles, "doc98.doc");
			_ttxFile = Path.Combine(PathTestFiles, "test.ttx");
			_txtFile = Path.Combine(PathTestFiles, "test.txt");
			_srtFile = Path.Combine(PathTestFiles, "test.srt");

			_xliffTC10 = Path.Combine(PathTestFiles, "TC-10En.xliff");

			ResultFilePath = Path.Combine(PathTestFiles, "Result");
			_exportFilePath = DocumentFileToConfirm;

			// Не закрывать браузер
			quitDriverAfterTest = false;

			// Переходим к странице воркспейса
			GoToWorkspace();
		}

		///////////////////////////////
		// Что это?

		/// <summary>
		/// Метод создания файла для записи результатов тестирования
		/// </summary>
		public void CreateResultFile()
		{
			FileInfo fi = new FileInfo(ResultFilePath);
			StreamWriter sw = fi.CreateText();
			sw.WriteLine("Test Results");
			sw.Close();

		}

		/// <summary>
		/// Метод для записи результатов тестирования в файл и в консоль
		/// </summary>
		/// <param name="s">Строка, записываемая в файл</param>
		public void WriteStringIntoFile(string s)
		{
			StreamWriter sw = new StreamWriter(ResultFilePath, true);
			sw.WriteLine(s);
			sw.Close();
		}

		/// <summary>
		/// Вывод в консоль failed тестов красным цветом
		/// </summary>
		/// <param name="s">Строка, выводимая в консоль </param>
		public void FailConsoleWrite(string s)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(s);
			Console.ResetColor();
		}

		/// <summary>
		/// Вывод в консоль успешно пройденных тестов зеленым цветом
		/// </summary>
		/// <param name="s">Строка, выводимая в консоль</param>
		public void PassConsoleWrite(string s)
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine(s);
			Console.ResetColor();
		}

		/// <summary>
		/// Вывод результатов в консоль и в файл 
		/// </summary>
		/// <param name="s">Строка, выводимая в консоль и добавляемая в файл</param>
		/// <param name="p">Параметр, отвечающий за цвет: 0 - fail(red), 1 - pass(green), 2 - black</param>
		public void WriteFileConsoleResults(string s, int p)
		{
			StreamWriter sw = new StreamWriter(ResultFilePath, true);

			switch (p)
			{
				//проваленные тесты
				case 0:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(s);
					Console.ResetColor();
					sw.WriteLine(s);
					sw.WriteLine("\n");
					sw.Close();
					break;
				//успешно пройденные тесты
				case 1:
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine(s);
					Console.ResetColor();
					sw.WriteLine(s);
					sw.WriteLine("\n");
					sw.Close();
					break;
				case 2:
					Console.WriteLine(s);
					sw.WriteLine(s);
					sw.WriteLine("\n");
					sw.Close();
					break;
				default:
					break;
			}
		}

		///////////////////////////////




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
			Assert.IsTrue(GetIsExistProject(projectName), "Ошибка: проекта " + projectName + " нет в списке");
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
			bool isNotExist = !GetIsExistProject(projectNameCheck);

			setDriverTimeoutDefault();
			return isNotExist;
		}
	}
}
