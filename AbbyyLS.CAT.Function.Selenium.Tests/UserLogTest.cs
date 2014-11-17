using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки лога
	/// </summary>
	public class UserLogTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public UserLogTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Название проекта
		/// </summary>
		protected string projectName;
		protected StreamWriter logSW;
		protected string time;
		protected string timeFormat = "HH:mm:ss.fff";


		/// <summary>
		/// Метод выгрузки логов 
		/// </summary>		
		public void ExportLog()
		{
			//Выбрать документ
			SelectDocumentInProject();

			//Нажать кнопку выгрузки логов
			ProjectPage.ClickDownloadLogs();

			// Экспортировать и проверить, что файл сохранен
			ExternalDialogSaveDocument(
				subFolderName: "UserLogTests\\" + TestContext.CurrentContext.Test.Name,
				originalFileExtension: false, 
				fileExtension: ".zip",
				time: time);
		}

		/// <summary>
		/// Создание глоссария
		/// </summary>
		public void CreateGlossary(string glossaryName)
		{
			// Добавление словаря для глоссария
			var dictionary = new Dictionary<string, string>
			{ 
				{"first", "первый"},
				{"second", "второй"},
				{"one", "один"},
				{"last", "последний"}
			};

			// Создание глоссария с уникальным именем
			SwitchGlossaryTab();
			CreateGlossaryByName(glossaryName);

			// Создание глоссария на основании словаря
			SetGlossaryByDictinary(dictionary);
			// Создание проекта с файлом
			SwitchWorkspaceTab();
		}

		/// <summary>
		/// Метод проверки подстановки из САТ
		/// </summary>
		public void PasteFromCAT(int segmentNumber, EditorPageHelper.CAT_TYPE CatType, bool useHotkey)
		{
			string catType = "";

			switch (CatType)
			{
				case EditorPageHelper.CAT_TYPE.MT:
					catType = "MT";
					break;
				case EditorPageHelper.CAT_TYPE.TM:
					catType = "TM";
					break;
				case EditorPageHelper.CAT_TYPE.TB:
					catType = "TB";
					break;
				default:
					break;
			}

			//Выбираем сегмент
			EditorPage.ClickTargetCell(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");

			//Ждем пока загрузится CAT-панель
			Assert.IsTrue(EditorPage.GetCATPanelNotEmpty(), "Ошибка: панель CAT пуста");

			var TMNumber = EditorPage.GetCATTranslationRowNumber(CatType);
			Console.WriteLine("TMNumber: " + TMNumber);

			if (useHotkey)
			{
				//Нажать хоткей для подстановки из TM перевода сегмента
				EditorPage.PutCatMatchByHotkey(segmentNumber, TMNumber);
				// Пишем в лог
				WriteLog(segmentNumber, "Вставка из панели САТ (" + catType + ")", "Ctrl + " + TMNumber.ToString(), EditorPage.GetTargetText(segmentNumber));
			}
			else
			{
				// Двойной клик
				EditorPage.DoubleClickCATPanel(TMNumber);
				// Пишем в лог
				WriteLog(segmentNumber, "Вставка из панели САТ (" + catType + ")", "Двойной клик мыши", EditorPage.GetTargetText(segmentNumber));
			}

			// Дождаться автосохранения
			AutoSave();
		}

		public void WriteLog(int segment, string even, string evenClick, string text)
		{
			// Пишем в лог
			logSW.WriteLine(
				DateTime.Now.ToString(timeFormat) + " | {0} | {1} | {2} | {3}",
				segment, even.PadRight(50), evenClick.PadRight(30), text.PadRight(100));
		}

		/// <summary>
		/// Предварительная подготовка группы тестов
		/// </summary>
		[SetUp]
		public void Setup()
		{
			projectName = "TestLogProject" + DateTime.Now.Ticks;

			time = DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss.fff");

			var path = Path.Combine(
				PathTestResults, 
				"UserLogTests", 
				TestContext.CurrentContext.Test.Name, 
				time);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			logSW = new StreamWriter(new FileStream(path + "\\EventsLog.txt", FileMode.Create));

			// Не закрывать браузер
			quitDriverAfterTest = false;
			// Переходим к странице воркспейса
			GoToWorkspace();
		}

		/// <summary>
		/// Конечные действия для каждого теста
		/// </summary>
		[TearDown]
		public void TearDown()
		{
			// Закрытие файла
			logSW.Close();
		}



		/// <summary>
		/// Открытие-закрытие документа 
		/// </summary>
		[Test]
		public void OpenCloseDocument()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования удаления текста с клавиатуры в документе 
		/// </summary>
		[Test]
		public void DeleteText()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			const int segmentNum = 1;
			const string text = "Translation";

			// Написать текст в первом сегменте
			EditorPage.AddTextTarget(segmentNum, text);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);

			//Удалить текст путем нажатия клавиши Backspace
			while (EditorPage.GetTargetText(segmentNum).Length > 0)
			{
				EditorPage.SendKeysTarget(segmentNum, OpenQA.Selenium.Keys.Backspace);
			}

			// Пишем в лог
			WriteLog(segmentNum, "Удаление текста", "BackSpace", text);
			// Дождаться автосохранения
			AutoSave();
			//Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подтверждения перевода с помощью нажатия кнопки на панели инструментов 
		/// </summary>
		[Test]
		public void ConfirmTextButton()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;
			var text = "Translation";

			//Набрать текст в первом сегменте и нажать кнопку Confirm Segment
			AddTranslationAndConfirm(segmentNum, text);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);
			// Пишем лог Confirm
			WriteLog(segmentNum, "Нажатие кнопки Confirm", "Кнопка редактора", "-");
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования перемещения курсора между сегментами
		/// </summary>
		[Test]
		public void ChooseSegment()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			var segmentNum = 1;
			//Курсор в первом сегменте Source
			EditorPage.ClickSourceCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			segmentNum = 2;
			//Курсор во втором сегменте Source
			EditorPage.ClickSourceCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			//Курсор во втором сегменте Target
			EditorPage.ClickTargetCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			segmentNum = 4;
			//Курсор в четвертом сегменте Target
			EditorPage.ClickTargetCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			segmentNum = 7;
			//Курсор в седьмом сегменте Source
			EditorPage.ClickSourceCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			//Курсор в последнем сегменте Target
			segmentNum = EditorPage.GetSegmentsNumber();
			EditorPage.ClickTargetCell(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования копирования текста из source в target по нажатию кнопки в панели инструментов
		/// </summary>
		[Test]
		public void CopySourceSegmentButton()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			var segmentNum = 1;
			//Копировать текст сегмента
			ToTargetButton(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(
				segmentNum,
				"Нажатие кнопки копирования source в target", 
				"Кнопка редактора", 
				EditorPage.GetSourceText(segmentNum));
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования копирования текста из source в target по хоткею
		/// </summary>
		[Test]
		public void CopySourceSegmentHotkey()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;

			//Копировать текст сегмента
			ToTargetHotkey();
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(
				segmentNum, 
				"Нажатие хоткея копирования source в target",
				"Ctrl + Insert",
				EditorPage.GetSourceText(segmentNum));
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования Undo/Redo
		/// </summary>
		[Test]
		public void UndoRedoActions()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			int segmentNumber = 1;
			// Копирование первого сегмента
			ToTargetButton(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки копирования source в target", "Кнопка редактора", EditorPage.GetSourceText(segmentNumber));

			segmentNumber = 2;
			// Копирование второго сегмента
			ToTargetButton(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки копирования source в target", "Кнопка редактора", EditorPage.GetSourceText(segmentNumber));

			// Кликнуть Undo
			EditorPage.ClickUndoBtn();
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки Undo", "Кнопка редактора", "-");

			string targetText = EditorPage.GetTargetText(segmentNumber);
			Assert.IsTrue(targetText == "", "Ошибка: после undo текст в таргете не удалился\n" + targetText);

			// Кликнуть Redo
			EditorPage.ClickRedoBtn();
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки Redo", "Кнопка редактора", "-");

			Thread.Sleep(500);
			Assert.AreEqual(EditorPage.GetSourceText(segmentNumber), EditorPage.GetTargetText(segmentNumber), "Ошибка: после redo текст в target не восстановился");

			// Написать текст в третьем сегменте
			Console.WriteLine("Написать текст в третьем сегменте");
			segmentNumber = 3;
			string text3Segment = "Test for 3d segment";
			EditorPage.AddTextTarget(segmentNumber, text3Segment);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNumber, "Добавление текста", "Клавиатура", text3Segment);

			// Написать текст в 4 сегменте
			Console.WriteLine("Написать текст в 4 сегменте");
			segmentNumber = 4;
			string text4Segment = "Test for 4th segment";
			EditorPage.AddTextTarget(segmentNumber, text4Segment);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNumber, "Добавление текста", "Клавиатура", text4Segment);

			// Нажать хоткей отмены
			Console.WriteLine("Нажать хоткей отмены");
			EditorPage.UndoByHotkey(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие хоткея Undo", "Ctrl + Z", "-");
			// Проверить, что 4 сегмент пуст
			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber) == "", "Ошибка: после хоткея отмены текст в 4 сегменте не удалился");

			// Нажать хоткей отмены
			Console.WriteLine("Нажать хоткей отмены");
			segmentNumber = 3;
			EditorPage.UndoByHotkey(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие хоткея Undo", "Ctrl + Z", "-");
			// Проверить, что 3 сегмент пуст
			Assert.IsTrue(EditorPage.GetTargetText(segmentNumber) == "", "Ошибка: после хоткея отмены текст в 3 сегменте не удалился");

			// Нажать хоткей восстановления
			Console.WriteLine("Нажать хоткей восстановления");
			EditorPage.RedoByHotkey(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие хоткея Redo", "Ctrl + Y", "-");

			segmentNumber = 5;
			EditorPage.ClickTargetCell(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");

			// Проверить, что 3 сегмент не пуст
			Assert.AreEqual(
				text3Segment, 
				EditorPage.GetTargetText(3),
				"Ошибка: после хоткея возврата текст в 3 сегменте не появился");
			
			// Дождаться автосохранения
			AutoSave();

			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования кнопки перемещения курсора между полями source и target
		/// </summary>
		[Test]
		public void SourceTargetSegmentsSwitchButton()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				withMT: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			var segmentNum = 1;
			//Переключить курсор между полями source и target
			SourceTargetSwitchButton(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог Toggle
			WriteLog(segmentNum, "Нажатие кнопки Toggle", "Кнопка редактора", "-");
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования кнопки перемещения курсора между полями source и target по хоткею
		/// </summary>
		[Test]
		public void SourceTargetSegmentsSwitchHotkey()
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withTM: false,
				withMT: false,
				uploadDocument: DocumentFileToConfirm);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			var segmentNum = 1;
			//Переключить курсор между полями source и target
			SourceTargetSwitchHotkey(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог Toggle
			WriteLog(segmentNum, "Нажатие хоткея Toggle", "Tab", "-");
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из MT по хоткею
		/// </summary>
		[Test]
		public void SubstituteTranslationMTHotkey()
		{
			// Открыть документ
			CreateReadyProject(projectName, withMT: true);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			// Проверяем подстановку САТ 1-го сегмента
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: true);
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из MT по клику на сегмент в САТ-панели
		/// </summary>
		[Test]
		[Ignore("Temporarily not working")]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Bing)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Google)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Moses)]
		[TestCase(Workspace_CreateProjectDialogHelper.MT_TYPE.Yandex)]
		public void SubstituteTranslationMTDoubleClick(Workspace_CreateProjectDialogHelper.MT_TYPE mtType)
		{
			// Открыть документ
			CreateReadyProject(
				projectName,
				withMT:true,
				mtType: mtType);

			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			// Проверяем подстановку САТ 1-го сегмента
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: false);

			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
		/// </summary>
		[Test]
		public void SubstituteTranslationTMHotkey()
		{
			// Открыть документ
			CreateReadyProject(projectName);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			// Проверяем подстановку САТ 1-го сегмента
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: true);

			// Проверяем подстановку САТ 2-го сегмента
			PasteFromCAT(
				segmentNumber: 2,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: true);

			// Проверяем подстановку САТ 4-го сегмента
			PasteFromCAT(
				segmentNumber: 4,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: true);

			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели
		/// </summary>
		[Test]
		public void SubstituteTranslationTMDoubleClick()
		{
			// Открыть документ
			CreateReadyProject(projectName, true);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");
			// Проверяем подстановку САТ 1-го сегмента
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: false);
			// Проверяем подстановку САТ 2-го сегмента
			PasteFromCAT(
				segmentNumber: 2,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: false);
			// Проверяем подстановку САТ 4-го сегмента
			PasteFromCAT(
				segmentNumber: 4,
				CatType: EditorPageHelper.CAT_TYPE.MT,
				useHotkey: false);
			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования выгрузки пустого лога
		/// </summary>
		[Test]
		public void EmptyLogTest()
		{
			// Создание проекта
			CreateProject(projectName, DocumentFile);
			// Открыть проект
			OpenProjectPage(projectName);

			// Дождаться пропадания колеса ожидания
			Assert.IsTrue(
				ProjectPage.WaitDocumentDownloadFinish(),
				"Ошибка: колесо ожидания долго не пропадает");

			// Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования выбора варианта написания слова (spellcheck)
		/// </summary>
		[Test]
		public void Spellcheck()
		{
			// Открыть документ
			CreateReadyProject(projectName, withTM: false);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;
			var text = "Плонета";

			// Написать текст в первом сегменте
			EditorPage.AddTextTarget(segmentNum, text);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);
			// Открыть контекстное меню и выбрать первое слово замены
			text = EditorPage.RightClickSpellcheck(segmentNum, 1);
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Вставка из словаря (контекстное меню)", "Клик мыши", text);
			// Дождаться автосохранения
			AutoSave();
			//Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования перемещения по source
		/// </summary>
		[Test]
		public void MovingInSource()
		{
			// Открыть документ
			CreateReadyProject(projectName, withTM: false);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;

			// Всячески перемещаемся в source
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.End);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки End", "End", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.Home);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки Home", "Home", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.ArrowRight);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки вправо", "Arrow Right", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.ArrowLeft);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки влево", "Arrow Left", "-");
			segmentNum = 3;
			// Всячески перемещаемся в source
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.End);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки End", "End", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.Home);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки Home", "Home", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.ArrowRight);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки вправо", "Arrow Right", "-");
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.ArrowLeft);
			// Пишем в лог
			WriteLog(segmentNum, "Нажатие кнопки влево", "Arrow Left", "-");
			// Дождаться автосохранения
			AutoSave();
			//Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");
			// Перейти к проекту
			OpenProjectPage(projectName);
			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования перемещения по target
		/// </summary>
		[Test]
		public void MovingInTarget()
		{
			// Открыть документ
			CreateReadyProject(projectName, withTM: false);
			
			WriteLog(0, "Открытие документа", "-", "-");

			int segmentNumber = 1;

			//Копировать текст сегмента
			ToTargetButton(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки копирования source в target", "Кнопка редактора", EditorPage.GetTargetText(segmentNumber));

			segmentNumber = 3;
			ToTargetButton(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в source", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки копирования source в target", "Кнопка редактора", EditorPage.GetTargetText(segmentNumber));

			segmentNumber = 1;
			// Всячески перемещаемся в target
			EditorPage.EndHotkey(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки End", "End", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Home);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки Home", "Home", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.ArrowRight);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки вправо", "Arrow Right", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.ArrowLeft);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки влево", "Arrow Left", "-");

			segmentNumber = 3;
			// Всячески перемещаемся в target
			EditorPage.EndHotkey(segmentNumber);
			// Пишем в лог
			WriteLog(segmentNumber, "Переход в target", "Клик мыши", "-");
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки End", "End", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.Home);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки Home", "Home", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.ArrowRight);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки вправо", "Arrow Right", "-");

			EditorPage.SendKeysTarget(segmentNumber, OpenQA.Selenium.Keys.ArrowLeft);
			// Пишем в лог
			WriteLog(segmentNumber, "Нажатие кнопки влево", "Arrow Left", "-");

			// Дождаться автосохранения
			AutoSave();

			//Нажать кнопку назад
			EditorClickHomeBtn();
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования выделения source и target
		/// </summary>
		[Test]
		public void HighlightText()
		{
			// Открыть документ
			CreateReadyProject(projectName, false);
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 2;

			//Копировать текст сегмента
			ToTargetButton(segmentNum);
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			WriteLog(segmentNum, "Нажатие кнопки копирования source в target", "Кнопка редактора", EditorPage.GetTargetText(segmentNum));

			segmentNum = 1;

			// Нажать хоткей выделения source
			EditorPage.SendKeysSource(segmentNum, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.End);
			WriteLog(segmentNum, "Переход в source", "Клик мыши", "-");
			WriteLog(
				segmentNum,
				"Нажатие хоткея выделения всего текста",
				"Shift + Ctrl + End", 
				EditorPage.GetSourceText(segmentNum));
			Thread.Sleep(1000);

			segmentNum = 2;
			// Нажать хоткей выделения target
			EditorPage.SendKeysTarget(segmentNum, OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.End);
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			WriteLog(
				segmentNum,
				"Нажатие хоткея выделения всего текста", 
				"Shift + Ctrl + End", 
				EditorPage.GetTargetText(segmentNum));
			Thread.Sleep(1000);

			// Дождаться автосохранения
			AutoSave();

			//Нажать кнопку назад
			EditorClickHomeBtn();
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования добавления перевода
		/// </summary>
		[Test]
		public void AddTranslation()
		{
			// Открыть документ
			CreateReadyProject(projectName, false);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;
			var text = "Первый";

			// Добавить перевод в первый target
			EditorPage.AddTextTarget(segmentNum, text);
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);

			segmentNum = 2;
			text = "Второй";
			// Добавить перевод во второй target
			EditorPage.AddTextTarget(segmentNum, text);
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);

			segmentNum = EditorPage.GetSegmentsNumber();
			text = "Последний";
			// Добавить перевод в последний target
			EditorPage.AddTextTarget(segmentNum, text);
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);

			// Дождаться автосохранения
			AutoSave();

			//Нажать кнопку назад
			EditorClickHomeBtn();
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из MT по клику на сегмент в САТ-панели вместо существующего
		/// </summary>
		[Test]
		public void SubstituteTranslationTextTmMt()
		{
			// Открыть документ
			CreateReadyProject(projectName, withMT: true);
			
			WriteLog(0, "Открытие документа", "-", "-");

			const int segmentNum = 1;
			const string text = "Translation";

			//Набрать текст в первом сегменте и нажать кнопку Confirm Segment
			AddTranslationAndConfirm();
			
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);
			// Пишем лог Confirm
			WriteLog(segmentNum, "Нажатие кнопки Confirm", "Кнопка редактора", "-");

			// Дождаться автосохранения
			AutoSave();
			
			// Проверяем подстановку TМ из САТ в 1-ый сегмент
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.TM, 
				useHotkey: false);

			// Проверяем подстановку MT из САТ в 1-ый сегмент
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);

			// Нажать кнопку назад
			EditorClickHomeBtn();
			
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из TM по клику на сегмент в САТ-панели вместо существующего
		/// </summary>
		[Test]
		public void SubstituteTranslationTextMtTm()
		{
			// Открыть документ
			CreateReadyProject(projectName, withMT: true);
			// Пишем в лог
			WriteLog(0, "Открытие документа", "-", "-");

			var segmentNum = 1;
			var text = "Translation";

			//Набрать текст в первом сегменте и нажать кнопку Confirm Segment
			AddTranslationAndConfirm(segmentNum, text);
			
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);
			// Пишем лог Confirm
			WriteLog(segmentNum, "Нажатие кнопки Confirm", "Кнопка редактора", "-");
			// Дождаться автосохранения
			AutoSave();

			// Проверяем подстановку MT из САТ в 1-ый сегмент
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);
			// Проверяем подстановку TМ из САТ в 1-ый сегмент
			PasteFromCAT(
				segmentNumber: 1,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);

			// Нажать кнопку назад
			EditorClickHomeBtn();
			
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}

		/// <summary>
		/// Метод тестирования подстановки перевода сегмента из TB по клику на сегмент в САТ-панели
		/// </summary>
		[Test]
		public void SubstituteTranslationTB()
		{
			var uniqueGlossaryName = GlossaryName + DateTime.Now;

			// Создание глоссария
			CreateGlossary(uniqueGlossaryName);

			// Открыть документ
			CreateReadyProject(
				projectName,
				withMT: true,
				mtType: Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT,
				chooseGlossary: true,
				glossaryName: uniqueGlossaryName);
			WriteLog(0, "Открытие документа", "-", "-");
			// Проверяем подстановку TB из САТ в 1-ый сегмент
			PasteFromCAT(1, EditorPageHelper.CAT_TYPE.TB, false);

			const int segmentNum = 2;
			const string text = "Translation";

			//Набрать текст во втором сегменте и нажать кнопку Confirm Segment
			AddTranslationAndConfirm(segmentNum);
			// Пишем в лог
			WriteLog(segmentNum, "Переход в target", "Клик мыши", "-");
			// Пишем лог ввод текста
			WriteLog(segmentNum, "Добавление текста", "Клавиатура", text);
			// Пишем лог Confirm
			WriteLog(segmentNum, "Нажатие кнопки Confirm", "Кнопка редактора", "-");
			// Дождаться автосохранения
			AutoSave();
			// Проверяем подстановку TB из САТ в 2-ой сегмент
			PasteFromCAT(
				segmentNumber: 2,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);

			// Проверяем подстановку MT из САТ в 3-ий сегмент
			PasteFromCAT(
				segmentNumber: 3,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);

			// Проверяем подстановку TB из САТ в 3-ый сегмент
			PasteFromCAT(
				segmentNumber: 3,
				CatType: EditorPageHelper.CAT_TYPE.TB,
				useHotkey: false);

			// Проверяем подстановку TМ из САТ в 4-ый сегмент
			PasteFromCAT(
				segmentNumber: 4,
				CatType: EditorPageHelper.CAT_TYPE.TM,
				useHotkey: false);

			// Проверяем подстановку TB из САТ в 4-ый сегмент
			PasteFromCAT(
				segmentNumber: 4,
				CatType: EditorPageHelper.CAT_TYPE.TB,
				useHotkey: false);

			// Нажать кнопку назад
			EditorClickHomeBtn();
			// Пишем в лог
			WriteLog(0, "Нажатие кнопки Home", "Кнопка редактора", "-");

			// Перейти к проекту
			OpenProjectPage(projectName);

			//Выгрузить логи
			ExportLog();
		}


		
	}
}
