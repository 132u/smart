using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Collections.Generic;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Glossary
{
	/// <summary>
	/// Группа тестов для общей проверки глоссриев
	/// </summary>
	[Category("Standalone")]
	public class GlossaryMainTest<TWebDriverSettings> : GlossaryTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования создания Глоссария
		/// </summary>
		[Test]
		public void CreateGlossaryTest()
		{
			// Создать новый глоссарий
			var glossaryName = CreateGlossaryAndReturnToGlossaryList();

			// Проверить, что глоссарий сохранился
			Assert.IsTrue(GlossaryListPage.GetIsExistGlossary(glossaryName), "Ошибка: глоссарий не создался");
		}

		/// <summary>
		/// Метод тестирования создания Глоссария без имени
		/// </summary>
		[Test]
		public void CreateGlossaryWithoutNameTest()
		{
			// Открыть форму создания глоссария
			OpenCreateGlossary();
			// Нажать сохранить
			GlossaryEditForm.ClickSaveGlossary();

			// Проверить, что появилось сообщение о пустом имени
			GlossaryEditForm.AssertionIsExistErrorMessageEmptyGlossaryName();
		}

		/// <summary>
		/// Метод тестирования создания Глоссария с существующим именем
		/// </summary>
		[Test]
		public void CreateGlossaryWithExistingNameTest()
		{
			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Перейти в список глоссариев
			SwitchGlossaryTab();
			// Создать глоссарий с этим же именем
			CreateGlossaryByName(glossaryName, false);

			// Проверить, что появилось сообщение о существующем имени
			GlossaryEditForm.AssertionIsExistErrorMessageExistGlossaryName();
		}

		/// <summary>
		/// Метод: проверка отсутствия уже выбранного языка в выпадающем списке при выборе нового языка (при создании глоссария)
		/// </summary>
		[Test]
		public void CheckAbsenceSelectedLanguageAmongAvailableTest()
		{
			// Открыть форму создания глоссария
			OpenCreateGlossary();

			// Добавить язык (de)
			GlossaryEditForm.ClickAddLanguage();
			GlossaryEditForm.ClickLastLangOpenCloseList();
			GlossaryEditForm.SelectLanguage(CommonHelper.LANGUAGE.German);

			// Нажать кнопку добавления языка
			GlossaryEditForm.ClickAddLanguage();
			GlossaryEditForm.ClickLastLangOpenCloseList();

			// Проверить, что языка нет в списке для добавления языка
			GlossaryEditForm.AssertionIsLanguageInLangListNotExist(CommonHelper.LANGUAGE.German);
		}

		/// <summary>
		/// Метод тестирования удаления языка при добавлении глоссария
		/// </summary>
		[Test]
		public void DeleteLanguageCreateGlossaryTest()
		{
			// Открыть форму создания глоссария
			OpenCreateGlossary();
			// Получить количество выбранных языков
			var selectedLangCountBefore = GlossaryEditForm.GetGlossaryLanguageCount();
			// Удалить язык
			GlossaryEditForm.ClickDeleteLanguage();
			// Получить количество выбранных языков
			var selectedLangCountAfter = GlossaryEditForm.GetGlossaryLanguageCount();

			// Проверить, что количество уменьшилось
			Assert.IsTrue(
				selectedLangCountAfter < selectedLangCountBefore, 
				"Ошибка: количество языков не уменьшилось!");
		}

		/// <summary>
		/// Метод проверки даты создания при добавлении глоссария
		/// </summary>
		[Test]
		public void CheckDateCreatedGlossaryTest()
		{
			// Получить текущую дату
			var todayDate = DateTime.Now;
			// Создать новый глоссарий
			var glossaryName = CreateGlossaryAndReturnToGlossaryList();

			// Сравнить дату создания с текущей датой
			Assert.IsTrue(
			GetIsDateEqualCurrentDayOrToday(
				GlossaryListPage.GetGlossaryDateModified(glossaryName), 
				todayDate), 
			"Ошибка: дата создания глоссари и дата в таблице на странице списка глоссари не совпадают");
		}

		/// <summary>
		/// Метод проверки даты и времени изменения глоссария
		/// </summary>
		[Test]
		public void CheckDateModifyGlossaryTest()
		{
			// Создать новый глоссарий
			var glossaryName = CreateGlossaryAndReturnToGlossaryList();
			// Получить дату и время создания глоссария
			var dateModifiedBefore = GlossaryListPage.GetGlossaryDateModified(glossaryName);
			// Перейти в глоссарий
			SwitchCurrentGlossary(glossaryName);
			// Поставить задержку минуту, чтобы дата изменения изменилась (точность даты и времени до минуты)
			Thread.Sleep(60000);
			// Создать термин (изменение глоссария)
			CreateItemAndSave();

			GlossaryPage.AssertionConceptGeneralSave();

			// Перейти к списку глоссариев
			SwitchGlossaryTab();
			// Получить дату и время изменения глоссария
			var dateModifiedAfter = GlossaryListPage.GetGlossaryDateModified(glossaryName);

			// Сравнить дату с предыдущей датой
			Assert.IsTrue(
				dateModifiedBefore != dateModifiedAfter, 
				"Ошибка: дата изменения глоссария не изменилась");
		}

		/// <summary>
		/// Метод проверки автора при добавлении глоссария
		/// </summary>
		[Test]
		public void CheckAuthorLastModificationGlossaryTest()
		{
			// Получить имя пользователя из панели WS 
			var userName = WorkspacePage.GetUserName();
			// Создать глоссарий
			var glossaryName = CreateGlossaryAndReturnToGlossaryList();
			// Получить имя автора глоссария
			var authorName = GlossaryListPage.GetGlossaryModificationAuthor(glossaryName);

			// Проверить, что текущий пользователь и есть автор
			Assert.IsTrue(
				userName.Contains(authorName), 
				"Ошибка: автор нового глоссария - не текущий пользователь");
		}

		/// <summary>
		/// Метод тестирования удаления Глоссария
		/// </summary>
		[Test]
		public void DeleteGlossaryTest()
		{
			// Создать  глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Нажать Удалить глоссарий 
			GlossaryEditForm.ClickDeleteGlossary();

			// Проверить, что появилась кнопка подтверждения удаления глоссария
			GlossaryEditForm.WaitUntilDeleteGlossaryButtonDisplay();

			// Нажать Да (удалить)
			GlossaryEditForm.ClickConfirmDeleteGlossary();

			// Проверить, что глоссария нет
			Assert.IsTrue(
				!GlossaryListPage.GetIsExistGlossary(glossaryName), 
				"Ошибка: глоссарий не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления языка, когда уже есть термин на этом языке
		/// </summary>
		[Test]
		public void DeleteLanguageExistTermTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Создать термин
			CreateItemAndSave();
			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Получить количество языков
			var availLanguageCountBefore = GlossaryEditForm.GetGlossaryLanguageCount();
			// Удалить язык
			GlossaryEditForm.ClickDeleteLanguage();

			GlossaryEditForm.AssertionIsExistWarningDeleteLanguage();

			// Отменить удаление
			GlossaryEditForm.CancelDeleteLanguage();

			// Получить количество языков
			var availLanguageCountAfter = GlossaryEditForm.GetGlossaryLanguageCount();

			// Сравнить количество языков
			Assert.AreEqual(
				availLanguageCountBefore, 
				availLanguageCountAfter, 
				"Ошибка: количество языков разное (должно быть одинаковое, т.к. удаление отменили)");
		}

		/// <summary>
		/// Метод тестирования изменения структуры
		/// </summary>
		[Test]
		public void EditGlossaryStructureTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Добавить элемент в структуре глоссария
			EditGlossaryStructureAddField();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();

			// Проверить, что появилось поле добавления нового термина в расширенном виде
			GlossaryPage.AssertionIsExistNewItemExtendedMode();
		}

		/// <summary>
		/// Метод тестирования импорта глоссария с добавлением терминов
		/// </summary>
		[Test]
		public void ImportGlossaryAddConceptsTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Открыть Импорт, указать документ для импорта
			FillImportGlossaryForm();
			// Нажать Импорт
			GlossaryPage.ClickImportFormImportBtn();
			// Дождаться закрытия формы импорта
			GlossaryPage.AssertionImportFormDisappear();
			// Нажать Закрыть в сообщении об успешном добавлении
			GlossaryPage.ClickCloseSuccessResult();
			// TODO убрать sleep
			Thread.Sleep(1000);

			// Проверить, что количество терминов больше нуля
			Assert.IsTrue(
				GlossaryPage.GetConceptCount() > 0, 
				"Ошибка: количество терминов должно быть больше нуля");
		}

		/// <summary>
		/// Метод тестирования импорта глоссария с заменой всех терминов
		/// </summary>
		[Test]
		public void ImportGlossaryReplaceAllConceptsTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Создать термины
			CreateItemAndSave();
			CreateItemAndSave();

			Assert.IsTrue(
				GlossaryPage.GetConceptCount() == 2, 
				"Ошибка: должно сохраниться 2 термина");

			// Открыть Импорт, указать документ для импорта
			FillImportGlossaryForm();
			// Указать "Заменить все термины"
			GlossaryPage.ClickReplaceAll();
			// Нажать Импорт
			GlossaryPage.ClickImportFormImportBtn();
			// Дождаться закрытия формы импорта
			GlossaryPage.AssertionImportFormDisappear();

			// Нажать Закрыть в сообщении об успешном добавлении
			GlossaryPage.WaitUntilCloseSuccessButtonDisplay();
			GlossaryPage.ClickCloseSuccessResult();
			// TODO убрать sleep
			Thread.Sleep(1000);

			// Проверить, что количество терминов изменилось (количество терминов должно быть равно 1 для этого импортируемого файла)
			Assert.IsTrue(
				GlossaryPage.GetConceptCount() == 1, 
				"Ошибка: количество терминов должно быть равно 1");
		}

		/// <summary>
		/// Метод тестирования Экспорта глоссария
		/// </summary>
		[Test]
		public void ExportGlossaryTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);
			// Создать термины
			CreateItemAndSave();
			CreateItemAndSave();

			var uniqueGlossaryName = Path.Combine(PathProvider.ResultsFolderPath, glossaryName.Replace(":", "-"));
			// Экспорт глоссари
			GlossaryPage.ClickExportGlossary();

			CommonHelper.AssertionFileDownloaded(uniqueGlossaryName + ".xlsx");
		}

		/// <summary>
		/// Метод тестирования изменения названия глоссария
		/// </summary>
		[Test]
		public void ChangeGlossaryNameTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Получить уникальное имя для глоссария
			var uniqueGlossaryName = GetUniqueGlossaryName();

			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Изменить имя глоссария и сохранить
			ChangeGlossaryNameToCurrent(uniqueGlossaryName);
			// Перейти к списку глоссариев
			SwitchGlossaryTab();

			// Проверить, что нет глоссария со старым именем
			Assert.IsTrue(
				!GlossaryListPage.GetIsExistGlossary(glossaryName), 
				"Ошибка: старое имя глоссария не удалилось");
			// Проверить, что появился глоссарий с новым именем
			Assert.IsTrue(
				GlossaryListPage.GetIsExistGlossary(uniqueGlossaryName), 
				"Ошибка: новое имя глоссария не появилось");
		}

		/// <summary>
		/// Метод тестирования изменения названия глоссария на существующее
		/// </summary>
		[Test]
		public void ChangeGlossaryExistingNameTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Перейти к списку глоссариев
			SwitchGlossaryTab();

			// Создать другой глоссарий
			var secondGlossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(secondGlossaryName);

			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Изменить имя глоссария и сохранить
			ChangeGlossaryNameToCurrent(glossaryName);

			// Проверить, что появилось сообщение о существующем имени
			GlossaryEditForm.AssertionIsExistErrorMessageExistGlossaryName();
		}

		/// <summary>
		/// Метод тестирования изменения названия глоссария на пустое
		/// </summary>
		[Test]
		public void ChangeGlossaryEmptyNameTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Ввести имя
			ChangeGlossaryNameToCurrent("");

			// Проверить, что появилось сообщение о пустом имени
			GlossaryEditForm.AssertionIsExistErrorMessageEmptyGlossaryName();
		}

		/// <summary>
		/// Метод тестирования изменения названия глоссария на пробельное
		/// </summary>
		[Test]
		public void ChangeGlossarySpaceNameTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();
			// Изменить имя глоссария и сохранить
			ChangeGlossaryNameToCurrent(" ");

			// Проверить, что появилось сообщение о пустом имени
			GlossaryEditForm.AssertionIsExistErrorMessageEmptyGlossaryName();
		}

		/// <summary>
		/// Тест: Открыть редактор структуры глоссария из редактора свойств
		/// </summary>
		[Test]
		public void OpenStructureFromPropertiesTest()
		{
			// Создать глоссарий
			var glossaryName = GetUniqueGlossaryName();
			CreateGlossaryByName(glossaryName);

			// Открыть редактирование свойств глоссария
			OpenGlossaryProperties();

			// Нажать Изменить структуру
			GlossaryEditForm.ClickSaveAndEditStructureBtn();
			GlossaryEditForm.AssertionPageClose();

			// Проверить, что открылся редактор структуры
			GlossaryPage.WaitPageLoad();
		}

		/// <summary>
		/// Тест: создать глоссарий с несколькими языками
		/// </summary>
		[Test]
		public void CreateMultiLanguageGlossary()
		{
			// Имя глоссария
			var glossaryName = "TestGlossary" + DateTime.Now.Ticks;

			// Список языков
			var langList = new List<CommonHelper.LANGUAGE>
			{
				CommonHelper.LANGUAGE.German,
				CommonHelper.LANGUAGE.French,
				CommonHelper.LANGUAGE.Japanese,
				CommonHelper.LANGUAGE.Lithuanian
			};

			// Создать глоссарий
			CreateGlossaryByName(glossaryName, true, langList);
			SwitchGlossaryTab();

			// Проверить, что глоссарий добавился в список
			Assert.IsTrue(GlossaryListPage.GetIsExistGlossary(glossaryName), 
				"Ошибка: глоссарий не добавился" + glossaryName);

			// Зайти в глоссарий
			SwitchCurrentGlossary(glossaryName);
			// Удалить глоссарий
			DeleteGlossary();
		}



		/// <summary>
		/// Изменить имя глоссария
		/// </summary>
		/// <param name="glossaryName">новое название</param>
		protected void ChangeGlossaryNameToCurrent(string glossaryName)
		{
			// Ввести имя глоссария
			GlossaryEditForm.EnterGlossaryName(glossaryName);
			// Сохранить
			GlossaryEditForm.ClickSaveGlossary();
			// TODO убрать sleep
			Thread.Sleep(1000);
		}

		/// <summary>
		/// Заполнить форму импорт глоссария
		/// </summary>
		protected void FillImportGlossaryForm()
		{
			// Нажать Импорт
			GlossaryPage.ClickImportBtn();
			// Дождаться открытия формы
			GlossaryPage.WaitImportForm();

			// Заполнить форму загрузки документа
			GlossaryPage.UploadTerm(PathProvider.ImportGlossaryFile);
		}

		/// <summary>
		/// Вернуть, равна ли дата переданной дате или равна дате-сегодня (переданная строка содержит дату, которая равна переданной дате или сегодня)
		/// </summary>
		/// <param name="dateTimeString">строка с датой</param>
		/// <param name="curDay">дата для сравнения</param>
		/// <returns>Равна</returns>
		protected bool GetIsDateEqualCurrentDayOrToday(string dateTimeString, DateTime curDay)
		{
			// Формат dateTimeString: M(M)/D(D)/YYYY H(H):M(M) AM
			var beginIndex = 0;
			var splitIndex = dateTimeString.IndexOf("/");
			// Месяц
			var month = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

			beginIndex = splitIndex + 1;
			splitIndex = dateTimeString.IndexOf("/", beginIndex);

			// День
			var day = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

			beginIndex = splitIndex + 1;
			splitIndex = dateTimeString.IndexOf(" ", beginIndex);

			// Год
			var year = dateTimeString.Substring(beginIndex, splitIndex - beginIndex);

			// Создать дату в стандартном формате
			var resDate = DateTime.ParseExact(day + "/" + month + "/" + year, "d/M/yyyy", new CultureInfo("en-US")).Date;
			Logger.Trace("время в таблице resDate = " + resDate);
			// Явное приведение к строке стоит, чтобы не падал ArgumentOutOfRangeException. (неявное приведение даты иногда не отрабатывает корректно)
			Logger.Trace("время создания глоссари curDay.Date = " + curDay.Date.ToString());
			Logger.Trace("сегодняшняя дата если тест проходит в полночь DateTime.Today.Date = " + DateTime.Today.Date);
			// Сравнить с текущей датой или с сегодня (если тест проходит в полночь)
			return curDay.Date == resDate || DateTime.Today.Date == resDate;
		}

		/// <summary>
		/// Получить имя пользователя из профиля
		/// </summary>
		/// <returns>имя</returns>
		protected string GetUserNameProfile()
		{
			var userName = "";
			// Нажать на Профиль
			MainHelperClass.OpenProfile();
			// Дождаться открытия окна с профилем
			MainHelperClass.WaitProfileOpen();
			// Получить имя пользователя
			userName = MainHelperClass.GetUserNameProfile();
			// Закрыть профиль
			MainHelperClass.CloseProfile();

			return userName;
		}
	}
}
