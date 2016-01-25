using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class GlossaryEditStructureCustomFieldsTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
		}

		[Test, Ignore("PRX-10924")]
		public void AddTextFieldTest()
		{
			var fieldName = "TextField";
			var customValue = "DefaultValue";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddTextRequiredFieldTest()
		{
			var fieldName = "RequiredTextField";
			var customValue = "DefaultValue";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddDateFieldTest()
		{
			var fieldName = "DateField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddDateRequiredFieldTest()
		{
			var fieldName = "DateRequiredField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddMediaFieldTest()
		{
			var fieldName = "MediaField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFile), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);
				
			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddMediaRequiredFieldTest()
		{
			var fieldName = "MediaRequiredField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFile), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddImageFieldTest()
		{
			var fieldName = "ImageField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadImageFile(PathProvider.ImageFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddImageRequiredFieldTest()
		{
			var fieldName = "ImageField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadImageFile(PathProvider.ImageFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddListFieldTest()
		{
			var fieldName = "ListField";
			var itemsList = new List<string> { "select1", "select2" };

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddListRequiredFieldTest()
		{
			var fieldName = "ListRequiredField";
			var itemsList = new List<string> { "select1", "select2" };

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddMultiselectListFieldTest()
		{
			var fieldName = "MultiselectListField";
			var itemsList = new List<string> { "select1", "select2", "select3" };

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Multiselect, itemsList: itemsList);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();

			Assert.IsTrue(_glossaryPage.IsFieldExistInNewEntry(fieldName),
				"Произошла ошибка:\n поле {0} отсутствует в новом термине", fieldName);

			_glossaryPage
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[0])
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[2])
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0] + ", " + itemsList[2]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddMultiselectListRequiredFieldTest()
		{
			var fieldName = "MultiselectListRequiredField";
			var itemsList = new List<string> { "select1", "select2", "select3" };

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[0])
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[2])
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsCustomFieldValueMatchExpected(fieldName, itemsList[0] + ", " + itemsList[2]),
				"Произошла ошибка:\n значение в кастомном поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddNumberFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddYesNoFieldTest()
		{
			var fieldName = "YesNoField";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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

		[Test, Ignore("PRX-10924")]
		public void AddNumberRequiredFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

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
		
		private GlossariesHelper _glossaryHelper;
		private WorkspacePage _workspacePage;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private string _glossaryUniqueName;
	}
}
