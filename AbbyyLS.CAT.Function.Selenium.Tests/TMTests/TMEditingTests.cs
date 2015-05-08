using NUnit.Framework;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMEditingTests<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Тестирование редактирования ТМ: изменение имени на пустое
		/// </summary>
		/// <param name="isNeedTmxUpload">Нужно ли загружать TMX файл при создании ТМ</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void EditTMSaveWithoutNameTest(bool isNeedTmxUpload)
		{
			Logger.Info(string.Format("Начало работы теста CreateTMWithExistingNameTest(). Значение параметра isNeedTmxUpload: {0}", isNeedTmxUpload));
			
			createNewTmForTest(isNeedTmxUpload);
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
		/// <param name="isNeedTmxUpload">Нужно ли загружать TMX файл при создании ТМ</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void EditTMSaveWhiteSpacesNameTest(bool isNeedTmxUpload)
		{
			Logger.Info(string.Format("Начало работы теста EditTMSaveWhiteSpacesNameTest(). Значение параметра isNeedTmxUpload: {0}", isNeedTmxUpload));
			// Закрываем поп-ап сообщения, т.к. селениум не может найти поле поиска в Chrome 
			TMPage.CloseAllErrorNotifications();
			createNewTmForTest(isNeedTmxUpload);
			// Изменить имя на пробельное и сохранить
			EditTMName(UniqueTmName, "     ");

			TMPage.AssertionIsExistEditErrorNoNameAppear();
		} 

		/// <summary>
		/// Редактирование имени ТМ: проверка изменения имени в визарде проектов
		/// </summary>
		/// <param name="isNeedTmxUpload">Нужно ли загружать TMX файл при создании ТМ</param>
		[Category("PRX_9476")]
		[TestCase(true)]
		[TestCase(false)]
		public void EditTMNameAndCheckChangesOnProjectWizard(bool isNeedTmxUpload)
		{
			Logger.Info(string.Format("Начало работы теста EditTMNameAndCheckChangesOnProjectWizard(). Значение параметра isNeedTmxUpload: {0}", isNeedTmxUpload));

			// Для облегчения теста создадим имя ТМ таким, чтобы при создании проекта
			// его можно было выбрать, не прибегая к прокрутке
			UniqueTmName = string.Concat("!", UniqueTmName);

			createNewTmForTest(isNeedTmxUpload);

			Assert.IsTrue(
				GetIsExistTmInListDuringProjectCreation(UniqueTmName, true),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта.", UniqueTmName));

			WorkspaceCreateProjectDialog.ClickCloseDialog();
			WorkspaceCreateProjectDialog.WaitDialogDisappear();
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
		/// <param name="isNeedTmxUpload">Нужно ли загружать TMX файл при создании ТМ</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void EditTMComment(bool isNeedTmxUpload)
		{
			Logger.Info(string.Format("Начало работы теста EditTMComment(). Значение параметра isNeedTmxUpload: {0}", isNeedTmxUpload));

			createNewTmForTest(isNeedTmxUpload);
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
		/// <param name="isNeedTmxUpload">Нужно ли загружать TMX файл при создании ТМ</param>
		[Test]
		[TestCase(true)]
		[TestCase(false)]
		public void EditTmProjects(bool isNeedTmxUpload)
		{
			Logger.Info(string.Format("Начало работы теста EditTmProjects(). Значение параметра isNeedTmxUpload: {0}", isNeedTmxUpload));

			createNewTmForTest(isNeedTmxUpload);

			CheckProjectExistForTm(UniqueTmName, string.Empty);

			// Добавляем проект ProjectName дл ТМ и возвращаем имя группы проектов
			var projectGroupName = AddProjectToTmAndGetProjectName(UniqueTmName);

			CheckProjectExistForTm(UniqueTmName, projectGroupName);
		}

		private void createNewTmForTest(bool isNeedTmxUpload)
		{
			Logger.Debug("Создание новой ТМ для теста");

			if (isNeedTmxUpload)
				CreateTMWithUploadTMX(UniqueTmName, ImportTMXFileList[0]);
			else
				CreateTMIfNotExist(UniqueTmName);
		}

		private const string InitialComment = "_initialComment";
		private const string FinalComment = "FinalComment";

		private const string RussianLanguage = "ru";
		private const string EnglishLanguage = "en";
		private const string LithuanianLanguage = "lt";
	}
}
