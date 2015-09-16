using System.IO;

using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Standalone]
	public class GlossaryEditStructureGeneralFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
			_glossaryHelper = _workspaceHelper
				.GoToGlossariesPage()
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddAllSystemFields()
				.ClickNewEntryButton()
				.AssertExtendModeOpen()
				.FillTermInLanguagesAndTermsSection();
		}

		[TestCase(GlossarySystemField.Definition)]
		[TestCase(GlossarySystemField.DefinitionSource)]
		[TestCase(GlossarySystemField.Example)]
		public void AddSystemFieldTextareaTypeTest(GlossarySystemField fieldName)
		{
			var value = "Test System Field";
			string field;

			if (fieldName == GlossarySystemField.DefinitionSource)
			{
				field = "Definition source";
			}
			else
			{
				field = fieldName.ToString();
			}

			_glossaryHelper
				.AssertSystemTextAreaFieldDisplayed(fieldName)
				.FillSystemField(fieldName, value)
				.ClickSaveEntryButton()
				.AssertFieldValueMatch(field, value)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void AddTopicSystemFieldTest()
		{
			var value = "Life";

			_glossaryHelper
				.AssertSystemDropdownFieldDisplayed(GlossarySystemField.Topic)
				.SelectOptionInTopic(value)
				.ClickSaveEntryButton()
				.AssertFieldValueMatch(GlossarySystemField.Topic.Description(), value)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void AddImageFieldTest()
		{
			var fieldName = GlossarySystemField.Image.Description();

			_glossaryHelper
				.AssertImageFieldExistInNewEntry(fieldName)
				.UploadImage(fieldName, PathProvider.ImageFile)
				.ClickSaveEntryButton()
				.AssertImageFieldFilled(fieldName)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void AddMultimediaFieldTest()
		{
			var fieldName = GlossarySystemField.Multimedia.Description();

			_glossaryHelper
				.AssertMediaFieldExistInNewEntry(fieldName)
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.ClickSaveEntryButton()
				.AssertMediaFieldFilled(fieldName, Path.GetFileName(PathProvider.AudioFile))
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		private readonly WorkspaceHelper _workspaceHelper = new WorkspaceHelper();
		private GlossariesHelper _glossaryHelper;
		private string _glossaryUniqueName;
	}
}
