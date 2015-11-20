using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class GlossaryEditStructureCustomFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);
			_glossaryHelper = _workspaceHelper.GoToGlossariesPage();
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
		}

		[Test, Ignore("PRX-10924")]
		public void AddTextFieldTest()
		{
			var fieldName = "TextField";
			var customValue = "DefaultValue";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Text)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.FillField(fieldName, customValue)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, customValue)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddTextRequiredFieldTest()
		{
			var fieldName = "RequiredTextField";
			var customValue = "DefaultValue";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Text,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SaveEntry()
				.AssertFieldErrorDisplayed(fieldName)
				.FillField(fieldName, customValue)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, customValue)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddDateFieldTest()
		{
			var fieldName = "DateField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Date)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.FillDateField(fieldName)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddDateRequiredFieldTest()
		{
			var fieldName = "DateRequiredField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Date,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SaveEntry()
				.AssertFieldErrorDisplayed(fieldName)
				.FillDateField(fieldName)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture))
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddMediaFieldTest()
		{
			var fieldName = "MediaField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Media)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.SaveEntry()
				.AssertMediaFieldFilled(fieldName, Path.GetFileName(PathProvider.AudioFile))
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddMediaRequiredFieldTest()
		{
			var fieldName = "MediaRequiredField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Media,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.SaveEntry()
				.AssertCustomImageFieldErrorDisplayed(fieldName)
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.SaveEntry()
				.AssertMediaFieldFilled(fieldName, Path.GetFileName(PathProvider.AudioFile))
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddImageFieldTest()
		{
			var fieldName = "ImageField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Image)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertImageFieldExistInNewEntry(fieldName)
				.UploadImage(fieldName, PathProvider.ImageFile)
				.SaveEntry()
				.AssertImageFieldFilled(fieldName)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddImageRequiredFieldTest()
		{
			var fieldName = "ImageField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Image,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertImageFieldExistInNewEntry(fieldName)
				.SaveEntry()
				.AssertCustomImageFieldErrorDisplayed(fieldName)
				.UploadImage(fieldName, PathProvider.ImageFile)
				.SaveEntry()
				.AssertImageFieldFilled(fieldName)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddListFieldTest()
		{
			var fieldName = "ListField";
			var itemsList = new List<string> { "select1", "select2" };

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.List,
					itemsList: itemsList)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SelectItemInListDropdown(fieldName, itemsList[0])
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, itemsList[0])
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddListRequiredFieldTest()
		{
			var fieldName = "ListRequiredField";
			var itemsList = new List<string> { "select1", "select2" };

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.List,
					itemsList: itemsList,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SaveEntry()
				.AssertFieldErrorDisplayed(fieldName)
				.SelectItemInListDropdown(fieldName, itemsList[0])
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, itemsList[0])
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddMultiselectListFieldTest()
		{
			var fieldName = "MultiselectListField";
			var itemsList = new List<string> { "select1", "select2", "select3" };

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Multiselect,
					itemsList: itemsList)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[0])
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[2])
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, itemsList[0] + ", " + itemsList[2])
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddMultiselectListRequiredFieldTest()
		{
			var fieldName = "MultiselectListRequiredField";
			var itemsList = new List<string> { "select1", "select2", "select3" };

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Multiselect,
					itemsList: itemsList,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.SaveEntry()
				.AssertFieldErrorDisplayed(fieldName)
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[0])
				.SelectItemInMultiSelectListDropdown(fieldName, itemsList[2])
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, itemsList[0] + ", " + itemsList[2])
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddNumberFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Number)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.FillNumberField(fieldName, customNumberValue)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, customNumberValue)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddYesNoFieldTest()
		{
			var fieldName = "YesNoField";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.YesNo)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.ClickYesNoCheckBox()
				.SaveEntry()
				.AssertYesNoCheckboxChecked("Yes", fieldName)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Ignore("PRX-10924")]
		public void AddNumberRequiredFieldTest()
		{
			var fieldName = "NumberField";
			var customNumberValue = "90";

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddCustomField(
					fieldName,
					GlossaryCustomFieldType.Number,
					isRequiredField: true)
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection()
				.AssertFieldExistInNewEntry(fieldName)
				.SaveEntry()
				.AssertFieldErrorDisplayed(fieldName)
				.FillNumberField(fieldName, customNumberValue)
				.SaveEntry()
				.AssertCustomFieldValueMatch(fieldName, customNumberValue)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}
		
		private GlossariesHelper _glossaryHelper;
		private WorkspaceHelper _workspaceHelper;
		private string _glossaryUniqueName;
	}
}
