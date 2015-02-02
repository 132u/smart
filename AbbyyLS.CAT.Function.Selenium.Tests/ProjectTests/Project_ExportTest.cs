using System;
using System.Threading;
using NUnit.Framework;
using System.IO;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки экспорта проекта
	/// </summary>
	public class Project_ExportTest : NewProjectTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public Project_ExportTest(string browserName)
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
		/// тип экспорта
		/// </summary>
		const string EXPORT_TYPE_SOURCE = "Original";
		const string EXPORT_TYPE_TMX = "TMX";
		const string EXPORT_TYPE_TARGET = "Translation";

		/// <summary>
		/// тип страницы для просмотра нотификатора
		/// </summary>
		const string PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST = "updateProjectList";
		const string PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE = "updateProjectPage";
		const string PLACE_SEARCH_NOTIFIER_OPEN_PROJECT = "openProject";
		const string PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST = "openProjectList";
		const string PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY = "openGlossary";

		/// <summary>
		/// максимальное количество попыток возобновить экспорт
		/// </summary>
		protected const int MAX_NUMBER_RESTART_EXPORT = 1;

		/// <summary>
		/// ТЕСТ: экспорт документа из проекта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportDocumentFromProjectTest(string exportType)
		{			
			// Создать проект с документом (если его нет)
			CreateProjectOneDocument(true);
			
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Нажать галочку у документа
			SelectDocumentInProject();
			// Нажать "красный" экспорт
			ClickExportBtnRed(exportType);
			// Экспортировать документ
			WorkWithExport(exportType);
		}

		/// <summary>
		/// ТЕСТ: экспорт проекта из списка проектов по "красной кнопке"
		/// </summary>
		/// <param name="exportType">тип экспорт</param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportProjectFromProjectListTest(string exportType)
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Выбрать этот проект
			SelectProjectInList(projectName);
			// Нажать "красный" экспорт
			ClickExportBtnRed(exportType);
			// Экспортировать проект
			WorkWithExport(exportType);
		}

		/// <summary>
		/// ТЕСТ: экспорт проекта из списка проектов из свертки проекта
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportProjectFromProjectListProjectSettingsTest(string exportType)
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Нажать на Экспорт в свертке проекта
			ClickExportBtnProjectSettings(exportType);
			// Экспортировать проект
			WorkWithExport(exportType);
		}

		/// <summary>
		/// ТЕСТ: экспорт документа из свертки документа в списке проектов
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportDocumentFromProjectListDocumentSettingsTest(string exportType)
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Открыть информацию о документе (свертку документа)
			ClickDocumentOpenInfo();
			// Нажать Экспорт в свертке документа
			ClickExportBtnDocumentSettings(exportType);
			// Экспортировать документ
			WorkWithExport(exportType);
		}

		/// <summary>
		/// ТЕСТ: экспорт нескольких документов (архив)
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportMultiDocumentsTest(string exportType)
		{
			// Создать проект с документами (если его нет)
			var projectName = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Выделить проект
			SelectProjectInList(projectName);
			// Нажать на "красный" экспорт
			ClickExportBtnRed(exportType);
			// Экспортировать документ
			WorkWithExport(exportType, true);
		}

		/// <summary>
		/// ТЕСТ: закрытие сообщения об экспорте (Отмена в сообщении Download)
		/// </summary>
		[Test]
		public void ExportCloseNotifier()
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Нажать экспорт проекта "Original"
			ClickExportBtnProjectSettings(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();
			// Нажать отмену в сообщении
			WorkspacePage.ClickCancelNotifier();

			// Дождаться, пока информационное окно пропадет
			Assert.IsTrue(
				WorkspacePage.WaitUntilDisappearNotifier(),
				"Ошибка: сообщение с экспортом не закрылось");
		}

		/// <summary>
		/// ТЕСТ: проверка, что сообщение об экспорте остается висеть, если инициировать экспорт в списке проектов
		/// </summary>
		/// <param name="placeSearch">где проверить сообщение об экспорте</param>
		[Test]
		[TestCase(PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST)]
		[TestCase(PLACE_SEARCH_NOTIFIER_OPEN_PROJECT)]
		[TestCase(PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY)]
		public void ExportSaveNotifierProjectList(string placeSearch)
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Наэать Экспорт Original
			ClickExportBtnProjectSettings(EXPORT_TYPE_SOURCE);
			// Дождаться кнопки Download
			WaitExportDownloadBtn();

			switch (placeSearch)
			{
				case PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_LIST:
					// обновить страницу
					Driver.Navigate().Refresh();
					break;
				case PLACE_SEARCH_NOTIFIER_OPEN_PROJECT:
					// перейти в проект
					OpenProjectPage(projectName);
					break;
				case PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY:
					// перейти на страницу глоссариев
					SwitchGlossaryTab();
					// проверить, что сообщения нет
					Assert.IsTrue(
						WorkspacePage.WaitUntilDisappearNotifier(),
						"Ошибка: информационное окно об экспорте появилось на странице глоссария");
					// вернуться к списку проектов
					SwitchWorkspaceTab();
					break;
				default:
					Assert.Fail("Неверный аргумент: " + placeSearch);
					break;
			}

			// Проверить, что есть окно с экпортом
			Assert.IsTrue(
				WorkspacePage.GetIsExistNotifier(),
				"Ошибка: информационное окно об экспорте пропало после обновления страницы");
		}

		/// <summary>
		/// ТЕСТ: проверка, что сообщение об экспорте остается висеть, если инициировать экспорт в проекте
		/// </summary>
		/// <param name="placeSearch"></param>
		[Test]
		[TestCase(PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE)]
		[TestCase(PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST)]
		[TestCase(PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY)]
		public void ExportSaveNotifierProjectPage(string placeSearch)
		{
			// Создать проект с документом (если его нет)
			var projectName = CreateProjectOneDocument(true);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Выделить документ
			SelectDocumentInProject();
			// Нажать экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться появления Doqnload
			WaitExportDownloadBtn();

			switch (placeSearch)
			{
				case PLACE_SEARCH_NOTIFIER_UPDATE_PROJECT_PAGE:
					// обновить страницу
					Driver.Navigate().Refresh();
					break;
				case PLACE_SEARCH_NOTIFIER_OPEN_PROJECT_LIST:
					// перейти к списку проектов
					SwitchWorkspaceTab();
					break;
				case PLACE_SEARCH_NOTIFIER_OPEN_GLOSSARY:
					// перейти в глоссарии
					SwitchGlossaryTab();
					// проверить, что сообщения нет
					Assert.IsTrue(
						WorkspacePage.WaitUntilDisappearNotifier(),
						"Ошибка: информационное окно об экспорте появилось на странице глоссария");
					// вернуться в проект
					SwitchWorkspaceTab();
					OpenProjectPage(projectName);
					break;
				default:
					Assert.Fail("Неверный аргумент: " + placeSearch);
					break;
			}

			// Проверить, что есть окно с экпортом
			Assert.IsTrue(
				WorkspacePage.GetIsExistNotifier(),
				"Ошибка: информационное окно об экспорте пропало после обновления страницы");
		}

		/// <summary>
		/// ТЕСТ: проверка, что сообщение об экспорте остается и при открытии другого проекта (не который инициировал экспорт)
		/// </summary>
		[Test]
		public void ExportSaveNotifierAnotherProjectPage()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Открыть первый проект
			OpenProjectPage(projectName);
			// Выделить документ
			SelectDocumentInProject();
			// Нажать экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Вернуться к списку проектов
			SwitchWorkspaceTab();
			// Открыть второй проект
			OpenProjectPage(projectName2);

			// Проверить, что есть окно с экпортом
			Assert.IsTrue(
				WorkspacePage.GetIsExistNotifier(),
				"Ошибка: информационное окно об экспорте пропало после обновления страницы");
		}

		/// <summary>
		/// ТЕСТ: экспорт документа из проекта: проверка текста сообщения (должно быть название документа)
		/// </summary>
		[Test]
		public void ExportDocumentFromProjectCheckNotifierText()
		{
			// Создать проект с документом (если его нет)
			CreateProjectOneDocument(true);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Выбрать документ
			SelectDocumentInProject();
			// Нажать экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться кнопки Download
			WaitExportDownloadBtn();

			// Проверить, есть ли в тексте сообщения название документа (ASSERT внутри)
			AssertNotifierDocumentName();
		}

		/// <summary>
		/// ТЕСТ: экспорт документов из проекта: проверка текста сообщения (должно быть 'документы')
		/// </summary>
		[Test]
		public void ExportDocumentsFromProjectCheckNotifierText()
		{
			// Создать проект с документами (если его нет)
			CreateCommonProjectMultiDocuments(true);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Выделить оба документа
			SelectDocumentInProject(1);
			SelectDocumentInProject(2);
			// Нажать Экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверить, что в тексте сообщения есть указание на несколько документов (ASSERT внутри)
			AssertNotifierMultiDocuments();
		}

		/// <summary>
		/// ТЕСТ: экспорт документа из списка проектов: проверка текста сообщения (должно быть название документа)
		/// </summary>
		[Test]
		public void ExportDocumentFromProjectListCheckNotifierText()
		{
			// Создать проект с документами (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Открыть свертку документа
			ClickDocumentOpenInfo();
			// Нажать экспорт в свертке документа Original
			ClickExportBtnDocumentSettings(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверить, есть ли в тексте сообщения название документа (ASSERT внутри)
			AssertNotifierDocumentName();
		}

		/// <summary>
		/// ТЕСТ: экспорт проекта с одним документом: проверка текста сообщения (должно быть название документа)
		/// </summary>
		[Test]
		public void ExportProjectOneDocCheckNotifierText()
		{
			// Создать проект с документами (если его нет)
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Выбрать этот проект
			SelectProjectInList(projectName);
			// Нажать "красный" экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверить, есть ли в тексте сообщения название документа (ASSERT внутри)
			AssertNotifierDocumentName();
		}

		/// <summary>
		/// ТЕСТ: экспорт проекта с несколькими документами: проверка текста сообщения (должно быть 'документы')
		/// </summary>
		[Test]
		public void ExportProjectMultiDocCheckNotifierText()
		{
			// Создать проект с документами (если его нет)
			var projectName = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Выбрать этот проект
			SelectProjectInList(projectName);
			// Нажать "красный" экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверить, что в тексте сообщения есть указание на несколько документов (ASSERT внутри)
			AssertNotifierMultiDocuments();
		}

		/// <summary>
		/// ТЕСТ: экспорт проектов с документами, проверка сообщения об экспорте (должно быть указание на несколько документов)
		/// </summary>
		[Test]
		public void ExportProjectsCheckNotifierText()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Выбрать оба проекта
			SelectProjectInList(projectName);
			SelectProjectInList(projectName2);

			// Нажать "красный" экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверить, что в тексте сообщения есть указание на несколько документов (ASSERT внутри)
			AssertNotifierMultiDocuments();
		}

		/// <summary>
		/// ТЕСТ: проверить, что в сообщении об экспорте указывается правильная дата (в пределах часа от текущей даты ОС)
		/// </summary>
		[Test]
		public void ExportDocumentCheckNotifierDate()
		{
			// Создать проект с документами (если его нет)
			CreateProjectOneDocument(true);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Выбрать документ
			SelectDocumentInProject();
			// Нажать Экспорт Original
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();
			// Текущая дата
			var curDate = DateTime.Now;
			// Получили дату из сообщения:
			var notifierDate = WorkspacePage.GetDateFromNotifier();
			// Разница между датами
			double timeSubtract = curDate.Subtract(notifierDate).Ticks;
			timeSubtract = timeSubtract < 0 ? (-1 * timeSubtract) : timeSubtract;

			// Если разница между датами/временем больше часа - ошибка
			Assert.IsTrue(
				timeSubtract < TimeSpan.TicksPerHour,
				"Ошибка: неправильная дата в сообщений об экспорте: " + WorkspacePage.GetNotifierText());
		}

		/// <summary>
		/// ТЕСТ: проверка, что между сообщениями можно переключаться
		/// </summary>
		[Test]
		public void ExportChangeNotifiers()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Экспортировать первый проект (один документ)
			SelectProjectInList(projectName);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Дождаться Download
			WaitExportDownloadBtn();
			// Убрать галочку с первого
			SelectProjectInList(projectName);

			// Экспортировать второй проект
			SelectProjectInList(projectName2);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Ждем второе сообщение об экспорте
			Assert.IsTrue(
				WorkspacePage.WaitNotifierAppear(2),
				"Ошибка: не появилось второе сообщение об экспорте");
			
			Thread.Sleep(1000);
			// Текст верхнего сообщения (первое сообщение)
			var firstNotifierText = WorkspacePage.GetNotifierText();
			// Кликнуть, чтобы переключить сообщения
			ChangeNotifier(1);
			// Текст верхнего сообщения
			var secondNotifierText = WorkspacePage.GetNotifierText();

			// Проверить, что сообщение изменилось/переключилось
			Assert.AreNotEqual(
				firstNotifierText, 
				secondNotifierText, 
				"Ошибка: сообщение не изменилось");
		}

		/// <summary>
		/// ТЕСТ: проверить, что можно открыть максимально возможное количество сообщений
		/// </summary>
		[Test]
		public void ExportLimitNotifiers()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть сообщения об экспорте
			InitializeLimitNotifiers(projectName);
		}

		/// <summary>
		/// ТЕСТ: проверить, что нельзя открыть сообщений больше максимального количества
		/// </summary>
		[Test]
		public void ExportMoreLimitNotifiers()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть сообщения об экспорте
			InitializeLimitNotifiers(projectName);
			// Вызвать еще раз экспорт
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// TODO попробовать убрать sleep
			Thread.Sleep(2000);
			var notifierCount = WorkspacePage.GetNotifierNumber();

			// Проверить, что сообщений не больше максимального количества
			Assert.IsFalse(
				notifierCount > MaxNotifierNumber, 
				"Ошибка: слишком много сообщений об экспорте");
		}

		/// <summary>
		/// ТЕСТ: проверка, что более свежее сообщение появляется поверх старых
		/// </summary>
		[Test]
		public void ExportCheckNotifiersFreshAbove()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Экспортировать первый проект (один документ)
			SelectProjectInList(projectName);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Дождаться Download
			WaitExportDownloadBtn();

			var firstNotifierText = WorkspacePage.GetNotifierText();

			// Убрать галочку с первого
			SelectProjectInList(projectName);

			// Экспортировать второй проект
			SelectProjectInList(projectName2);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Ждем второе сообщение об экспорте
			Assert.IsTrue(
				WorkspacePage.WaitNotifierAppear(2),
				"Ошибка: не появилось второе сообщение об экспорте");

			Thread.Sleep(1000);
			
			// Текст верхнего сообщения
			var secondNotifierText = WorkspacePage.GetNotifierText();

			// Проверить, что сообщение изменилось - то есть свежее сообщение сверху
			Assert.AreNotEqual(
				firstNotifierText, 
				secondNotifierText, 
				"Ошибка: сообщение не изменилось, свежее сообщение должно быть сверху");
		}

		/// <summary>
		/// ТЕСТ: проверка переключения между 1 и 3
		/// </summary>
		[Test]
		public void ExportChangeFirstToThird()
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Экспортировать первый проект (один документ)
			SelectProjectInList(projectName);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Дождаться Download
			WaitExportDownloadBtn();
			// Убрать галочку с первого
			SelectProjectInList(projectName);

			// Экспортировать второй проект
			SelectProjectInList(projectName2);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Ждем второе сообщение об экспорте
			Assert.IsTrue(
				WorkspacePage.WaitNotifierAppear(2),
				"Ошибка: не появилось второе сообщение об экспорте");
			
			// Убрать галочку с проекта
			SelectProjectInList(projectName2);
			// Открыть информацию о проекте
			OpenProjectInfo(projectName2);
			// Открыть информацию о втором документе
			ClickDocumentOpenInfo(2);
			// Экспорировать документ
			ClickExportBtnDocumentSettings(EXPORT_TYPE_SOURCE);

			// Ждем третье сообщение об экспорте
			Assert.IsTrue(
				WorkspacePage.WaitNotifierAppear(3),
				"Ошибка: не появилось третье сообщение об экспорте");

			// Текст верхнего сообщения
			var freshNotifierText = WorkspacePage.GetNotifierText();
			Logger.Trace("freshNotifierText: " + freshNotifierText);

			// Кликнуть, чтобы переключить сообщения - на первое
			ChangeNotifier(1);

			// Текст верхнего сообщения
			var currentNotifierText = WorkspacePage.GetNotifierText();
			Logger.Trace("currentNotifierText: " + currentNotifierText);
			var docName = Path.GetFileNameWithoutExtension(_exportFilePath);

			// Проверить, что появилось самое старое сообщение
			Assert.IsTrue(
				currentNotifierText.Contains(docName), 
				"Ошибка: кликнули по верхнему сообщению - появилось не первое!");
		}

		/// <summary>
		/// ТЕСТ: проверка переключения между 1 и 2 из 3
		/// </summary>
		[Test]
		public void ExportChangeFirstToSecond()
		{
			// Название документа, которое будем искать во втором сообщении
			var secondNotifierDocName = Path.GetFileNameWithoutExtension(_exportFilePath);
			// Создать проект
			var projectName = CreateProjectOneDocument(false);
			// Создать второй проект
			var projectName2 = CreateCommonProjectMultiDocuments(false);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();

			// Экспортировать второй проект (с 2 документами)
			SelectProjectInList(projectName2);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Дождаться Download
			WaitExportDownloadBtn();

			// Проверка, что первом сообщении нет текста, который мы будем искать при переключении на второе сообщение
			Assert.IsFalse(
				WorkspacePage.GetNotifierText().Contains(secondNotifierDocName),
				"Тестовая ошибка: в первом сообщении об экспорте есть искомое название документа (должно быть только во втором)");

			// Убрать галочку с проекта
			SelectProjectInList(projectName2);

			// Экспортировать первый проект (один документ)
			SelectProjectInList(projectName);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);
			// Убрать галочку с первого
			SelectProjectInList(projectName);

			// Ждем второе сообщение об экспорте
			Assert.IsTrue(
				WorkspacePage.WaitNotifierAppear(2),
				"Ошибка: не появилось второе сообщение об экспорте");

			// Проверка, что во втором сообщении есть искомый текст
			Assert.IsTrue(
				WorkspacePage.GetNotifierText().Contains(secondNotifierDocName),
				"Тестовая ошибка: во втором сообщении об экспорте нет нужного названия документа");

			// Экспортировать второй проект (с 2 документами)
			SelectProjectInList(projectName2);
			ClickExportBtnRed(EXPORT_TYPE_SOURCE);

			// Ждем третье сообщение об экспорте
			Assert.IsTrue(WorkspacePage.WaitNotifierAppear(3),
				"Ошибка: не появилось третье сообщение об экспорте");
			
			// Проверка, что третьем сообщении нет текста, который мы будем искать при переключении на второе сообщение
			Assert.IsFalse(WorkspacePage.GetNotifierText().Contains(secondNotifierDocName),
				"Тестовая ошибка: в третьем сообщении об экспорте есть искомое название документа (должно быть только во втором)");

			// Текст верхнего сообщения
			var freshNotifierText = WorkspacePage.GetNotifierText();
			Logger.Trace("freshNotifierText: " + freshNotifierText);

			// Кликнуть, чтобы переключить сообщения - на второе
			ChangeNotifier(2);

			// Текст верхнего сообщения
			var currentNotifierText = WorkspacePage.GetNotifierText();
			Logger.Trace("currentNotifierText: " + currentNotifierText);

			// Проверить, что появилось второе сообщение
			Assert.IsTrue(
				currentNotifierText.Contains(secondNotifierDocName),
				"Ошибка: кликнули по верхнему сообщению - появилось не второе!");
		}

		/// <summary>
		/// ТЕСТ: проверка, что после переименования документа, он экспортируется с новым именем
		/// </summary>
		/// <param name="exportType"></param>
		[Test]
		[TestCase(EXPORT_TYPE_SOURCE)]
		[TestCase(EXPORT_TYPE_TMX)]
		[TestCase(EXPORT_TYPE_TARGET)]
		public void ExportRenamedDocument(string exportType)
		{
			// Создать проект
			var projectName = CreateProjectOneDocument(false, true);
			// Закрыть все открытые сообщения об экспорте
			WorkspacePage.CancelAllNotifiers();
			// Открыть информацию о проекте
			OpenProjectInfo(projectName);
			// Открыть информацию о документе
			ClickDocumentOpenInfo();

			// Изменить имя документа
			WorkspacePage.ClickDocumentSettingsBtn();
			Assert.IsTrue(
				WorkspacePage.WaitDocumentSettingsDialogAppear(),
				"Ошибка: диалог настройки документа не открылся");
			// Ввести новое название документа
			var newDocumentName = "docName" + DateTime.Now.Ticks;
			WorkspacePage.DocumentSettingsAddName(newDocumentName);

			// Сохранить
			WorkspacePage.DocumentSettingsClickSave();
			WorkspacePage.WaitUntilDocumentSettingsDialogDisappear();

			// Экспортировать документ
			ClickExportBtnDocumentSettings(exportType);

			// Дождаться появления Download
			WaitExportDownloadBtn();

			// Проверить, что отображается новое имя документа
			var notifierText = WorkspacePage.GetNotifierText();
			Assert.IsTrue(
				notifierText.Contains(newDocumentName), 
				"Ошибка: экспортируется документ со старым названием");
		}



		/// <summary>
		/// Экспортировать после появления сообщения Download
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		/// <param name="multiDoc">производится экспорт нескольких документов</param>
		protected void WorkWithExport(string exportType, bool multiDoc = false)
		{
			// Дождаться появления Download в Notifier 
			WaitExportDownloadBtn();
			// Нажать Download
			WorkspacePage.ClickDownloadNotifier();
			var isNeedOriginalExtension = true;
			var fileExtension = "";

			if (multiDoc)
			{
				isNeedOriginalExtension = false;
				fileExtension = ".zip";
			}

			switch (exportType)
			{
				case EXPORT_TYPE_SOURCE:
					ExternalDialogSaveDocument("ExportedOriginalDocuments", true, _exportFilePath, isNeedOriginalExtension, fileExtension);
					break;
				case EXPORT_TYPE_TARGET:
					ExternalDialogSaveDocument("ExportedTranslatedDocuments", true, _exportFilePath, isNeedOriginalExtension, fileExtension);
					break;
				case EXPORT_TYPE_TMX:
					ExternalDialogSaveDocument("ExportedTMXDocuments", true, _exportFilePath, false, multiDoc ? fileExtension : ".tmx");
					break;
			}
		}

		/// <summary>
		/// Дождаться возможности загрузить экспорт
		/// </summary>
		protected void WaitExportDownloadBtn()
		{
			// Дождаться появления информационного окна
			Assert.IsTrue(
				WorkspacePage.GetIsExistNotifier(),
				"Ошибка: не появилось информационное окно об экспорте");

			// Дождаться появления кнопки Download
			Assert.IsTrue(
				WaitNotifierDownloadBtn(), 
				"Ошибка: не появилась кнопка Download");
		}

		/// <summary>
		/// Дождаться, пока появится кнопка Download в сообщении об экспорте
		/// </summary>
		/// <returns>появилась кнопка</returns>		
		protected bool WaitNotifierDownloadBtn()
		{
			var restartCounter = 0;
			var isExistDownloadBtn = false;

			// Дождаться, пока пропадет Prepare
			Assert.IsTrue(
				WorkspacePage.WaitUntilDisappearPrepareNotifier(),
				"Ошибка: сообщение Prepare висит слишком долго");

			// Проверить, появилась ли кнопка Download
			isExistDownloadBtn = WorkspacePage.GetIsDownloadBtnNotifierDisplayed();

			// Пробовать Restart, пока не появится Download, но не больше максимального количества попыток
			while (!isExistDownloadBtn && restartCounter < MAX_NUMBER_RESTART_EXPORT)
			{
				// Если нет сообщения об ошибке (Restart) - вообще нет сообщений - ошибка
				Assert.IsTrue(
					WorkspacePage.GetIsRestartBtnNotifierDisplayed(), 
					"Ошибка: нет ни сообщения Download, ни сообщения об ошибке");

				// Нажать Restart
				WorkspacePage.ClickRestartNotifier();

				// Проверить, что Restart начался
				var isRestartStarted = WorkspacePage.GetIsPrepareNotifierDisplayed()
					|| WorkspacePage.GetIsDownloadBtnNotifierDisplayed();

				// Нет ни сообщения о Prepare, ни download
				Assert.IsTrue(
					isRestartStarted, 
					"Ошибка: после нажатия на Restart не начался экпорт заново");
				// Дождаться, пока пропадет Prepare
				Assert.IsTrue(
					WorkspacePage.WaitUntilDisappearPrepareNotifier(),
						"Ошибка: сообщение Prepare висит слишком долго");

				// Проверить, появилась ли кнопка Download
				isExistDownloadBtn = WorkspacePage.GetIsDownloadBtnNotifierDisplayed();
				++restartCounter;
			}

			if (!isExistDownloadBtn)
			{
				// Если сделали N Restart - вывести ошибку, что тест дальше не может проверить функционал
				Assert.IsTrue(
					restartCounter < MAX_NUMBER_RESTART_EXPORT,
					"Ошибка при экспорте! Выводится сообщение, что при экспорте произошла ошибка! Тест дальше проходить не может");
			}

			return isExistDownloadBtn;
		}

		/// <summary>
		/// Нажать "красный" экспорт
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		protected void ClickExportBtnRed(string exportType)
		{
			//Нажать на кнопку Export
			Assert.IsTrue(
				WorkspacePage.ClickExportRedBtn(), 
				"Ошибка: кнопка Экспорт заблокирована");
			
			//Выбрать тип экспорта
			SelectExportType(exportType);
		}

		/// <summary>
		/// Выбрать тип экспорта в выпадающем списке
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		protected void SelectExportType(string exportType)
		{
			//Выбрать тип экспорта
			WorkspacePage.SelectExportType(GetExportTypePage(exportType));
		}

		/// <summary>
		/// Нажать Экспорт в свертке проекта
		/// </summary>
		/// <param name="exportType"></param>
		protected void ClickExportBtnProjectSettings(string exportType)
		{
			//Нажать на кнопку Export
			WorkspacePage.ClickExportBtnProjectInfo();
			//Выбрать тип экспорта
			SelectExportType(exportType);
		}

		/// <summary>
		/// Нажать экспорт свертки документа
		/// </summary>
		/// <param name="exportType">тип экспорта</param>
		protected void ClickExportBtnDocumentSettings(string exportType)
		{
			//Нажать на кнопку Export в свертке документа
			WorkspacePage.ClickExportBtnDocumentInfo();
			//Выбрать тип экспорта
			SelectExportType(exportType);
		}

		/// <summary>
		/// Получить тип экспорта по читаемому имени
		/// </summary>
		/// <param name="exportType">читаемое имя экспорта</param>
		/// <returns>внутренний тип экспорта</returns>
		protected WorkSpacePageHelper.EXPORT_TYPE GetExportTypePage(string exportType)
		{
			var exportTypePage = WorkSpacePageHelper.EXPORT_TYPE.Original;

			switch (exportType)
			{
				case EXPORT_TYPE_SOURCE:
					exportTypePage = WorkSpacePageHelper.EXPORT_TYPE.Original;
					break;
				case EXPORT_TYPE_TMX:
					exportTypePage = WorkSpacePageHelper.EXPORT_TYPE.TMX;
					break;
				case EXPORT_TYPE_TARGET:
					exportTypePage = WorkSpacePageHelper.EXPORT_TYPE.Translated;
					break;
			}

			return exportTypePage;
		}

		/// <summary>
		/// Проверить, что в тексте сообщения об экспорте есть название документа
		/// </summary>
		protected void AssertNotifierDocumentName()
		{
			var notifierText = WorkspacePage.GetNotifierText();
			var docName = Path.GetFileNameWithoutExtension(_exportFilePath);
			Logger.Trace("docName " + docName);
			Logger.Trace("notifierText: " + notifierText);

			// Есть ли в тексте сообщения название документа:
			Assert.IsTrue(
				notifierText.Contains(docName),
				"Ошибка: неправильный текст в сообщении об экспорте: нет названия документа.\n"
					+ "Документ: " + docName
					+ "\nСейчас текст: " + notifierText);
		}

		/// <summary>
		/// Проверить, что в тексте сообщения об экспорте есть указание на множественное число документов
		/// </summary>
		protected void AssertNotifierMultiDocuments()
		{
			// Текст сообщения об экспорте
			var notifierText = WorkspacePage.GetNotifierText();

			// Проверить, что в тексте есть указание на несколько документов
			Assert.IsTrue(
				notifierText.Contains(EXPORT_NOTIFIER_DOWNLOAD_DOCUMENTS),
				"Ошибка: неправильный текст в сообщении об экспорте: нет указания на несколько документов.\nСейчас текст: " + notifierText);
		}

		/// <summary>
		/// Открыть сообщения об экспорте (максимально допустимое)
		/// </summary>
		/// <param name="projectName">имя проекта, который экспортировать</param>
		protected void InitializeLimitNotifiers(string projectName)
		{
			// Выбрать проект
			SelectProjectInList(projectName);

			for (var i = 0; i < MaxNotifierNumber; ++i)
			{
				// Нажать "красный" экспорт Original
				ClickExportBtnRed(EXPORT_TYPE_SOURCE);
				// TODO проверить без SLEEP
				Thread.Sleep(2000);
				// Дождаться появления нового сообщения об экспорте
				Assert.IsTrue(WorkspacePage.WaitNotifierAppear(i + 1),
					"Ошибка: не появилось новое сообщение об экспорте (" + (i + 1) + ")");
			}
		}

		/// <summary>
		/// Кликнуть для изменения сообщения об экспорте
		/// </summary>
		/// <param name="notifierNumberFromTop">номер сообщения об экспорте сверху (от старого к новому)</param>
		protected void ChangeNotifier(int notifierNumberFromTop)
		{
			// Кликнуть, чтобы переключить сообщения
			WorkspacePage.ClickNotifier(notifierNumberFromTop);
		}

		/// <summary>
		/// Кликнуть, чтобы открыть свертку документов
		/// </summary>
		/// <param name="documentNumber">номер документа</param>
		protected void ClickDocumentOpenInfo(int documentNumber = 1)
		{
			// Кликнуть на открытие информации о документе
			Assert.IsTrue(
				WorkspacePage.OpenDocumentInfo(documentNumber), 
				"Ошибка: нет такого документа N " + documentNumber);
		}

		/// <summary>
		/// Создать проект, загрузить документ, ввести перевод
		/// </summary>
		/// <param name="isNeedOpenProject">Нужно ли открыть проект</param>
		/// <param name="isNeedUniqueProject">должно быть уникальное имя?</param>
		/// <returns>название проекта</returns>
		protected string CreateProjectOneDocument(bool isNeedOpenProject, bool isNeedUniqueProject = false)
		{
			// имя общего проекта
			var currentProjectName = "";

			if (isNeedUniqueProject)
			{
				currentProjectName = "TestProject" + DateTime.Now.Ticks;
			}
			else
			{
				currentProjectName = ProjectNameExportTestOneDoc;
			}

			// проверить, есть ли проект
			// (рассчитываем на то, что если проект есть, то в нем уже есть документ с переводом)
			var isProjectNotExist = isNeedUniqueProject || GetIsNotExistProject(currentProjectName);

			if (isProjectNotExist)
			{
				// Создать проект с документом и открыть документ
				CreateReadyProject(currentProjectName, false, false, _exportFilePath);
				// Ввести текст и подтвердить
				AddTranslationAndConfirm();
				// Выйти
				EditorClickHomeBtn();

				if (!isNeedOpenProject)
				{
					GoToTranslationMemories();
				}
			}
			else if (isNeedOpenProject)
			{
				// Заходим в проект
				OpenProjectPage(currentProjectName);
			}


			// Вернуть имя проекта
			return currentProjectName;
		}

		/// <summary>
		/// Создать универсальный проект с 2 документами с переводами
		/// </summary>
		/// <param name="isNeedOpenProject">нужно ли остаться на странице проекта</param>
		/// <returns>название проекта</returns>
		protected string CreateCommonProjectMultiDocuments(bool isNeedOpenProject)
		{
			var currentProjectName = ProjectNameExportTestMultiDoc;
			var isNotExistProject = GetIsNotExistProject(currentProjectName);

			// Создать проект, если его нет
			if (isNotExistProject)
			{
				GoToWorkspace();
				// Создать проект
				CreateProject(currentProjectName);
				// Загрузить документ
				ImportDocumentProjectSettings(TestFile.DocumentFileToConfirm, currentProjectName);
				// Назначить
				AssignTask();
				// Открыть документ
				OpenDocument();
				// Добавить перевод и подтвердить
				AddTranslationAndConfirm();
				// Выйти из редактора
				EditorClickHomeBtn();
				GoToWorkspace();
				// Загрузить второй документ
				ImportDocumentProjectSettings(
					TestFile.DocumentFileToConfirm2, 
					currentProjectName);
				// Назначить второй
				AssignTask(2);
				// Открыть второй документ
				OpenDocument(2);
				// Добавить перевод и подтвердить
				AddTranslationAndConfirm();
				// Выйти из редактора
				EditorClickHomeBtn();

				if (!isNeedOpenProject)
				{
					// Перейти к проекту
					GoToWorkspace();
				}
			}
			else if (isNeedOpenProject)
			{
				// Открыть проект
				OpenProjectPage(currentProjectName);
			}

			return currentProjectName;
		}

		/// <summary>
		/// Конченые действия для каждого теста
		/// </summary>
		[TearDown]
		public void Teardown()
		{
			WorkspacePage.CancelAllNotifiers();
		}

		const int MaxNotifierNumber = 5;
	}
}