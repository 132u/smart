using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Client;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries.SuggestedTerms;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.ProjectGroups;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.CreateProjectDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.DocumentUploadDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageGlossaryContent
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossaryContentEditAdvancedSettingsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			_clientsPage = new ClientsPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossariesAdvancedSettingsSection = new GlossariesAdvancedSettingsSection(Driver);
			_newProjectSettingsPage = new NewProjectSettingsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_settingsDialog = new ProjectSettingsDialog(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_workspacePage = new WorkspacePage(Driver);
			_loginHelper = new LoginHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_exportNotification = new ExportNotification(Driver);
			_usersTab = new UsersTab(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_qualityAssuranceDialog = new QualityAssuranceDialog(Driver);
			_projectSettingsHelper = new ProjectSettingsHelper(Driver);
			_workflowSetUpTab = new WorkflowSetUpTab(Driver);
			_generalTab = new GeneralTab(Driver);
			_qualityAssuranceSettings = new QualityAssuranceSettings(Driver);
			_cancelConfirmationDialog = new CancelConfirmationDialog(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_documentSettingsDialog = new DocumentSettingsDialog(Driver);
			_confirmDeclineTaskDialog = new ConfirmDeclineTaskDialog(Driver);
			_statisticsPage = new BuildStatisticsPage(Driver);
			_selectTaskDialog = new SelectTaskDialog(Driver);
			_editorPage = new EditorPage(Driver);
			_pretranslationDialog = new PretranslationDialog(Driver);
			_userRightsHelper = new UserRightsHelper(Driver);
			_addFilesStep = new AddFilesStep(Driver);
			_newProjectDocumentUploadPage = new NewProjectDocumentUploadPage(Driver);
			_newProjectWorkflowPage = new NewProjectWorkflowPage(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
			_projectGroupsPage = new ProjectGroupsPage(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossaryImportDialog = new GlossaryImportDialog(Driver);
			_glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_suggestTermDialog = new SuggestTermDialog(Driver);
			_suggestedTermsPageForAllGlossaries = new SuggestedTermsGlossariesPage(Driver);
			_suggestedTermsPageForCurrentGlossaries = new SuggestedTermsGlossaryPage(Driver);
			_addTermDialog = new AddTermDialog(Driver);
			_glossaryPage = new GlossaryPage(Driver);

			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			AdditionalUser = TakeUser(ConfigurationManager.AdditionalUsers);

			var groupName = Guid.NewGuid().ToString();

			_userRightsHelper.CreateGroupWithSpecificRights(
				AdditionalUser.NickName,
				groupName,
				new List<RightsType> { RightsType.GlossaryContentManagement });
		}
		
		[SetUp]
		public override void BeforeTest()
		{
			CustomTestContext.WriteLine("Начало работы теста {0}", TestContext.CurrentContext.Test.Name);
			_loginHelper.Authorize(StartPage.Workspace, ThreadUser);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
				
			_workspacePage.GoToGlossariesPage();
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_loginHelper.Authorize(StartPage.Workspace, AdditionalUser);
			_workspacePage.GoToGlossariesPage();
			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);
		}

		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			if (AdditionalUser != null)
			{
				ReturnUser(ConfigurationManager.AdditionalUsers, AdditionalUser);
			}
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
			var imageFile = PathProvider.ImageFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadImageFileWithMultimedia(imageFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddMultiMediaFieldTest()
		{
			var fieldName = GlossarySystemField.Multimedia.Description();
			var audioFile = PathProvider.AudioFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();
			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(audioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(audioFile, fieldName),
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
			var audioFile = PathProvider.AudioFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(audioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(audioFile, fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddMediaRequiredFieldTest()
		{
			var fieldName = "MediaRequiredField";
			var audioFile = PathProvider.AudioFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Media, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadMediaFile(audioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(audioFile, fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test]
		public void AddImageFieldTest()
		{
			var fieldName = "ImageField";
			var imageFile = PathProvider.ImageFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.UploadImageFile(imageFile)
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
			var imageFile = PathProvider.ImageFileForGlossariesTests;

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddCustomField(fieldName, GlossaryCustomFieldType.Image, isRequired: true);

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection()
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldErrorDisplayed(fieldName),
				"Произошла ошибка:\n поле {0} не подсвечено красным цветом.", fieldName);

			_glossaryPage
				.UploadImageFile(imageFile)
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

		protected string _term1;
		protected string _term2;
		protected string _term3;
		protected string _term4;
		protected string _term5;
		protected string _term6;

		protected string _glossaryUniqueName;
		protected string _glossaryUniqueName2;
		protected string _glossaryUniqueName3;
		protected string _glossaryUniqueName4;
		protected string _glossaryUniqueName5;

		protected string _projectGroup;

		protected string _clientName;

		protected AddTermDialog _addTermDialog;
		protected SuggestedTermsGlossaryPage _suggestedTermsPageForCurrentGlossaries;
		protected ProjectGroupsPage _projectGroupsPage;
		protected GlossaryPropertiesDialog _glossaryPropertiesDialog;
		protected ClientsPage _clientsPage;
		protected GlossariesPage _glossariesPage;
		protected GlossaryPage _glossaryPage;
		protected GlossariesAdvancedSettingsSection _glossariesAdvancedSettingsSection;
		protected NewProjectWorkflowPage _newProjectWorkflowPage;
		protected NewProjectDocumentUploadPage _newProjectDocumentUploadPage;
		protected NewProjectSettingsPage _newProjectSettingsPage;
		protected AddFilesStep _addFilesStep;
		protected ProjectSettingsDialog _settingsDialog;
		protected EditorPage _editorPage;
		protected BuildStatisticsPage _statisticsPage;
		protected QualityAssuranceDialog _qualityAssuranceDialog;
		protected CreateProjectHelper _createProjectHelper;
		protected WorkspacePage _workspacePage;
		protected LoginHelper _loginHelper;
		protected string _projectUniqueName;
		protected UsersTab _usersTab;
		protected AddAccessRightDialog _addAccessRightDialog;
		protected ProjectsPage _projectsPage;
		protected ExportNotification _exportNotification;
		protected NewGroupDialog _newGroupDialog;
		protected GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		protected ProjectSettingsPage _projectSettingsPage;
		protected ProjectSettingsHelper _projectSettingsHelper;
		protected PretranslationDialog _pretranslationDialog;
		protected WorkflowSetUpTab _workflowSetUpTab;
		protected GeneralTab _generalTab;
		protected QualityAssuranceSettings _qualityAssuranceSettings;
		protected CancelConfirmationDialog _cancelConfirmationDialog;
		protected TaskAssignmentPage _taskAssignmentPage;
		protected ConfirmDeclineTaskDialog _confirmDeclineTaskDialog;
		protected DocumentSettingsDialog _documentSettingsDialog;
		protected SelectTaskDialog _selectTaskDialog;
		protected UserRightsHelper _userRightsHelper;
		protected GlossariesHelper _glossariesHelper;
		protected GlossaryStructureDialog _glossaryStructureDialog;
		protected GlossaryImportDialog _glossaryImportDialog;
		protected GlossarySuccessImportDialog _glossarySuccessImportDialog;
		protected SuggestTermDialog _suggestTermDialog;
		protected SuggestedTermsGlossariesPage _suggestedTermsPageForAllGlossaries;
	}
}
