using System.Collections.Generic;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects.ChangeProjectTests
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	class ChangeProjectLanguagesTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7058")]
		public void AddLanguageJobsTest()
		{
			var documentFileName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
			var ttxFileName = Path.GetFileNameWithoutExtension(PathProvider.TtxFile);

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
				projectName: _projectUniqueName,
				filesPaths: new[] { PathProvider.DocumentFile, PathProvider.TtxFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 1)
				.OpenDocumentInfoForProject(_projectUniqueName, documentNumber: 2);

			expectedJobList.Sort();
			var jobList = _projectsPage.GetJobList(_projectUniqueName);

			Assert.AreEqual(expectedJobList, jobList,
				"Произошла ошибка: Неверный список джобов.");
		}

		[Test, Description("S-7140")]
		public void AddLanguageInProjectWithTranslationMemoryTest()
		{
			var targetLanguages = new List<string> { Language.Russian.Description(), Language.German.Description() };
			targetLanguages.Sort();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.EditorTxtFile },
				tmxFilesPaths: new[] { PathProvider.EditorTmxFile },
				createNewTm: true,
				useMachineTranslation: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_workspacePage.GoToTranslationMemoriesPage();

			Assert.AreEqual(targetLanguages, _translationMemoriesPage.GetTranslationMemoryTargetLanguages(Path.GetFileNameWithoutExtension(PathProvider.EditorTmxFile)),
				"Произошла ошибка: Неверно указаны целевые языки.");
		}

		[Test, Description("S-7141")]
		public void AddLanguageInProjectWithGlossaryTest()
		{
			var languages = new List<string> { Language.Russian.Description(), Language.English.Description(), Language.German.Description()};
			languages.Sort();
			var glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.EditorTxtFile },
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
