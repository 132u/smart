using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class GlossaryEditStructureGeneralFieldsTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspaceHelper = new WorkspaceHelper(Driver);

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

		[TestCase(GlossarySystemField.Interpretation), Explicit("Тест исключен из-за баги SCAT-559")]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Example)]
		public void AddSystemFieldTextareaTypeTest(GlossarySystemField fieldName)
		{
			var value = "Test System Field";
			
			_glossaryHelper
				.AssertSystemTextAreaFieldDisplayed(fieldName)
				.FillSystemField(fieldName, value)
				.ClickSaveEntryButton()
				.AssertGeneralFieldValueMatch(fieldName, value)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test, Explicit("Тест исключен из-за баги SCAT-559")]
		public void AddTopicSystemFieldTest()
		{
			var value = "Life";

			_glossaryHelper
				.AssertSystemDropdownFieldDisplayed(GlossarySystemField.Topic)
				.SelectOptionInTopic(value)
				.ClickSaveEntryButton()
				.AssertGeneralFieldValueMatch(GlossarySystemField.Topic, value)
				.CloseTermsInfo()
				.AssertExtendTermsCountMatch(expectedTermCount: 1);
		}

		[Test]
		public void AddImageFieldTest()
		{
			var fieldName = GlossarySystemField.Image.Description();

			_glossaryHelper
				.AssertImageFieldExistInNewEntry(fieldName)
				.UploadImageWithMultimedia(fieldName, PathProvider.ImageFile)
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

		private WorkspaceHelper _workspaceHelper;
		private GlossariesHelper _glossaryHelper;
		private string _glossaryUniqueName;
	}
}
