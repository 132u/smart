using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	[UsersAndRights]
	class ManageGlossariesAndCreateProjectTests<TWebDriverProvider> : ManageGlossariesAndCreateProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateGlossaryInWizardTest()
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();
			var document = PathProvider.DocumentFile;

			_workspacePage.GoToProjectsPage();
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { document })
				.ClickSettingsButton();

			_newProjectSettingsPage
				.FillGeneralProjectInformation(projectUniqueName)
				.ExpandAdvancedSettings()
				.ClickGlossariesTab();

			_glossariesAdvancedSettingsSection.ClickCreateGlossaryButton();

			Assert.AreEqual(1, _glossariesAdvancedSettingsSection.GetGlossariesCount(),
				"Произошла ошибка: Неверное количество глоссариев.");
		}
	}
}
