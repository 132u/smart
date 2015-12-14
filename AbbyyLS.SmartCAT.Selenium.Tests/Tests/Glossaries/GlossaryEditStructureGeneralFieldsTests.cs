using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
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
			_glossaryPage = new GlossaryPage(Driver);
			_glossaryHelper = new GlossariesHelper(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);

			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspaceHelper.GoToGlossariesPage();

			_glossaryHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryStructure();

			_glossaryStructureDialog.AddAllSystemFields();

			_glossaryPage
				.ClickNewEntryButton()
				.FillTermInLanguagesAndTermsSection();
		}

		[TestCase(GlossarySystemField.Interpretation), Ignore("PRX-10924")]
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

		[Test, Ignore("PRX-10924")]
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

		[Test, Ignore("PRX-10924")]
		public void AddImageFieldTest()
		{
			var fieldName = GlossarySystemField.Image.Description();

			_glossaryPage
				.UploadImageFileWithMultimedia(PathProvider.ImageFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsImageFieldFilled(fieldName),
				"Произошла ошибка:\n  поле {0} типа Image не заполнено", fieldName);

			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		[Test, Ignore("PRX-10924")]
		public void AddMultimediaFieldTest()
		{
			var fieldName = GlossarySystemField.Multimedia.Description();

			_glossaryPage
				.UploadMediaFile(fieldName, PathProvider.AudioFile)
				.ClickSaveEntryButton();

			Assert.IsTrue(_glossaryPage.IsMediaFileMatchExpected(Path.GetFileName(PathProvider.AudioFile), fieldName),
				"Произошла ошибка:\n неверное значение в поле {0} типа Media.", fieldName);
				
			_glossaryPage.CloseExpandedTerms();

			Assert.AreEqual(1, _glossaryPage.CustomTermsCount(),
				"Произошла ошибка:\n неверное количество терминов.");
		}

		private WorkspaceHelper _workspaceHelper;
		private GlossariesHelper _glossaryHelper;
		private GlossaryPage _glossaryPage;
		private GlossaryStructureDialog _glossaryStructureDialog;
		private string _glossaryUniqueName;
	}
}
