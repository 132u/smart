using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMEditingTests: TMTest 
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public TMEditingTests(string browserName)
			: base(browserName)
		{
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на пустое
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMSaveWithoutNameTest(TmxFileExisting tmxFileExisting)
		{
			// Создать ТМ с/без ТМХ файла
			CrateNewTmForTest(tmxFileExisting);

			// Изменить имя на пустое и сохранить
			EditTMFillName(UniqueTmName, "");

			// Проверить, что появилось сообщение об ошибке в имени
			Assert.IsTrue(TMPage.GetIsExistEditErrorNoName(),
				"Ошибка: не появилось сообщение о пустом имени");
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на существующее
		/// </summary>
		[Test]
		public void EditTMSaveExistingNameTest()
		{
			// Создать ТМ с таким именем, если его еще нет
			CreateTMIfNotExist(ConstTMName);

			// Создать ТМ
			CreateTMByNameAndSave(UniqueTmName);

			// Изменить имя на существующее и сохранить
			EditTMFillName(UniqueTmName, ConstTMName);

			// Проверить, что появилось сообщение об ошибке в имени
			Assert.IsTrue(TMPage.GetIsExistEditErrorExistName(),
				"Ошибка: не появилось сообщение об ошибке в имени");
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на новое
		/// </summary>
		[Test]
		public void EditTMSaveUniqueNameTest()
		{
			// Создать ТМ с таким именем, если его еще нет
			CreateTMIfNotExist(ConstTMName);

			// Изменить имя на уникальное и сохранить
			EditTMFillName(ConstTMName, UniqueTmName);

			// Перезагрузим страницу
			RefreshPage();

			// Проверить, что ТМ со старым именем удалился, а с новым именем есть
			Assert.IsTrue(!GetIsExistTM(ConstTMName), "Ошибка: не удалилось старое имя");
			Assert.IsTrue(GetIsExistTM(UniqueTmName), "Ошибка: нет ТМ с новым именем");
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на пробельное
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMSaveWhiteSpacesNameTest(TmxFileExisting tmxFileExisting)
		{
			// Создать ТМ с/без ТМХ файла
			CrateNewTmForTest(tmxFileExisting);

			// Изменить имя на существующее и сохранить
			EditTMFillName(UniqueTmName, "     ");

			// Проверить, что появилось сообщение об ошибке в имени
			Assert.IsTrue(TMPage.GetIsExistEditErrorNoName(),
				"Ошибка: не появилось сообщение об ошибке в имени");
		} 

		/// <summary>
		/// Редактирование имени ТМ: проверка изменения имени в визарде проектов
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMNameAndCheckChangesOnProjectWizard(TmxFileExisting tmxFileExisting)
		{
			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			UniqueTmName = string.Concat("!", UniqueTmName);

			// Создать ТМ с именем UniqueTmName
			CrateNewTmForTest(tmxFileExisting);

			Assert.IsTrue(
				GetIsExistTMCreateProjectList(UniqueTmName, true),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта.", UniqueTmName));

			// Перейти на страницу TM
			SwitchTMTab();

			// Измененное имя ТМ
			var tmChangedName = UniqueTmName + "_changed";

			// Изменить имя на уникальное и сохранить
			EditTMFillName(UniqueTmName, tmChangedName);

			Assert.IsTrue(
				GetIsExistTMCreateProjectList(tmChangedName, true),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта.", tmChangedName));

			Assert.IsFalse(
				GetIsExistTMCreateProjectList(UniqueTmName, true),
				string.Format("Ошибка: ТМ {0} имеется в списке при создании проекта, когда ее быть не должно.", UniqueTmName));
		}

		/// <summary>
		/// Редактирования коментария ТМ
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMComment(TmxFileExisting tmxFileExisting)
		{
			// Создать ТМ с/без ТМХ файла
			CrateNewTmForTest(tmxFileExisting);

			// Заполняем поле комментарий строкой _initialComment
			FillingCommentForm(UniqueTmName, InitialComment);

			Assert.IsTrue(
				GetIsCommentExist(UniqueTmName, InitialComment),
					string.Format(
						"Ошибка: комментарий {0} не найден для ТМ {1}.", 
						InitialComment, 
						UniqueTmName));

			// Заполняем поле комментарий строкой _initialComment
			FillingCommentForm(UniqueTmName, FinalComment);

			Assert.IsTrue(
				GetIsCommentExist(UniqueTmName, FinalComment),
					string.Format(
						"Ошибка: комментарий {0} не найден для ТМ {1}.",
						InitialComment,
						FinalComment));
		}

		/// <summary>
		/// Редактирование языков перевода ТМ
		/// </summary>
		[Test]
		public void EditTmLanguages()
		{
			var srcLang = CommonHelper.LANGUAGE.English;
			var trgLang = CommonHelper.LANGUAGE.Russian;

			CreateTMByNameAndSave(UniqueTmName, srcLang, trgLang);

			Assert.IsTrue(
				GetIsCorrectLanguagesForTm(
					UniqueTmName,
					EnglishLanguage,
					new[] { RussianLanguage }),
					"Ошибка: для ТМ неверно отображены исходный язык и язык перевода.");

			EditTMAddTargetLanguage(UniqueTmName, CommonHelper.LANGUAGE.Lithuanian);

			Assert.IsTrue(
				GetIsCorrectLanguagesForTm(
					UniqueTmName,
					EnglishLanguage,
					new[]
					{
						RussianLanguage, 
						LithuanianLanguage
					}),
					"Ошибка: для ТМ неверно отображены исходный язык и язык перевода.");
		}

		/// <summary>
		/// Редактирование проектов ТМ
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTmProjects(TmxFileExisting tmxFileExisting)
		{
			// Создать ТМ с/без ТМХ файла
			CrateNewTmForTest(tmxFileExisting);

			Assert.IsTrue(
				GetIsProjectExistForTm(UniqueTmName, string.Empty),
				"Ошибка: неверно указан проект для ТМ.");

			// Добавляем проект ProjectName дл ТМ и возвращаем имя группы проектов
			var projectGroupName = AddProjectToTmAndGetProjectName(UniqueTmName);

			Assert.IsTrue(
				GetIsProjectExistForTm(UniqueTmName, projectGroupName),
				"Ошибка: неверно указан проект для ТМ.");
		}

		/// <summary>
		/// создать новую ТМ для теста 
		/// </summary>
		private void CrateNewTmForTest(TmxFileExisting tmWithTmxUploading)
		{
			if (tmWithTmxUploading == TmxFileExisting.TmWithoutTmxFile)
			{
				CreateTMIfNotExist(UniqueTmName);
			}
			else
			{
				CreateTMWithUploadTMX(UniqueTmName, ImportTMXFileList[0]);
			}
		}

		public enum TmxFileExisting
		{
			TmWithTmxFile, 
			TmWithoutTmxFile
		}

		private const string InitialComment = "_initialComment";
		private const string FinalComment = "FinalComment";

		private const string RussianLanguage = "ru";
		private const string EnglishLanguage = "en";
		private const string LithuanianLanguage = "lt";

	}
}
