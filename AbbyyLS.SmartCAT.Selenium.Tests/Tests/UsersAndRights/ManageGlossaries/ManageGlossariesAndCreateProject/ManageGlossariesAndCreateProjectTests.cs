using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersAndRights
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ManageGlossariesAndCreateProjectTests<TWebDriverProvider> : ManageGlossariesAndCreateProjectBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test]
		public void CreateGlossaryInWizardTest()
		{
			var projectUniqueName = CreateProjectHelper.CourseraProjectName;

			_workspacePage.GoToProjectsPage();
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { PathProvider.DocumentFile })
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
