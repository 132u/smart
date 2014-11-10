using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public TMTest(string browserName)
			: base(browserName)
		{
		}

		/// <summary>
		/// Предварительная подготовка для каждого тест-метода
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не закрывать браузер
			quitDriverAfterTest = false;

			// Переходим к странице воркспейса
			GoToTranslationMemories();

			// Выбираем имя для ТМ, которое будет использовано в тесте
			UniqueTmName = GetUniqueTMName();
		}

		#region Методы создания ТМ
		/// <summary>
		/// Открыть форму создания ТМ
		/// </summary>
		public void OpenCreateTMForm()
		{
			Assert.IsTrue(TMPage.OpenCreateTMDialog(), "Ошибка: не открылась форма создания ТМ");
		}

		/// <summary>
		/// Создать ТМ и сохранить
		/// </summary>
		/// <param name="TMName">Название ТМ</param>
		/// <param name="sourceLang">Язык источника</param>
		/// <param name="targetLang">Язык перевода</param>
		public void CreateTMByNameAndSave(
			string TMName,
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			// Создать ТМ без сохранения формы
			CreateTMByName(TMName, sourceLang, targetLang, isMultilanguageTm);

			// Нажать кнопку Сохранить
			TMPage.ClickSaveNewTM();
			
			// Дождаться окончания загрузки
			Assert.IsTrue(TMPage.WaitDocumentDownloadFinish(),
				"Ошибка: документ загружается слишком долго");
		}

		/// <summary>
		/// Создать ТМ (без сохранения)
		/// </summary>
		/// <param name="TMName">Название ТМ</param>
		/// <param name="sourceLang">Язык источника</param>
		/// <param name="targetLang">Язык перевода</param>
		/// <param name="isMultilanguageTm">несколько языков перевода</param>
		public void CreateTMByName(
			string TMName,
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			// Открыть форму создания ТМ
			OpenCreateTMForm();

			// Ввести имя
			TMPage.InputNewTMName(TMName);

			// Выбрать языки (source и target), чтобы сохранить ТМ
			SelectSourceAndTargetLang(sourceLang, targetLang, isMultilanguageTm);
		}

		/// <summary>
		/// Создать ТМ, если его нет
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void CreateTMIfNotExist(string TMName)
		{
			if (!GetIsExistTM(TMName))
			{
				// Если нет такого ТМ, создать  его
				CreateTMByNameAndSave(TMName);
			}
		}

		/// <summary>
		/// Создать ТМ с загрузкой TMX
		/// </summary>
		/// <param name="uniqueTMName">название ТМ</param>
		/// <param name="fileName">файл для загрузки</param>
		public void CreateTMWithUploadTMX(
			string uniqueTMName,
			string fileName,
			bool checkBaloonExisting = false)
		{
			// Создать ТМ
			CreateTMByName(uniqueTMName);

			// Нажать на Сохранить и Импортировать TMX файл
			TMPage.ClickSaveAndImportCreateTM();

			// Загрузить TMX файл
			UploadDocumentTM(fileName, uniqueTMName, checkBaloonExisting);
		}

		/// <summary>
		/// Выбрать уникальное имя
		/// </summary>
		/// <returns>имя</returns>
		public static string GetUniqueTMName(string tmName)
		{
			// Создать уникальное имя для ТМ без проверки существова
			return tmName + DateTime.Now;
		}
		#endregion

		#region Методы связанные с загрузкой ТМХ
		/// <summary>
		/// Загрузить документ в ТМ
		/// </summary>
		/// <param name="documentName">название документа</param>
		/// <param name="tmName">имя ТМ</param>
		/// <param name="checkBaloonExisting">проверять существование всплывающего окна</param>
		public void UploadDocumentTM(
			string documentName,
			string tmName,
			bool checkBaloonExisting = false,
			bool acceptConfirmationMessage = false)
		{
			// Узнаем текущую локализацию для проверки сообщения на плашках
			var locale = WorkspacePage.GetCurrentLocale();

			TMPage.WaitUntilUploadDialog();
			// Нажать на Add для появления диалога загрузки документа
			TMPage.ClickAddUploadBtn();

			// Заполнить диалог загрузки документа
			FillAddDocumentForm(documentName);

			Console.WriteLine(documentName);

			// Нажать на Импорт
			TMPage.ClickImportBtn();

			// Подтверждаем, что согласны на перезатирание ТМ, если передан флаг acceptConfirmationMessage = true
			if (acceptConfirmationMessage)
			{
				TMPage.ConfirmTMEdition();
			}

			// Проверяем наличие информационных плашек, если это необходимо
			if (checkBaloonExisting)
			{
				CheckTMInformationBaloonExisting(tmName, locale);
			}

			Console.WriteLine("кликнули импорт");

			if (TMPage.GetIsErrorMessageNotTMX())
			{
				SendKeys.SendWait(@"{Enter}");
				Thread.Sleep(1000);
				// Нажать на Импорт
				TMPage.ClickImportBtn();

				// Проверяем наличие информационных плашек, если это необходимо
				if (checkBaloonExisting)
				{
					CheckTMInformationBaloonExisting(tmName, locale);
				}
			}

			// Дождаться окончания загрузки
			Assert.IsTrue(TMPage.WaitDocumentDownloadFinish(),
				"Ошибка: документ загружается слишком долго");
		}

		/// <summary>
		/// Проверка существования необходимых плашек при загрузке TM
		/// </summary>
		public void CheckTMInformationBaloonExisting(
			string tmName,
			WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT locale)
		{
			bool isProcessingInformationBaloonExist;
			bool isTUInformationBaloonExist;

			if (locale == WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English)
			{
				// Получаем информацию о наличии\отсутствии плашки с предупреждением о том, 
				// что информация обрабатывается была выведена
				isProcessingInformationBaloonExist = TMPage.IsBaloonWithSpecificMessageExist(
					string.Format("Importing translation units to \"{0}\" TM", tmName));

				// Проверяем, что плашка с информацией о количестве загруженных TU была выведена
				// 20 - количество TU в файле TMFile2.tmx. Пока данный метод используется только в одном тесте, 
				// поэтому 20 захардкожена.
				isTUInformationBaloonExist = TMPage.IsBaloonWithSpecificMessageExist(
					string.Format("20 translation units have been successfully imported into \"{0}\" TM.", tmName));
			}
			else
			{
				isProcessingInformationBaloonExist = TMPage.IsBaloonWithSpecificMessageExist(
					string.Format("Добавление единиц перевода в TM \"{0}\"", tmName));

				isTUInformationBaloonExist = TMPage.IsBaloonWithSpecificMessageExist(
					string.Format("В TM \"{0}\" успешно добавлено 20 единиц перевода.", tmName));
			}

			Assert.IsTrue(isProcessingInformationBaloonExist,
				"Ошибка: плашка с предупреждением о том, что информация обрабатывается не была выведена.");

			Assert.IsTrue(isTUInformationBaloonExist,
				"Ошибка: плашка с информацией о количестве загруженных TU не была выведена.");
		}

		/// <summary>
		/// Загрузить документ в ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <param name="btnType">по какой кнопке (Update, Add)</param>
		/// <param name="uploadFile">файл для загрузки</param>
		public void UploadDocumentToTMbyButton(
			string TMName,
			TMPageHelper.TM_BTN_TYPE btnType,
			string uploadFile)
		{
			// Отрыть информацию о ТМ и нажать кнопку
			ClickButtonTMInfo(TMName, btnType);
			// Загрузить документ
			UploadDocumentTM(uploadFile, TMName);
		}
		#endregion

		#region Методы для работы с элементами страницы
		/// <summary>
		/// Кликнуть кнопку в информации о ТМ
		/// </summary>
		/// <param name="TMName"></param>
		/// <param name="btnType"></param>
		/// <param name="isConfirmationQuestionExist"></param>
		public void ClickButtonTMInfo(
			string TMName,
			TMPageHelper.TM_BTN_TYPE btnType,
			bool isConfirmationQuestionExist = false)
		{
			// Открыть информацию о ТМ
			OpenTMInfo(TMName);

			// Кликнуть кнопку
			TMPage.ClickTMButton(btnType);

			if (isConfirmationQuestionExist)
			{
				TMPage.ConfirmTMEdition();
			}
		}

		/// <summary>
		/// Переоткрыть информацию о ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void ReopenTMInfo(string TMName)
		{
			if (TMPage.GetIsTMOpened(TMName))
			{
				// Если открыта - закрыть
				TMPage.ClickTMRow(TMName);
			}
			OpenTMInfo(TMName);
		}

		/// <summary>
		/// Открыть информацию о ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		public void OpenTMInfo(string TMName)
		{
			if (!TMPage.GetIsTMOpened(TMName))
			{
				TMPage.ClickTMRow(TMName);
				// TODO проверить sleep
				Thread.Sleep(500);
			}
		}
		#endregion

		#region Методы редактирования ТМ
		/// <summary>
		/// Изменить название ТМ
		/// </summary>
		/// <param name="TMNameToEdit">Тм для изменения</param>
		/// <param name="newTMName">новое имя</param>
		public void EditTMFillName(string TMNameToEdit, string newTMName)
		{
			// Отрыть информацию о ТМ и нажать Edit
			ClickButtonTMInfo(TMNameToEdit, TMPageHelper.TM_BTN_TYPE.Edit);

			// Ждем открытия формы редактирования
			TMPage.WaitUntilEditTMOpen();

			// Очистить поле Имя
			TMPage.EditTMClearName();

			// Если новое имя не пустое, то заполнить им поле Имя
			if (newTMName.Length > 0)
			{
				TMPage.InputEditTMName(newTMName);
			}

			// Сохранить изменение
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			// TODO убрать sleep
			Thread.Sleep(2000);
		}

		/// <summary>
		/// Добавить группу проектов к ТМ и вернуть имя добаленной группы
		/// </summary>
		public string AddProjectToTmAndGetProjectName(string tmName)
		{
			// Отрыть информацию о ТМ и нажать Edit
			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			// Ждем открытия формы редактирования
			TMPage.WaitUntilEditTMOpen();

			// открываем список группы проектов
			TMPage.ClickToProjectsListAtTmEdditForm();

			// выбираем первую в списке группу проектов и возвращаем ее имя
			var projectGroupName = TMPage.EditTMAddProject();

			// Сохранить изменение
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			// TODO убрать sleep
			Thread.Sleep(2000);

			return projectGroupName;
		}

		/// <summary>
		/// Добавить язык перевода
		/// </summary>
		public void EditTMAddTargetLanguage(
			string tmName, 
			CommonHelper.LANGUAGE languagetToAdd)
		{
			// Отрыть информацию о ТМ и нажать Edit
			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			// Ждем открытия формы редактирования
			TMPage.WaitUntilEditTMOpen();

			// Открываем список языков перевода
			TMPage.ClickToTargetLanguagesAtTmEdditForm();

			// Добавляем язык перевода
			TMPage.SelectTargetLanguage(languagetToAdd);

			// Сохранить изменение
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			// TODO убрать sleep
			Thread.Sleep(2000);
		}

		/// <summary>
		/// Заполнить поле комментария ТМ
		/// </summary>
		public void FillingCommentForm(
			string tmName, 
			string comment)
		{
			// Отрыть информацию о ТМ и нажать Edit
			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			// Ждем открытия формы редактирования
			TMPage.WaitUntilEditTMOpen();

			// Очистить поле Комментарий
			TMPage.EditTMClearComment();

			// Ввести комментарий
			TMPage.InputEditTMComment(comment);

			// Сохранить изменение
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			Thread.Sleep(2000);
		}
		#endregion

		#region Методы поиска/подсчета объектв на странице
		/// <summary>
		/// Вернуть, есть ли ТМ в списке при создании проекта
		/// </summary>
		/// <param name="TMName">Название ТМ</param>
		/// <param name="isNeedChangeLanguages">Необходимость в смене языка</param>
		/// <param name="srcLang">Язык источника</param>
		/// <param name="trgLang">Язык перевода</param>
		/// <returns></returns>
		public bool GetIsExistTMCreateProjectList(
			string TMName,
			bool isNeedChangeLanguages = false,
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian)
		{
			// Перейти на страницу со списком проектов
			SwitchWorkspaceTab();
			// Заполнить создание проекта на первом шаге
			if (isNeedChangeLanguages)
			{
				FirstStepProjectWizard(ProjectName, false, srcLang, trgLang);
			}
			else
			{
				FirstStepProjectWizard(ProjectName);
			}

			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();

			// Проверить, что есть ТМ
			return WorkspaceCreateProjectDialog.GetIsExistTM(TMName);
		}

		/// <summary>
		/// Вернуть: есть ли ТМ в списке
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <returns>есть</returns>
		public bool GetIsExistTM(string TMName)
		{
			// Есть ли ТМ с таким именем в списке на странице Translation Memory Bases
			return TMPage.GetIsExistTM(TMName);
		}

		/// <summary>
		/// Вернуть: если для ТМ найден комментарий comment
		/// </summary>
		public bool GetIsCommentExist(string tmName, string comment)
		{
			// Открыть информацию о ТМ
			OpenTMInfo(tmName);

			// Есть ли комментарий 
			return TMPage.GetIsCommentExist(comment);
		}

		/// <summary>
		/// Вернуть true, если в колонке с языками указаны корректные языки
		/// </summary>
		public bool GetIsCorrectLanguagesForTm(
			string tmName, 
			string sourceLanguage, 
			string[] targetLanguage)
		{
			var formattedLanguagesString = string.Concat(
				sourceLanguage,
				" > ", 
				string.Join(", ", targetLanguage));

			return TMPage.GetIsCorrectLanguagesForTm(tmName, formattedLanguagesString);
		}

		/// <summary>
		/// Вернуть true, если проект с именем projectName указан для ТМ tmName
		/// </summary>
		public bool GetIsProjectExistForTm(string tmName, string projectName)
		{
			// Открыть информацию о ТМ
			OpenTMInfo(tmName);

			// Вернуть true, если проект с именем projectName указан для ТМ tmName
			return TMPage.GetIsProjectExistInTmInformation(tmName, projectName);
		}

		/// <summary>
		/// Получить количество сегментов ТМ
		/// </summary>
		/// <param name="TMName">название ТМ</param>
		/// <returns>количество сегментов</returns>
		public int GetSegmentCount(string TMName)
		{
			// Открыть информацию о ТМ
			OpenTMInfo(TMName);

			// Получить количество сегментов
			return TMPage.GetSegmentCount();
		}
		#endregion

		#region Методы экспорта/импорта ТМХ
		/// <summary>
		/// Экспортировать (работа с внешним диалогом)
		/// </summary>
		public void ExportTM()
		{
			//ExternalDialogSaveDocument("TMExportTest", true, TMName, false, ".tmx");

			string resultPath = Path.Combine(PathTestResults, "TMExportTest");
			Directory.CreateDirectory(resultPath);

			string newFileName = DateTime.Now.Ticks.ToString();
			resultPath = Path.Combine(resultPath, newFileName + ".tmx");

			ExternalDialogSelectSaveDocument(resultPath);
			Assert.IsTrue(File.Exists(resultPath), "Ошибка: файл не экспортировался\n" + resultPath);
		}
		#endregion

		public string UniqueTmName { get; set; }

		public static readonly string[] importTMXFileList = Directory.GetFiles(
																Path.Combine(
																	Environment.CurrentDirectory,
																	@"..\TestingFiles\",
																	"TMTestFiles"));

		#region Private методы
		/// <summary>
		/// Выбрать языки Source и Target
		/// </summary>
		private void SelectSourceAndTargetLang(
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			// Нажать на Source Language для выпадения списка языков
			TMPage.ClickOpenSourceLangList();
			// Выбираем Английский
			TMPage.SelectSourceLanguage(sourceLang);

			// Нажать на Target Language для выпадения списка языков
			TMPage.ClickTargetLangList();
			// ждем выпадения списка
			// TODO проверить без ожидания
			//Wait.Until((d) => d.FindElement(By.XPath(
			//".//div[contains(@class,'ui-multiselect-menu')][contains(@class,'js-languages-multiselect')]")));
			// Выбираем Русский по value
			TMPage.SelectTargetLanguage(targetLang);

			// Если передан флаг, что в машине необходимо выбрать 
			// несколько языков перевода, выбираем еще один язык
			if (isMultilanguageTm)
			{
				TMPage.SelectTargetLanguage(CommonHelper.LANGUAGE.Lithuanian);
			}

			// Нажать на Target Language для закрытия списка
			TMPage.ClickTargetLangList();
		}

		/// <summary>
		/// Выбрать уникальное имя
		/// </summary>
		private string GetUniqueTMName()
		{
			// Создать уникальное имя для ТМ без проверки существова
			return ConstTMName + DateTime.Now;
		}
		#endregion
	}
}
