using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMCreationTests : TMTest 
	{
		public TMCreationTests(string browserName) 
			: base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования создания ТМ (без TMX)
		/// </summary>
		[Test, TestCaseSource("TmNamesList")]
		public void CreateNewTMTest(string tmName)
		{
			Logger.Info("Начало работы теста CreateNewTMTest().");

			var uniqueTMName = GetUniqueTMName(tmName);
			CreateTMByNameAndSave(uniqueTMName);

			Assert.IsTrue(TMPage.GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");
			Assert.IsTrue(GetSegmentCount(uniqueTMName) == 0, "Ошибка: количество сегментов должно быть равно 0");
		}

		/// <summary>
		/// Метод тестирования отмены создания новой ТМ
		/// </summary>
		[Test]
		public void CancelNewTMCreation()
		{
			Logger.Info("Начало работы теста CancelNewTMCreation().");

			TMPage.OpenCreateTMDialog();
			CreateTMByName(UniqueTmName);
			TMPage.ClickCancelSavingNewTM();

			Assert.IsFalse(
				TMPage.GetIsExistTM(UniqueTmName),
				string.Format("Ошибка: ТМ {0} имеется в списке после операции отмены сохранения", UniqueTmName));
		}

		/// <summary>
		/// Метод тестирования создания ТМ без имени
		/// </summary>
		[Test]
		public void CreateTMWithoutNameTest()
		{
			Logger.Info("Начало работы теста CreateTMWithoutNameTest().");

			TMPage.OpenCreateTMDialog();
			TMPage.ClickSaveNewTM();

			TMPage.AssertionNoNameErrorAppearDuringTmCreation();
		}

		/// <summary>
		/// Метод тестирования создания ТМ без указания языков
		/// </summary>
		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			Logger.Info("Начало работы теста CreateTMWithoutLanguageTest().");

			TMPage.OpenCreateTMDialog();
			TMPage.InputNewTMName(UniqueTmName);
			TMPage.ClickSaveNewTM();

			TMPage.AssertionIsNoTargetErrorAppearDuringTmCreation();
		}

		/// <summary>
		/// Метод тестирования создания ТМ с существующим именем
		/// </summary>
		[Test]
		public void CreateTMWithExistingNameTest()
		{
			Logger.Info("Начало работы теста CreateTMWithExistingNameTest().");

			CreateTMIfNotExist(ConstTMName);
			TMPage.OpenCreateTMDialog();
			CreateTMByName(ConstTMName);
			TMPage.ClickSaveNewTM();
			
			TMPage.AssertionIsExistNameErrorAppearDuringTmCreation();
		}

		/// <summary>
		/// Метод тестирования создания ТМ с загрузкой НЕ(!) TMX файла
		/// </summary>
		[Test]
		public void CreateTMWithNotTMXTest()
		{
			Logger.Info("Начало работы теста CreateTMWithNotTMXTest().");

			CreateTMWithUploadTMX(UniqueTmName, PathProvider.DocumentFile, needToSave: false);

			Assert.IsTrue(TMPage.GetIsErrorMessageNotTmx(),
				"Ошибка: не появилось сообщение о неверном расширении файла");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с несколькими языками перевода
		/// </summary>
		[Test]
		public void CreateMultilanguageTM()
		{
			Logger.Info("Начало работы теста CreateMultilanguageTM().");

			//Создать ТМ с двумя языками перевода 
			//(языки перевода по умолчанию source - English, target - Russian, Lithuanian)
			CreateTMByNameAndSave(UniqueTmName, isMultilanguageTm: true);

			// Перейти на вкладку Workspace и проверить, 
			// что TM есть в списке при создании проекта (языки перевода по умолчанию)
			Assert.IsTrue(
				GetIsExistTmInListDuringProjectCreation(UniqueTmName, true),
				"Ошибка: ТМ нет в списке при создании проекта");

			// Закрыть диалог создания проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();

			// Перейти на вкладку Workspace и проверить, 
			// что TM есть в списке при создании проекта (языки перевода English - Lithuanian)
			Assert.IsTrue(
				GetIsExistTmInListDuringProjectCreation(
					UniqueTmName,
					isNeedChangeLanguages: true,
					srcLang: CommonHelper.LANGUAGE.English,
					trgLang: CommonHelper.LANGUAGE.Lithuanian),
				string.Format("Ошибка: ТМ {0} нет в списке при создании проекта", UniqueTmName));
		}
		
		private static readonly string[] TmNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}
