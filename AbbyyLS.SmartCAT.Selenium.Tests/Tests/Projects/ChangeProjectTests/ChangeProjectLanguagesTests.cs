using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Projects]
	class ChangeProjectLanguagesTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7058"), ShortCheckList]
		public void AddLanguageJobsTest()
		{
			var document = PathProvider.DocumentFile;
			var documentFileName = Path.GetFileNameWithoutExtension(document);
			var ttxFile = PathProvider.TtxFile;
			var ttxFileName = Path.GetFileNameWithoutExtension(ttxFile);

			var expectedJobList = new List<string>
				(
					new[]
					{
						documentFileName + "_de",
						documentFileName + "_ru",
						ttxFileName + "_de",
						ttxFileName + "_ru"
					}
				);

			_createProjectHelper.CreateNewProject(
				projectName: _projectUniqueName, filesPaths: new[] { document, ttxFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			expectedJobList.Sort();
			var jobList = _projectsPage.GetJobList(_projectUniqueName);

			Assert.AreEqual(expectedJobList, jobList,
				"Произошла ошибка: Неверный список джобов.");
		}

		[Test, Description("S-7140"), ShortCheckList]
		public void AddLanguageInProjectWithTranslationMemoryTest()
		{
			var document = PathProvider.EditorTxtFile;
			var targetLanguages = new List<string> { Language.Russian.Description(), Language.German.Description() };
			targetLanguages.Sort();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { document },
				createNewTm: true,
				useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_workspacePage.GoToTranslationMemoriesPage();

			_translationMemoriesPage.SearchForTranslationMemory(_projectUniqueName);

			Assert.AreEqual(targetLanguages, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(_projectUniqueName),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}

		[Test, Description("S-7141"), ShortCheckList]
		public void AddLanguageInProjectWithGlossaryTest()
		{
			var document = PathProvider.EditorTxtFile;
			var languages = new List<string> { Language.Russian.Description(), Language.English.Description(), Language.German.Description()};
			languages.Sort();
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { document },
				glossaryName: glossaryUniqueName);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.ClickSetUpLanguagesOptionsButton();

			_newLanguageSettingsDialog
				.SelectGlossary(glossaryUniqueName)
				.ClickSaveButton();

			_projectSettingsDialog.SaveSettingsExpectingProjectsPage();

			_workspacePage.GoToGlossariesPage();

			Assert.AreEqual(
				languages,
				_glossariesPage.GetTargetLanguages(glossaryUniqueName),
				"Произошла ошибка: неверный список языков у глоссария {0}.", "TestGlossary-c3716197-bbd1-4616-ab6d-7900d34a100c");
		}
	}
}
