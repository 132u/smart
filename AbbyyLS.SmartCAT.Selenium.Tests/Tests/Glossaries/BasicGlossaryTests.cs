using System.Collections.Generic;
using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	class BasicGlossaryTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_workspaceHelper = new WorkspaceHelper();
			_glossaryHelper = _workspaceHelper.GoToGlossariesPage();
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();
		}

		[Test]
		public void CreateGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName);
		}

		[Test]
		public void CreateGlossaryWithoutNameTest()
		{
			_glossaryHelper
				.CreateGlossary("", errorExpected: true)
				.AssertSpecifyGlossaryNameErrorDisplay();
		}
		
		[Test]
		public void CreateGlossaryWithExistingNameTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName)
				.CreateGlossary(_glossaryUniqueName, errorExpected: true)
				.AssertExistNameErrorDisplay();
		}

		[Test]
		public void CheckLanguageNotExistTest()
		{
			_glossaryHelper
				.OpenNewGlossaryDialog()
				.SelectLanguage(Language.German, 1)
				.AssertLanguageNotExistInDropdown(Language.German, 2);
		}

		[Test]
		public void DeleteLanguageCreateGlossaryTest()
		{
			int languagesCountBefore;
			int languagesCountAfter;

			_glossaryHelper
				.OpenNewGlossaryDialog()
				.GetLanguagesCount(out languagesCountBefore)
				.DeleteLanguage(languagesCountBefore)
				.GetLanguagesCount(out languagesCountAfter);

			_glossaryHelper.AssertLanguagesCountChanged(languagesCountBefore, languagesCountAfter);
		}

		[Ignore("PRX-10784"), Test]
		public void CreatedDateGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName)
				.AssertDateModifiedMatchCurrentDate(_glossaryUniqueName);
		}

		[Test]
		public void ModifiedDateGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName);

			var modifiedDateBefore = _glossaryHelper.ModifiedDateTime(_glossaryUniqueName);
			_glossaryHelper.GoToGlossaryPage(_glossaryUniqueName);

			// Sleep нужен, чтобы дата изменения изменилась (точность до минуты)
			Thread.Sleep(60000);

			_glossaryHelper
				.CreateTerm()
				.GoToGlossariesPage();

			var modifiedDateAfter = _glossaryHelper.ModifiedDateTime(_glossaryUniqueName);

			_glossaryHelper.AssertDateTimeModifiedNotMatch(modifiedDateBefore, modifiedDateAfter);
		}

		[Test]
		public void ModifiedByAuthorTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName)
				.AssertModifiedByMatch(_glossaryUniqueName, ThreadUser.NickName);
		}

		[Test]
		public void DeleteGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryProperties()
				.DeleteGlossaryInPropertiesDialog()
				.AssertGlossaryNotExist(_glossaryUniqueName);
		}

		[Test]
		public void DeleteLanguageExistTermTest()
		{
			int languagesCountBefore;
			int languagesCountAfter;

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm()
				.OpenGlossaryProperties()
				.LanguagesCountInPropertiesDialog(out languagesCountBefore);

			_glossaryHelper
				.CancelDeleteLanguageInPropertiesDialog()
				.LanguagesCountInPropertiesDialog(out languagesCountAfter);

			_glossaryHelper.AssertLanguagesCountMatch(languagesCountBefore, languagesCountAfter);
		}

		[Test]
		public void EditGlossaryStructureTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryStructure()
				.AddNewSystemField(GlossarySystemField.Topic)
				.ClickNewEntryButton()
				.AssertExtendModeOpen();
		}

		[Test]
		public void ImportGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.ImportGlossary(PathProvider.ImportGlossaryFile)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1);
		}

		[Test]
		public void ImportGlossaryReplaceAllTermsTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm()
				.CreateTerm("termsecond", "termsecond")
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 2)
				.ImportGlossaryWithReplaceTerms(PathProvider.ImportGlossaryFile)
				.AssertGlossaryContainsCorrectTermsCount(termsCount: 1);
		}

		[Test]
		public void ExportGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.CreateTerm()
				.CreateTerm("secondTerm1", "secondTerm2")
				.ExportGlossary()
				.AssertGlossaryExportedSuccesfully(Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx"));
		}

		[Test]
		public void ChangeGlossaryNameTest()
		{
			var newGlossaryName = GlossariesHelper.UniqueGlossaryName();

			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryProperties()
				.FillGlossaryNameInPropertiesDialog(newGlossaryName)
				.ClickSaveButtonInPropetiesDialog()
				.GoToGlossariesPage()
				.AssertGlossaryExist(newGlossaryName)
				.AssertGlossaryNotExist(_glossaryUniqueName);
		}

		[Test]
		public void ChangeGlossaryExistingNameTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.GoToGlossariesPage()
				.CreateGlossary(_glossaryUniqueName + "2")
				.OpenGlossaryProperties()
				.FillGlossaryNameInPropertiesDialog(_glossaryUniqueName)
				.ClickSaveButtonInPropetiesDialog(errorExpected: true)
				.AssertExistNameErrorDisplay();
		}

		[TestCase("")]
		[TestCase(" ")]
		public void ChangeGlossaryEmptyNameTest(string newGlossaryName)
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryProperties()
				.FillGlossaryNameInPropertiesDialog(newGlossaryName)
				.ClickSaveButtonInPropetiesDialog(errorExpected: true)
				.AssertSpecifyGlossaryNameErrorDisplay();
		}

		[Test]
		public void OpenStructureFromPropertiesTest()
		{
			_glossaryHelper
				.CreateGlossary(_glossaryUniqueName)
				.OpenGlossaryProperties()
				.SwitchToGlossaryStructureDialog();
		}

		[Test]
		public void CreateMultiLanguageGlossaryTest()
		{
			_glossaryHelper
				.CreateGlossary(
					_glossaryUniqueName,
					languageList: new List<Language>
						{
							 Language.German,
							 Language.French,
							 Language.Japanese,
							 Language.Lithuanian
						})
				.GoToGlossariesPage()
				.AssertGlossaryExist(_glossaryUniqueName);
		}

		private GlossariesHelper _glossaryHelper;
		private WorkspaceHelper _workspaceHelper;
		private string _glossaryUniqueName;
	}
}
