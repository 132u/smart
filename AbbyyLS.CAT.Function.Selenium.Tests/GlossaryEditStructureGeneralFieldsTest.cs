using System;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки редактирования структуры глоссария
	/// </summary>
	public class GlossaryEditStructureGeneralFieldsTest : GlossaryTest
	{
		/// <summary>
		/// Конструктор хелпера
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public GlossaryEditStructureGeneralFieldsTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования изменения структуры - Definition/Interpretation
		/// </summary>
		[Test]
		public void AddInterpretationFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();
			// Проверить поле Interpretation
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Interpretation];
			CheckEditGlossaryStructureTextarea(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры - Definition source/Interpretation source
		/// </summary>
		[Test]
		public void AddInterpretationSourceFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();
			// Проверить поле InterpretationSource
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.InterpretationSource];
			CheckEditGlossaryStructureTextarea(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры - Example
		/// </summary>
		[Test]
		public void AddExampleFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();
			// Проверить поле Example
			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Example];
			CheckEditGlossaryStructureTextarea(fieldName);
		}

		/// <summary>
		/// Метод тестирования изменения структуры - Topic
		/// </summary>
		[Test]
		public void AddTopicFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();

			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Topic];

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistInput(fieldName), "Ошибка: поле не появилось");

			// Нажать на поле
			GlossaryPage.ClickTopicDropdown(fieldName);

			// Проверить, что список открылся
			Assert.IsTrue(GlossaryPage.GetIsTopicListVisible(), "Ошибка: список не открылся");

			// Выбрать Все
			GlossaryPage.SelectTopicItem();
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			var text = GlossaryPage.GetTopicValue(fieldName);
			Console.WriteLine("text:\n" + text + "...");

			// Проверить, что значение в поле есть
			Assert.IsTrue(GlossaryPage.GetTopicValue(fieldName).Length > 0, "Ошибка: значение не сохранилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры - Project/Domain
		/// </summary>
		[Test]
		public void AddDomainFieldTest()
		{
			// Перейти на вкладку проектов
			SwitchDomainTab();

			// Проверить, есть ли проект с таким именем
			const string domainName = "TestDomainGlossaryEditStructure";
			CreateDomainIfNotExist(domainName);

			// Вернуться к глоссариям
			SwitchGlossaryTab();

			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();

			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Domain];

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistSelect(fieldName), "Ошибка: поле не появилось");

			// Нажать на поле, чтобы открылся список
			GlossaryPage.ClickSelectDropdown(fieldName);
			// Проверить, что список открылся
			Assert.IsTrue(GlossaryPage.GetIsSelectListVisible(), "Ошибка: список не открылся");

			GlossaryPage.SelectChoiceItem(domainName);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что значение в поле есть
			Assert.AreEqual(
				domainName,
				GlossaryPage.GetSelectValue(fieldName), 
				"Ошибка: проект не сохранился в поле");

		}

		/// <summary>
		/// Метод тестирования изменения структуры - Image
		/// </summary>
		[Test]
		public void AddImageFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();

			const string fieldName = "Image";

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistInput(fieldName), "Ошибка: поле не появилось");

			// Нажать на поле, чтобы открылся диалог загрузки документа
			GlossaryPage.ClickImageToImport(fieldName);
			// Заполнить диалог загрузки изображения
			FillAddDocumentForm(ImageFile);
			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			
			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что изображение загрузилось
			Assert.IsTrue(GlossaryPage.GetFieldImageFilled(fieldName), "Ошибка: изображение не загрузилось");
		}

		/// <summary>
		/// Метод тестирования изменения структуры - Multimedia
		/// </summary>
		[Test]
		public void AddMultimediaFieldTest()
		{
			// Изменить структуру глоссария, открыть создание нового термина
			EditGlossaryGeneralStructure();

			var fieldName = GlossaryEditStructureForm.attributeDict[GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Multimedia];

			// Проверить, что поле появилось
			Assert.IsTrue(GlossaryPage.GetIsExistInput(fieldName), "Ошибка: поле не появилось");

			// Нажать на поле, чтобы открылся диалог загрузки документа
			GlossaryPage.ClickMediaToImport(fieldName);

			// Загружать видео или звук
			FillAddDocumentForm(AudioFile);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();

			// Дождаться появления поля с сохраненным термином
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что файл загрузился
			Assert.IsTrue(
				GlossaryPage.GetIsFieldMediaFilled(fieldName), 
				"Ошибка: файл не загрузился");
		}



		/// <summary>
		/// Проверить работу textarea
		/// </summary>
		/// <param name="fieldName">название поля</param>
		protected void CheckEditGlossaryStructureTextarea(string fieldName)
		{
			// Проверить, что поле появилось
			Assert.IsTrue(
				GlossaryPage.GetIsExistTextarea(fieldName),
				"Ошибка: поле не появилось");

			// Ввести текст в поле
			var interpretationExample = fieldName + " Example";

			GlossaryPage.FillTextarea(fieldName, interpretationExample);

			// Сохранить термин
			GlossaryPage.ClickSaveExtendedConcept();
			Assert.IsTrue(GlossaryPage.WaitConceptSave(), "Ошибка: термин не сохранился");

			// Проверить, что текст в поле сохранился
			var text = GlossaryPage.GetTextareaValue(fieldName).Trim();
			Assert.AreEqual(interpretationExample, text, "Ошибка: текст не сохранился");
		}

		/// <summary>
		/// Создать глоссарий, изменить структуру глоссария, заполнить термин
		/// </summary>
		protected void EditGlossaryGeneralStructure()
		{
			// Имя глоссария для тестирования структуры, чтобы не создавать лишний раз
			const string glossaryName = "TestGlossaryEditStructureUniqueName";

			if (!GetIsExistGlossary(glossaryName))
			{
				// Создать глоссарий
				CreateGlossaryByName(glossaryName);
			}
			else
			{
				// Открыть глоссарий
				SwitchCurrentGlossary(glossaryName);
			}

			// Добавить все поля в структуру
			AddAllSystemGeneralFieldStructure();
			// Нажать New item
			GlossaryPage.ClickNewItemBtn();
			// Заполнить термин
			FillNewItemExtended();
		}

		/// <summary>
		/// Добавить все поля
		/// </summary>
		protected void AddAllSystemGeneralFieldStructure()
		{
			// Открыть редактирование структуры
			OpenEditGlossaryStructure();
			// Добавить все поля
			GlossaryEditStructureForm.SelectAllFields();
			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();
		}

		/// <summary>
		/// Создать Domain, есть его нет
		/// </summary>
		/// <param name="domainName"></param>
		protected void CreateDomainIfNotExist(string domainName)
		{
			if (!GetIsDomainExist(domainName))
			{
				// Если проект не найден, создать его
				CreateDomain(domainName);
			}
		}
	}
}