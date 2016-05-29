using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.UsersRights;
using AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[Projects]
	class StatisticsTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpBaseProjectTest()
		{
			_buildStatisticsPage = new BuildStatisticsPage(Driver);
			_statisticsPage = new StatisticsPage(Driver);
			_taskAssignmentPage = new TaskAssignmentPage(Driver);
			_usersTab = new UsersTab(Driver);
			_newGroupDialog = new NewGroupDialog(Driver);
			_addAccessRightDialog = new AddAccessRightDialog(Driver);
			_groupsAndAccessRightsTab = new GroupsAndAccessRightsTab(Driver);
			_selectAssigneePage = new SelectAssigneePage(Driver);
		}

		[Test, Description("S-7061"), ShortCheckList]
		public void CollapseAndExpandBuildStatisticTest()
		{
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[]
				{
					PathProvider.DocumentFile,
					PathProvider.DocumentFile2
				});

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectSettingsButton(_projectUniqueName);

			_projectSettingsDialog
				.SelectTargetLanguages(Language.German)
				.SaveSettingsExpectingProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			_buildStatisticsPage.ClickBuildStatisticsButton();

			_statisticsPage.ClickCollapseAllButton();

			Assert.IsTrue(_statisticsPage.IsAllLanguagesPanelsCollapsed(),
				"Произошла ошибка: не все языковые панели свернуты.");

			Assert.IsTrue(_statisticsPage.IsAllTotalStatistickPanelsCollapsed(),
				"Произошла ошибка: не все панели 'Total Statistics' свернуты.");

			Assert.IsTrue(_statisticsPage.IsAllFilePanelsCollapsed(),
				"Произошла ошибка: не все панели документов свернуты.");

			_statisticsPage.ClickExpandAllButton();

			Assert.IsFalse(_statisticsPage.IsAllLanguagesPanelsCollapsed(),
				"Произошла ошибка: не все языковые панели развернуты.");

			Assert.IsFalse(_statisticsPage.IsAllTotalStatistickPanelsCollapsed(),
				"Произошла ошибка: не все панели 'Total Statistics' развернуты.");

			Assert.IsFalse(_statisticsPage.IsAllFilePanelsCollapsed(),
				"Произошла ошибка: не все панели документов развернуты.");
		}

		[TestCase("jpcFile.jpc")]
		[TestCase("tjsonFile.tjson")]
		[TestCase("bmpFile.bmp")]
		[TestCase("docFile.doc")]
		[TestCase("docxFile.docx")]
		[TestCase("htmFile.htm")]
		[TestCase("htmlFile.html")]
		[TestCase("inxFile.inx")]
		[TestCase("jsonFile.json")]
		[TestCase("odpFile.odp")]
		[TestCase("odtFile.odt")]
		[TestCase("phpFile.php")]
		[TestCase("poFile.po")]
		[TestCase("rtfFile.rtf")]
		[TestCase("srtFile.srt")]
		[TestCase("test.txt")]
		[TestCase("ttxFile.ttx")]
		[TestCase("xlfFile.xlf")]
		[TestCase("xmlFile.xml")]
		[TestCase("tiffFile.tiff")]
		[TestCase("jp2File.jp2")]
		[TestCase("xliffFile.xliff")]
		[TestCase("gifFile.GIF")]
		[TestCase("jfifFile.JFIF")]
		[TestCase("pngFile.PNG")]
		[TestCase("pptFile.ppt")]
		[TestCase("djvuFile.djvu")]
		[TestCase("dcxFile.dcx")]
		[TestCase("tifFile.TIF")]
		[TestCase("jpgFile.jpg")]
		[TestCase("pdfFile.pdf")]
		[TestCase("xlsFile.xls")]
		[TestCase("resxFile.resx")]
		[TestCase("csvFile.csv")]
		[TestCase("jb2File.jb2")]
		[TestCase("pcxFile.pcx")]
		[TestCase("potxFile.potx")]
		[TestCase("ppsFile.pps")]
		[TestCase("ppsxFile.ppsx")]
		[TestCase("pptxFile.pptx")]
		[TestCase("idmlFile.idml")]
		[TestCase("sdlxliffFile.sdlxliff")]
		[TestCase("xlsxFile.xlsx")]
		[TestCase("Source.zip")]
		[Description("S-7152"), ShortCheckList]
		public void DownloadStatisticTest(string file)
		{
			var exportFile = Path.Combine(PathProvider.SupportedFormatsFiles, file);

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { exportFile });

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			_buildStatisticsPage.ClickBuildStatisticsButton();

			_statisticsPage
				.ClickExportStatisticButton()
				.ClickTradosXlsxButton()
				.ClickExportStatisticButton()
				.ClickTradosXmlButton();

			Assert.IsTrue(_workspacePage.IsFileDownloaded("[ru] Statistics for project " + _projectUniqueName + "*.xml"),
				"Произошла ошибка: файл xml не загрузился");

			Assert.IsTrue(_workspacePage.IsFileDownloaded("[ru] Statistics for project " + _projectUniqueName + ".xlsx"),
				"Произошла ошибка: файл xlsx не загрузился");
		}

		[Test, Description("S-7153"), ShortCheckList]
		public void DownloadStatisticByAssigneesTest()
		{
			_secondUser = TakeUser(ConfigurationManager.Users);
			var groupName = Guid.NewGuid().ToString();

			_workspacePage.GoToUsersPage();
			_usersTab
				.ClickGroupsButton()
				.OpenNewGroupDialog();

			_newGroupDialog.CreateNewGroup(groupName);

			_groupsAndAccessRightsTab.OpenAddRightsDialogForGroup(groupName);

			_addAccessRightDialog
				.AddRightToGroupAnyProject(RightsType.ProjectCreation)
				.ClickSaveButton(groupName);

			_groupsAndAccessRightsTab.AddUserToGroupIfNotAlredyAdded(groupName, ThreadUser.FullName);

			_workspacePage.GoToProjectsPage();
			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				filesPaths: new[] { PathProvider.EditorTxtFile },
				tasks: new[] {
						WorkflowTask.Translation,
						WorkflowTask.Editing,
						WorkflowTask.Postediting}
				);

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(groupName, isGroup: true, taskNumber: 1)
				.SetResponsible(ThreadUser.NickName, isGroup: false, taskNumber: 2)
				.SetResponsible(_secondUser.NickName, isGroup: false, taskNumber: 3)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage.OpenAssignDialog(_projectUniqueName, PathProvider.EditorTxtFile);

			_taskAssignmentPage
				.SetResponsible(ThreadUser.NickName, taskNumber: 3)
				.ClickSaveButton();

			_workspacePage.GoToProjectsPage();

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickProjectStatisticsButtonExpectingBuildStatisticsPage(_projectUniqueName);

			_buildStatisticsPage.ClickBuildStatisticsButton();

			_statisticsPage.ClickByAssigneesButton();

			Assert.IsTrue(_workspacePage.IsFileDownloaded("Statistics by Assignees for project " + _projectUniqueName + "*.zip"),
			"Произошла ошибка: файл xml не загрузился");
		}

		[TearDown]
		public void TestFixtureTearDown()
		{
			if (_secondUser != null)
			{
				ReturnUser(ConfigurationManager.Users, _secondUser);
			}
		}

		private TestUser _secondUser;
		private AddAccessRightDialog _addAccessRightDialog;
		private GroupsAndAccessRightsTab _groupsAndAccessRightsTab;
		private NewGroupDialog _newGroupDialog;
		private UsersTab _usersTab;
		private TaskAssignmentPage _taskAssignmentPage;
		private BuildStatisticsPage _buildStatisticsPage;
		private StatisticsPage _statisticsPage;

		private SelectAssigneePage _selectAssigneePage;
	}
}
