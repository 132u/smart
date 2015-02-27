using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMCreationTests : TMTest 
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
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
			// Выбрать уникальное имя TM
			var uniqueTMName = GetUniqueTMName(tmName);

			// Создать ТМ
			CreateTMByNameAndSave(uniqueTMName);

			// Проверить, сохранился ли ТМ
			Assert.IsTrue(GetIsExistTM(uniqueTMName), "Ошибка: ТМ не сохранился (не появился в списке)");

			// Проверить, что количество сегментов равно 0
			Assert.IsTrue(GetSegmentCount(uniqueTMName) == 0, "Ошибка: количество сегментов должно быть равно 0");
		}

		/// <summary>
		/// Метод тестирования отмены создания новой ТМ
		/// </summary>
		[Test]
		public void CancelNewTMCreation()
		{
			// Открыть форму создания ТМ
			OpenCreateTMForm();

			// Открыть окно создания новой ТМ и заполнить необходимые поля
			CreateTMByName(UniqueTmName);

			// Нажать кнопку "отмена"
			TMPage.ClickCancelSavingNewTM();

			// Проверить, что ТМ с именем UniqueTmName нет в списке
			Assert.IsFalse(
				GetIsExistTM(UniqueTmName),
				string.Format("Ошибка: ТМ {0} имеется в списке после операции отмены сохранения", UniqueTmName));
		}

		/// <summary>
		/// Метод тестирования создания ТМ без имени
		/// </summary>
		[Test]
		public void CreateTMWithoutNameTest()
		{
			// Открыть форму создания ТМ
			OpenCreateTMForm();

			// Нажать кнопку Сохранить
			TMPage.ClickSaveNewTM();

			// Проверить появления сообщения об ошибке
			Assert.IsTrue(TMPage.GetIsExistCreateTMErrorNoName(),
				"Ошибка: не появилось сообщение об ошибке");
		}

		/// <summary>
		/// Метод тестирования создания ТМ без указания языков
		/// </summary>
		[Test]
		public void CreateTMWithoutLanguageTest()
		{
			// Открыть форму создания ТМ
			OpenCreateTMForm();
			// Ввести имя
			TMPage.InputNewTMName(UniqueTmName);
			// Нажать кнопку Сохранить
			TMPage.ClickSaveNewTM();

			// Проверить появления сообщения об ошибке
			Assert.IsTrue(TMPage.GetIsExistCreateTMErrorNoTarget(),
				"Ошибка: не появилось сообщение об ошибке");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с существующим именем
		/// </summary>
		[Test]
		public void CreateTMWithExistingNameTest()
		{
			CreateTMIfNotExist(ConstTMName);
			// Создать ТМ с тем же (уже существующим) именем
			OpenCreateTMForm();
			// Создать ТМ без сохранения формы
			CreateTMByName(ConstTMName);
			// Нажать кнопку Сохранить
			TMPage.ClickSaveNewTM();
			// Проверить появление ошибки
			Assert.IsTrue(TMPage.GetIsExistCreateTMErrorExistName(),
				"Ошибка: не появилась ошибка создания ТМ с существующим именем");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с загрузкой НЕ(!) TMX файла
		/// </summary>
		[Test]
		public void CreateTMWithNotTMXTest()
		{
			// Создать ТМ с загрузкой НЕ(!) TMX файла
			CreateTMWithUploadTMX(UniqueTmName, PathProvider.DocumentFile);

			// Проверить, что появилось сообщение о неверном расширении файла
			Assert.IsTrue(TMPage.GetIsErrorMessageNotTMX(),
				"Ошибка: не появилось сообщение о неверном расширении файла");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с несколькими языками перевода
		/// </summary>
		[Test]
		public void CreateMultilanguageTM()
		{
			//Создать ТМ с двумя языками перевода 
			//(языки перевода по умолчанию source - English, target - Russian, Lithuanian)
			CreateTMByNameAndSave(UniqueTmName, isMultilanguageTm: true);

			// Перейти на вкладку Workspace и проверить, 
			// что TM есть в списке при создании проекта (языки перевода по умолчанию)
			Assert.IsTrue(
				GetIsExistTMCreateProjectList(UniqueTmName, true),
				"Ошибка: ТМ нет в списке при создании проекта");
			// Закрыть диалог создания проекта
			WorkspaceCreateProjectDialog.ClickCloseDialog();
			// Перейти на вкладку Workspace и проверить, 
			// что TM есть в списке при создании проекта (языки перевода English - Lithuanian)
			Assert.IsTrue(
				GetIsExistTMCreateProjectList(
					UniqueTmName, 
					true,
					CommonHelper.LANGUAGE.English,
					CommonHelper.LANGUAGE.Lithuanian),
				"Ошибка: ТМ нет в списке при создании проекта");
		}
		
		private static readonly string[] TmNamesList =
		{
			"TestTM", 
			"Тестовая ТМ", 
			"我喜爱的哈伯尔阿哈伯尔"
		};
	}
}
