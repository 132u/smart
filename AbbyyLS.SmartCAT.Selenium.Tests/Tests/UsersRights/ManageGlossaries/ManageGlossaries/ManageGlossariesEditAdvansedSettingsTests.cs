using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;
using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	public class ManageGlossariesEditAdvansedSettingsTests<TWebDriverProvider> : ManageGlossariesBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public override void BeforeTest()
		{
		
			CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
			_loginHelper = new LoginHelper(Driver);
			_loginHelper.Authorize(StartPage, AdditionalUser);

			_exportNotification.CancelAllNotifiers<Pages.Projects.ProjectsPage>();
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);
		}

		[TestCase(GlossarySystemField.Interpretation)]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Example)]
		public void AddSystemFieldTextareaTypeTest(GlossarySystemField fieldName)
		{
			var value = "Test System Field";

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.FillSystemField(fieldName, value)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldValueMatchExpected(fieldName, value),
				"Произошла ошибка:\n значение в поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddTopicSystemFieldTest()
		{
			var value = "Life";

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.SelectOptionInTopic(value)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldValueMatchExpected(GlossarySystemField.Topic, value),
				"Произошла ошибка:\n значение в поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddImageGeneralFieldTest()
		{
			var fieldName = GlossarySystemField.Image.Description();

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadImageFileWithMultimedia(PathProvider.ImageFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddMultimediaFieldTest()
		{
			var fieldName = GlossarySystemField.Multimedia.Description();

			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(PathProvider.AudioFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFileForGlossariesTests), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);
				
			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddCommentFieldTest()
		{
			var comment = "Comment Example";

			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.AddLanguageComment(comment)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsCommentFilled(comment),
				"Произошла ошибка:\n неверный текст в поле комментария");
		}

		[Test]
		public void AddDefinitionFieldTest()
		{
			var definition = "Definition Example";

			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.AddDefinition(definition)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsDefinitionFilled(definition),
				"Произошла ошибка:\n неверный текст в поле Definition");
		}

		[Test]
		public void AddDefinitionSourceFieldTest()
		{
			var definitionSource = "Definition source example";

			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Language)
				.AddLanguageFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.AddDefinitionSource(definitionSource)
				.ClickSaveEntryButton()
				.OpenLanguageDetailsViewMode();

			Assert.IsTrue(_glossaryPage.IsDefinitionSourceFilled(definitionSource),
				"Произошла ошибка:\n неверный текст в поле 'Definition source'");
		}

		[TestCase(GlossarySystemField.Source)]
		[TestCase(GlossarySystemField.Interpretation)]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Context)]
		[TestCase(GlossarySystemField.ContextSource)]
		public void AddSystemTermFieldTest(GlossarySystemField termField)
		{
			var termValue = "testTermSystemField";

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog
				.SelectLevelGlossaryStructure(GlossaryStructureLevel.Term)
				.AddTermField(termField);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsTermFieldEditModeDisplayed(termField),
				"Произошла ошибка:\nПоле {0} не отображается в режиме редактирования термина.", termField);

			_glossaryPage
				.FillTermField(termField, termValue)
				.ClickSaveEntryButton()
				.ClickTermInLanguagesAndTermsColumn();

			Assert.AreEqual(termValue, _glossaryPage.TermFieldViewModelText(termField),
				"Произошла ошибка:\nВ поле {0} неверное значение.", termField);
		}

		[Test]
		public void AddTextFieldTest()
		{
			var fieldName = "TextField";
			var customValue = "DefaultValue";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Text);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.FillField(fieldName, customValue)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, customValue),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddTextRequiredFieldTest()
		{
			var fieldName = "RequiredTextField";
			var customValue = "DefaultValue";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Text, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом", fieldName);

			_glossaryPage
				.FillField(fieldName, customValue)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, customValue),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddDateFieldTest()
		{
			var fieldName = "DateField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Date);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.FillDateField(fieldName)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddDateRequiredFieldTest()
		{
			var fieldName = "DateRequiredField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Date, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом", fieldName);

			_glossaryPage
				.FillDateField(fieldName)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddMediaFieldTest()
		{
			var fieldName = "MediaField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(PathProvider.AudioFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFileForGlossariesTests), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddMediaRequiredFieldTest()
		{
			var fieldName = "MediaRequiredField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadMediaFile(PathProvider.AudioFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFileForGlossariesTests), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddImageFieldTest()
		{
			var fieldName = "ImageField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadImageFile(PathProvider.ImageFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddImageRequiredFieldTest()
		{
			var fieldName = "ImageField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadImageFile(PathProvider.ImageFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddListFieldTest()
		{
			var fieldName = "ListField";
			var itemsList = new List<string> { "select1", "select2" };
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.List, itemsList: itemsList);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.SelectItemInListDropdown(fieldName, itemsList[0])
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddListRequiredFieldTest()
		{
			var fieldName = "ListRequiredField";
			var itemsList = new List<string> { "select1", "select2" };
			
			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog
				.AddCustomField(fieldName, GlossaryCustomFieldType.List, itemsList: itemsList, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом", fieldName);

			_glossaryPage
				.SelectItemInListDropdown(fieldName, itemsList[0])
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddMultiselectListFieldTest()
		{
			var fieldName = "MultiselectListField";
			var itemsList = new List<string> { "select1", "select2", "select3" };
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Multiselect, itemsList: itemsList);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.SelectItemInMultiSelectListDropdown(fieldName, new List<string> { "select1", "select3" })
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0] + ", " + itemsList[2]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddMultiselectListRequiredFieldTest()
		{
			var fieldName = "MultiselectListRequiredField";
			var itemsList = new List<string> { "select1", "select2", "select3" };
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog
				.AddCustomField(fieldName, GlossaryCustomFieldType.Multiselect, itemsList: itemsList, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом", fieldName);

			_glossaryPage
				.SelectItemInMultiSelectListDropdown(fieldName, new List<string> { "select1", "select3" })
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0] + ", " + itemsList[2]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddNumberFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Number);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.FillNumberCustomField(fieldName, customNumberValue)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, customNumberValue),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddYesNoFieldTest()
		{
			var fieldName = "YesNoField";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.YesNo);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickYesNoCheckbox()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsYesNoCheckboxChecked("Yes", fieldName),
				"Произошла ошибка:\n неверное значение в поле Yes/No");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddNumberRequiredFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";
			
			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Number, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом", fieldName);

			_glossaryPage
				.FillNumberCustomField(fieldName, customNumberValue)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, customNumberValue),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}
	}
}
