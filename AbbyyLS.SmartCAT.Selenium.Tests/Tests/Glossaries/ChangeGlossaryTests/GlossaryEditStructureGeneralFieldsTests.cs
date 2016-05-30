using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Glossaries]
	public class GlossaryEditStructureGeneralFieldsTests<TWebDriverProvider>
		: BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();
		}

		[TestCase(GlossarySystemField.Interpretation)]
		[TestCase(GlossarySystemField.InterpretationSource)]
		[TestCase(GlossarySystemField.Example)]
		public void AddSystemFieldTextareaTypeTest(GlossarySystemField fieldName)
		{
			var value = "Test System Field";
			
			_glossaryPage
				.FillSystemField(fieldName, value)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldValueMatchExpected(fieldName, value),
				"Произошла ошибка:\n значение в поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-17114")]
		public void AddTopicSystemFieldTest()
		{
			var value = "Life";

			_glossaryPage
				.SelectOptionInTopic(value)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsFieldValueMatchExpected(GlossarySystemField.Topic, value),
				"Произошла ошибка:\n значение в поле не совпадает с ожидаемым значением");

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("SCAT-935")]
		public void AddImageFieldTest()
		{
			var fieldName = GlossarySystemField.Image.Description();

			_glossaryPage
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

			_glossaryPage
				.UploadMediaFile(PathProvider.AudioFileForGlossariesTests)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFileForGlossariesTests), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);
				
			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		private WorkspacePage _workspacePage;
		private GlossariesHelper _glossaryHelper;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private string _glossaryUniqueName;
	}
}
