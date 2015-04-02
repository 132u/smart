using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using NUnit.Framework;
using OpenQA.Selenium;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetupCommonTest()
		{
			Logger.Info("Начало работы метода SetUp. Подготовка перед каждым тестом.");
			
			Logger.Debug("Значение параметра QuitDriverAfterTest = false. Не закрывать браузер после каждого теста.");
			QuitDriverAfterTest = false;

			GoToUrl(RelativeUrlProvider.TranslationMemories);

			UniqueTmName = GetUniqueTMName();
		}

		#region Методы создания ТМ

		public void CreateTMByNameAndSave(
			string tmName,
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			Logger.Debug(string.Format("Создание и сохранение ТМ. Имя: {0}, язык источника: {1}, язык перевода: {2}",
				tmName, sourceLang, targetLang));

			TMPage.OpenCreateTMDialog();
			CreateTMByName(tmName, sourceLang, targetLang, isMultilanguageTm);
			TMPage.ClickSaveNewTM();

			TMPage.AssertionDocumentDownloadFinish();
		}

		public void CreateTMByName(
			string tmName,
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			Logger.Debug(string.Format("Создание ТМ. Имя: {0}, язык источника: {1}, язык перевода: {2}, мультиязыковая ТМ: {3}",
				tmName, sourceLang, targetLang, isMultilanguageTm));
			
			TMPage.InputNewTMName(tmName);

			SelectSourceAndTargetLang(sourceLang, targetLang, isMultilanguageTm);
		}

		public void CreateTMIfNotExist(
			string tmName,
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian)
		{
			Logger.Debug(string.Format("Создание ТМ с именем {0}, если такой не найдено", tmName));

			if (!TMPage.GetIsExistTM(tmName))
			{
				CreateTMByNameAndSave(tmName, sourceLang, targetLang);
			}
		}

		public void CreateTMWithUploadTMX(
			string uniqueTMName,
			string fileName,
			bool checkBaloonExisting = false,
			bool needToSave = true)
		{
			Logger.Debug(string.Format("Создание ТМ {0} с файлом для загрузки {1}. Проверять наличие информационной плашки: {2}", uniqueTMName, fileName, checkBaloonExisting));

			TMPage.OpenCreateTMDialog();
			TMPage.UploadTmxInCreateDialog(fileName);
			TMPage.InputNewTMName(uniqueTMName);
			TMPage.ClickTargetLangList();
			TMPage.SelectTargetLanguage(CommonHelper.LANGUAGE.Russian);

			if (needToSave)
			{
				TMPage.ClickSaveNewTM();

				Assert.IsFalse(TMPage.GetIsErrorMessageNotTmx(),
					"Ошибка: появилось сообщение о неверном расширении файла");
			}

			if (checkBaloonExisting)
			{
				TMPage.AssertionBaloonWithSpecificMessageDisappear(string.Format(
					"Adding translation units from the file {0} to the translation memory \"{1}\"...", fileName, uniqueTMName));
			}
		}

		public static string GetUniqueTMName(string tmName)
		{
			// Создать уникальное имя для ТМ без проверки существова
			return tmName + DateTime.Now;
		}
		#endregion

		#region Методы связанные с загрузкой ТМХ

		public void UploadDocumentTM(
			string documentName,
			string tmName,
			bool checkBaloonExisting = false,
			bool acceptConfirmationMessage = false)
		{
			Logger.Debug(string.Format("Загрузить документ {0} в ТМ {1}. Проверить существование всплывающего окна: {2}. Проверить существование информационной плашки: {3}", 
				documentName, tmName, acceptConfirmationMessage, checkBaloonExisting));
			
			var locale = WorkspacePage.GetCurrentLocale();

			TMPage.WaitUntilUploadDialog();
			TMPage.UploadTMXInUpdatePopUp(documentName);
			Logger.Trace(documentName);
			TMPage.ClickImportBtn();

			// Подтверждаем, что согласны на перезатирание ТМ, если передан флаг acceptConfirmationMessage = true
			if (acceptConfirmationMessage)
			{
				Assert.IsTrue(TMPage.GetConfirmWindowExist(), "Ошибка: Confirmation окно не появилось");
				TMPage.ConfirmTMEdition();
			}
			// Проверяем наличие информационных плашек, если это необходимо
			if (checkBaloonExisting)
			{
				CheckTMInformationBaloonExisting(tmName, locale);
			}

			if (TMPage.GetIsErrorMessageNotTmx())
			{
				SendKeys.SendWait(@"{Enter}");
				Thread.Sleep(1000);
				TMPage.ClickImportBtn();

				if (checkBaloonExisting)
				{
					CheckTMInformationBaloonExisting(tmName, locale);
				}
			}

			TMPage.AssertionDocumentDownloadFinish();
		}

		public void CheckTMInformationBaloonExisting(
			string tmName,
			WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT locale)
		{
			Logger.Debug(string.Format("Проверка существования необходимых плашек. Имя ТМ: {0}, локализация: {1}", tmName, locale));
			
			bool isProcessingInformationBaloonExist;
			bool isTUInformationBaloonExist;

			if (locale == WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English)
			{
				// Получаем информацию о наличии\отсутствии плашки с предупреждением о том, 
				// что информация обрабатывается была выведена
				isProcessingInformationBaloonExist = TMPage.IsTextExistInBaloon(
					string.Format("Adding translation units from the file TMFile2.tmx to the translation memory \"{0}\"...", tmName));

				// Проверяем, что плашка с информацией о количестве загруженных TU была выведена
				// 20 - количество TU в файле TMFile2.tmx. Пока данный метод используется только в одном тесте, 
				// поэтому 20 захардкожена.
				isTUInformationBaloonExist = TMPage.IsTextExistInBaloon(
					string.Format("20 out of 20 translation units imported from file TMFile2.tmx to the translation memory \"{0}\".", tmName));
			}
			else
			{
				isProcessingInformationBaloonExist = TMPage.IsTextExistInBaloon(
					string.Format("Добавление единиц перевода в память переводов \"{0}\" из файла TMFile2.tmx...", tmName));

				isTUInformationBaloonExist = TMPage.IsTextExistInBaloon(
					string.Format("Из файла TMFile2.tmx в память переводов \"{0}\" успешно добавлено 20 единиц перевода.", tmName));
			}

			Assert.IsTrue(isProcessingInformationBaloonExist,
				"Ошибка: плашка с предупреждением о том, что информация обрабатывается не была выведена.");

			Assert.IsTrue(isTUInformationBaloonExist,
				"Ошибка: плашка с информацией о количестве загруженных TU не была выведена.");
		}

		public void UploadDocumentToTMbyButton(
			string tmName,
			TMPageHelper.TM_BTN_TYPE btnType,
			string uploadFile)
		{
			Logger.Debug(string.Format("Загрузка документа {0} в ТМ {1} с помощью кнопки {2}", uploadFile, tmName, btnType));

			ClickButtonTMInfo(tmName, btnType);
			UploadDocumentTM(uploadFile, tmName);
		}
		#endregion

		#region Методы для работы с элементами страницы

		public void ClickButtonTMInfo(
			string tmName,
			TMPageHelper.TM_BTN_TYPE btnType,
			bool isConfirmationQuestionExist = false)
		{
			Logger.Debug(string.Format("Кликнуть кнопку {0} в информации о ТМ {1}. Наличие вопроса с подтверждением: {2}",
				btnType, tmName, isConfirmationQuestionExist));

			// Необходим рефреш страницы, иначе загрузка ТМХ не работает
			RefreshPage();

			TMPage.ScrollToRequiredTm(tmName);
			OpenTMInfo(tmName);
			TMPage.ClickTMButton(btnType);

			if (isConfirmationQuestionExist)
			{
				TMPage.ConfirmTMEdition();
			}
		}

		public void ReopenTMInfo(string tmName)
		{
			Logger.Debug(string.Format("Переоткрыть информацию о ТМ {0}", tmName));

			if (TMPage.GetIsTMOpened(tmName))
			{
				// Если открыта - закрыть
				TMPage.ClickTMRow(tmName);
			}

			OpenTMInfo(tmName);
		}

		public void OpenTMInfo(string tmName)
		{
			Logger.Debug(string.Format("Открыть информацию о ТМ {0}", tmName));
			TMPage.ScrollToRequiredTm(tmName);
			if (!TMPage.GetIsTMOpened(tmName))
				TMPage.ClickTMRow(tmName);
		}
		#endregion

		#region Методы редактирования ТМ

		public void EditTMName(string tmNameToEdit, string newTMName)
		{
			Logger.Debug(string.Format("Изменить имя ТМ с {0} на {1}", tmNameToEdit, newTMName));

			// Отрыть информацию о ТМ и нажать Edit
			ClickButtonTMInfo(tmNameToEdit, TMPageHelper.TM_BTN_TYPE.Edit);

			TMPage.AssertionEditTMFormIsOpen();

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

		public string AddProjectToTmAndGetProjectName(string tmName)
		{
			Logger.Debug(string.Format("Добавить группу проектов к ТМ {0} и вернуть имя добаленной группы", tmName));
			
			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			TMPage.AssertionEditTMFormIsOpen();
			TMPage.ClickToProjectsListAtTmEdditForm();

			// выбираем первую в списке группу проектов и возвращаем ее имя
			var projectGroupName = getProjectGroup(tmName);

			TMPage.ClickEditSaveBtn();

			// Ответ формы
			// TODO убрать sleep
			Thread.Sleep(2000);

			return projectGroupName;
		}

		public void EditTMAddTargetLanguage(
			string tmName, 
			CommonHelper.LANGUAGE languagetToAdd)
		{
			Logger.Debug(string.Format("Добавить язык перевода {0} к ТМ {1}", languagetToAdd, tmName));

			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			TMPage.AssertionEditTMFormIsOpen();
			TMPage.ClickToTargetLanguagesAtTmEdditForm();
			TMPage.SelectTargetLanguage(languagetToAdd);
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			// TODO убрать sleep
			Thread.Sleep(2000);
		}

		public void FillCommentForm(
			string tmName, 
			string comment)
		{
			Logger.Debug(string.Format("Заполнить поле комменатрия для ТМ {0}. Комментарий: {1}", tmName, comment));

			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			TMPage.AssertionEditTMFormIsOpen();
			TMPage.EditTMClearComment();
			TMPage.InputEditTMComment(comment);
			TMPage.ClickEditSaveBtn();

			// Ответ формы
			Thread.Sleep(2000);
		}
		#endregion

		#region Методы поиска/подсчета объектв на странице

		public bool GetIsExistTmInListDuringProjectCreation(
			string tmName,
			bool isNeedChangeLanguages = false,
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian)
		{
			Logger.Debug(string.Format("Получить, есть ли TM {0} в списке при создании проекта. Менять язык: {1}", tmName, isNeedChangeLanguages));

			SwitchWorkspaceTab();

			if (isNeedChangeLanguages)
			{
				FirstStepProjectWizard(ProjectUniqueName, false, srcLang, trgLang);
			}
			else
			{
				FirstStepProjectWizard(ProjectUniqueName);
			}

			// Переход в настройки Workflow
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Переход в настройки TM
			WorkspaceCreateProjectDialog.ClickNextStep();

			return WorkspaceCreateProjectDialog.GetIsExistTM(tmName);
		}

		public bool GetIsCommentExist(string tmName, string comment)
		{
			Logger.Debug(string.Format("Получить, существет ли для ТМ {0} комментарий {1}", tmName, comment));

			OpenTMInfo(tmName);

			return TMPage.GetIsCommentExist(comment);
		}

		public void CheckProjectExistForTm(string tmName, string projectName)
		{
			Logger.Debug(string.Format("Проверить, что для ТМ {0} указан проект {1}", tmName, projectName));

			OpenTMInfo(tmName);

			Assert.IsTrue(TMPage.GetIsProjectExistInTmInformation(projectName),
				string.Format("Ошибка: неверно указан проект для ТМ {0}.", tmName));
		}

		public int GetSegmentCount(string tmName)
		{
			Logger.Debug(string.Format("Получить количество сегментов для ТМ {0}", tmName));

			OpenTMInfo(tmName);

			return TMPage.GetSegmentCount();
		}

		#endregion

		#region Методы экспорта/импорта ТМХ
		public void MoveTMFile()
		{
			Logger.Debug("Экспортировать ТМХ (работа с внешним диалогом)");

			//Ждём появления загружаемого файла 30 секунд.
			var files=GetDownloadFiles("TestTM*", 30, PathProvider.ResultsFolderPath);

			Assert.IsTrue(files.Length > 0,
				"Ошибка: файл не загрузился за отведённое время (30 секунд)");
			
			var resultPath = Path.Combine(PathProvider.ResultsFolderPath, "TMExportTest");
			Directory.CreateDirectory(resultPath);

			var newFileName = DateTime.Now.Ticks.ToString();
			resultPath = Path.Combine(resultPath, newFileName + ".tmx");

			File.Move(files[0], resultPath);
			Assert.IsTrue(File.Exists(resultPath), "Ошибка: файл не экспортировался\n" + resultPath);
		}
		#endregion

		#region Методы создания фильтров ТМ

		/// <summary>
		/// Метод создания нового фильтра
		/// </summary>
		public void CreateNewTmFilter(
			Action applyingFilter, 
			bool clearFilters = true,
			bool cancelFilterCreation = false
			)
		{
			Logger.Debug(string.Format("Создание нового фильтра. Отчистить фильтры: {0}, отменить создание фильтров: {1}",
				clearFilters, cancelFilterCreation));

			TMPage.OpenTmFilters();

			if (clearFilters)
			{
				TMPage.ClearTmFilters();
			}

			applyingFilter();

			if (cancelFilterCreation)
			{
				TMPage.CancelTmFiltersCreation();
				return;
			}

			TMPage.ApplyTmFilters();
		}

		public void RemoveFilterfromTmPanel(string tmFilterName, string tmFilterValue)
		{
			Logger.Debug(string.Format("Удаление фильтра {0} с панели фильтров ТМ", tmFilterName));

			var fullTextForfilter = string.Format("{0}: {1}", tmFilterName, tmFilterValue);

			TMPage.RemoveTmFilterFromPanel(fullTextForfilter);
		}

		public void CreateSourceLanguageFilter(CommonHelper.LANGUAGE language)
		{
			Logger.Debug(string.Format("Создание фильтра по исходному языку {0}", language));

			TMPage.OpenSourceLanguagesTmFilters();
			TMPage.SelectSourceLanguageTmFilter(language);
		}

		public void CreateTargetLanguageFilter(CommonHelper.LANGUAGE language)
		{
			Logger.Debug(string.Format("Создание фильтра по языку перевода {0}", language));

			TMPage.OpenTargetLanguagesTmFilters();
			TMPage.SelectTargetLanguageTmFilter(language);
		}

		public void CreateAutorFilter(string authorName)
		{
			Logger.Debug(string.Format("Создание фильтра по автору {0}", authorName));

			TMPage.OpenAuthorsTmFilters();
			TMPage.SelectAuthorTmFilter(authorName);
		}

		public void CreateCreationDateFilter(DateTime creationDate)
		{
			Logger.Debug(string.Format("Создание фильтра по дате создания TM {0}", creationDate));
			TMPage.SetCreationDateTmFilters(creationDate);
		}

		public void CreateTopicFilter(string topicName)
		{
			Logger.Debug(string.Format("Создание фильтра по топику TM {0}", topicName));

			TMPage.OpenTopicsTmFilters();
			TMPage.SelectTopicTmFilter(topicName);
		}

		public void CreateProjectGroupFilter(string projectGroupName)
		{
			Logger.Debug(string.Format("Создание фильтра по проектной группе {0}", projectGroupName));

			TMPage.OpenProjectGroupTmFilters();
			TMPage.SelectProjectGroupTmFilter(projectGroupName);
		}

		public void CreateClientFilter(string clientName)
		{
			Logger.Debug(string.Format("Создание фильтра по клиенту {0}", clientName));

			TMPage.OpenClientsTmFilters();
			TMPage.SelectClientTmFilter(clientName);
		}
		#endregion

		public string UniqueTmName { get; set; }

		public static readonly string[] ImportTMXFileList = Directory.GetFiles(PathProvider.TMTestFolder);

		#region Private методы
		
		private void SelectSourceAndTargetLang(
			CommonHelper.LANGUAGE sourceLang = CommonHelper.LANGUAGE.English,
			CommonHelper.LANGUAGE targetLang = CommonHelper.LANGUAGE.Russian,
			bool isMultilanguageTm = false)
		{
			Logger.Debug(string.Format("Выбрать исходный язык {0} и язык перевод {1}", sourceLang, targetLang));

			TMPage.ClickOpenSourceLangList();
			TMPage.SelectSourceLanguage(sourceLang);
			TMPage.ClickTargetLangList();
			TMPage.SelectTargetLanguage(targetLang);

			// Если передан флаг, что в машине необходимо выбрать 
			// несколько языков перевода, выбираем еще один язык
			if (isMultilanguageTm)
			{
				TMPage.SelectTargetLanguage(CommonHelper.LANGUAGE.Lithuanian);
			}

			TMPage.ClickTargetLangList();
		}

		private string GetUniqueTMName()
		{
			// Создать уникальное имя для ТМ без проверки существова
			return ConstTMName + DateTime.Now;
		}

		private string getProjectGroup(string tmName)
		{
			Logger.Debug(string.Format("Выбрать первую проектную группу (или создать, если не найдена) и вернуть ее имя для ТМ {0}", tmName));

			if (!TMPage.IsAnyProjectGroupExist())
			{
				TMPage.ClickCanselOnEditionForm();

				GoToUrl(RelativeUrlProvider.Domains);
				CreateDomain("SingleDomain");

				GoToUrl(RelativeUrlProvider.TranslationMemories);
				ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

				TMPage.AssertionEditTMFormIsOpen();
				TMPage.ClickToProjectsListAtTmEdditForm();
			}

			return TMPage.EditTMAddProject();
		}

		#endregion
	}
}
