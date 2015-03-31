using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMEditingTests: TMTest 
	{
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
			Logger.Info(string.Format("Начало работы теста CreateTMWithExistingNameTest(). Значение параметра tmxFileExisting: {0}", tmxFileExisting));
			
			createNewTmForTest(tmxFileExisting);
			// Изменить имя на пустое и сохранить
			EditTMName(UniqueTmName, "");

			// Проверить, что появилось сообщение об ошибке в имени
			TMPage.AssertionIsExistEditErrorNoNameAppear();
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на существующее
		/// </summary>
		[Test]
		public void EditTMSaveExistingNameTest()
		{
			Logger.Info("Начало работы теста EditTMSaveExistingNameTest().");

			CreateTMIfNotExist(ConstTMName);
			CreateTMByNameAndSave(UniqueTmName);

			// Изменить имя на существующее и сохранить
			EditTMName(UniqueTmName, ConstTMName);

			// Проверить, что появилось сообщение об ошибке в имени
			TMPage.AssertionIsErrorExistingNameAppear();
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на новое
		/// </summary>
		[Test]
		public void EditTMSaveUniqueNameTest()
		{
			Logger.Info("Начало работы теста EditTMSaveExistingNameTest().");

			CreateTMIfNotExist(ConstTMName);
			EditTMName(ConstTMName, UniqueTmName);

			RefreshPage();

			// Проверить, что ТМ со старым именем удалился, а с новым именем есть
			Assert.IsFalse(TMPage.GetIsExistTM(ConstTMName), "Ошибка: не удалилось старое имя");
			Assert.IsTrue(TMPage.GetIsExistTM(UniqueTmName), "Ошибка: нет ТМ с новым именем");
		}

		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на пробельное
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMSaveWhiteSpacesNameTest(TmxFileExisting tmxFileExisting)
		{
			Logger.Info(string.Format("Начало работы теста EditTMSaveWhiteSpacesNameTest(). Значение параметра tmxFileExisting: {0}", tmxFileExisting));

			createNewTmForTest(tmxFileExisting);
			// Изменить имя на пробельное и сохранить
			EditTMName(UniqueTmName, "     ");

			TMPage.AssertionIsExistEditErrorNoNameAppear();
		} 

		/// <summary>
		/// Редактирование имени ТМ: проверка изменения имени в визарде проектов
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTMNameAndCheckChangesOnProjectWizard(TmxFileExisting tmxFileExisting)
		{
			Logger.Info(string.Format("Начало работы теста EditTMNameAndCheckChangesOnProjectWizard(). Значение параметра tmxFileExisting: {0}", tmxFileExisting));

			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			UniqueTmName = string.Concat("!", UniqueTmName);

			createNewTmForTest(tmxFileExisting);

			Assert.IsTrue(
				GetIsExistTmInListDuringProjectCreation(UniqueTmName, true),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта.", UniqueTmName));

			WorkspaceCreateProjectDialog.ClickCloseDialog();

			SwitchTMTab();

			// Измененное имя ТМ
			var tmChangedName = UniqueTmName + "_changed";

			// Изменить имя на уникальное и сохранить
			EditTMName(UniqueTmName, tmChangedName);

			Assert.IsTrue(
				GetIsExistTmInListDuringProjectCreation(tmChangedName, true),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта.", tmChangedName));

			WorkspaceCreateProjectDialog.ClickCloseDialog();

			Assert.IsFalse(
				GetIsExistTmInListDuringProjectCreation(UniqueTmName, true),
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
			Logger.Info(string.Format("Начало работы теста EditTMComment(). Значение параметра tmxFileExisting: {0}", tmxFileExisting));

			createNewTmForTest(tmxFileExisting);
			FillCommentForm(UniqueTmName, InitialComment);

			Assert.IsTrue(
				GetIsCommentExist(UniqueTmName, InitialComment),
					string.Format(
						"Ошибка: комментарий {0} не найден для ТМ {1}.", 
						InitialComment, 
						UniqueTmName));

			FillCommentForm(UniqueTmName, FinalComment);

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
			Logger.Info("Начало работы теста EditTmLanguages().");

			CreateTMByNameAndSave(UniqueTmName);

			TMPage.AssertionIsCorrectLanguagesForTm(
				UniqueTmName,
				EnglishLanguage,
				new[] {RussianLanguage});

			EditTMAddTargetLanguage(UniqueTmName, CommonHelper.LANGUAGE.Lithuanian);

			TMPage.AssertionIsCorrectLanguagesForTm(
				UniqueTmName,
				EnglishLanguage,
				new[]
				{
					LithuanianLanguage,
					RussianLanguage
				});
		}

		/// <summary>
		/// Редактирование проектов ТМ
		/// </summary>
		[TestCase(TmxFileExisting.TmWithTmxFile)]
		[TestCase(TmxFileExisting.TmWithoutTmxFile)]
		[Test]
		public void EditTmProjects(TmxFileExisting tmxFileExisting)
		{
			Logger.Info(string.Format("Начало работы теста EditTmProjects(). Значение параметра tmxFileExisting: {0}", tmxFileExisting));

			createNewTmForTest(tmxFileExisting);

			CheckProjectExistForTm(UniqueTmName, string.Empty);

			// Добавляем проект ProjectName дл ТМ и возвращаем имя группы проектов
			var projectGroupName = AddProjectToTmAndGetProjectName(UniqueTmName);

			CheckProjectExistForTm(UniqueTmName, projectGroupName);
		}

		private void createNewTmForTest(TmxFileExisting tmWithTmxUploading)
		{
			Logger.Debug("Создание новой ТМ для теста");

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
