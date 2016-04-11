using System;

using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings.SettingsDialog;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.UsersRights.ManageProjects.ProjectSettingsPage
{
	[Parallelizable(ParallelScope.Fixtures)]
	class FormsAndButtonsAvailabilityProjectSettingsPageTests<TWebDriverProvider> : FormsAndButtonsAvailabilityBaseTests<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUp()
		{
			_projectsPage.ClickProject(_projectUniqueName);
		}

		[Test]
		public void DeleteFileButtonProjectSettingsPageTest()
		{
			Assert.IsFalse(_projectSettingsPage.IsDeleteFileButtonDisplayed(),
				"Произошла ошибка:\n кнопка удаления файла {0} отображается на странице проекта {1}.", _projectUniqueName);
		}

		[Test]
		public void OpenProjectSettingsPageTest()
		{
			Assert.IsTrue(_projectSettingsPage.IsProjectSettingsPageOpened(),
				"Произошла ошибка:\n Страница проекта {0} не открылась.", _projectUniqueName);
		}

		[Test]
		public void AddFilesButtonTest()
		{
			Assert.IsFalse(_projectSettingsPage.IsAddFilesButtonDisplayed(),
				"Произошла ошибка:\n кнопка 'Add Files' отображается на странице проекта {0}.", _projectUniqueName);
		}
		
		[Test]
		public void QACheckButtonTest()
		{
			Assert.IsTrue(_projectSettingsPage.IsQaCheckButtonDisplayed(),
				"Произошла ошибка:\n Нет кнопки 'QA Check'.");

			_projectSettingsPage.ClickQACheckButton(_projectUniqueName);

			Assert.IsTrue(_qualityAssuranceDialog.IsQualityAssuranceDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог контроля качества.");
		}

		[Test]
		public void StatisticsButtonTest()
		{
			Assert.IsTrue(_projectSettingsPage.IsStatisticsButtonDisplayed(),
				"Произошла ошибка:\n Нет кнопки статистики.");

			_projectSettingsPage.ClickStatisticsButton();

			Assert.IsTrue(_statisticsPage.IsBuildStatisticsPageOpened(),
				"Произошла ошибка:\n Не открылся диалог статистики.");
		}

		[Test]
		public void DownloadButtonDisabledTest()
		{
			Assert.IsTrue(_projectSettingsPage.IsDownloadButtonDisabled(),
			"Произошла ошибка:\n Кнопка экспорта документа активна.");

			_projectSettingsPage.ClickDocumentCheckbox(PathProvider.DocumentFile);

			Assert.IsFalse(_projectSettingsPage.IsDownloadButtonDisabled(),
			"Произошла ошибка:\n Кнопка экспорта документа неактивна.");
		}

		[Test]
		public void AssignTaskButtonDisabledTest()
		{
			Assert.IsFalse(_projectSettingsPage.IsAssignTaskButtonDisabled(),
			"Произошла ошибка:\n Кнопка назначения пользователя на документ активна.");
			
			_projectSettingsPage.ClickDocumentCheckbox(PathProvider.DocumentFile);

			Assert.IsFalse(_projectSettingsPage.IsAssignTaskButtonDisabled(),
			"Произошла ошибка:\n Кнопка назначения пользователя на документ неактивна.");
		}

		[Test]
		public void ProjectSettingsButtonTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			Assert.IsTrue(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Диалог настроек пректа не открылся.");
		}

		[Test]
		public void TabsProjectSettingsDialogTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_settingsDialog.ClickWorkflowTab();

			Assert.IsTrue(_workflowSetUpTab.IsWorkflowSetUpTabOpened(),
				"Произошла ошибка:\n Не открылась вкладка Workflow Setup.");

			_settingsDialog.ClickGeneralTab();

			Assert.IsTrue(_generalTab.IsGeneralTabOpened(),
				"Произошла ошибка:\n Не открылась вкладка General.");
		}

		[Test]
		public void QualityAssuranceSettingsButtonTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_generalTab.ClickQualityAssuranceSettingsButton();

			Assert.IsTrue(_qualityAssuranceSettings.IsQualityAssuranceSettingsOpened(),
				"Произошла ошибка:\n Не открылись настройки контроля качества.");
		}

		[Test]
		public void CancelGeneralSettingsTest()
		{
			var deadline = DateTime.Today.AddDays(1);
			
			_projectSettingsPage.ClickSettingsButton();

			_generalTab
				.ClickRemoveDateButton()
				.OpenCalendar();

			_datePicker.SetDate<GeneralTab>(deadline.Day);

			_generalTab.CancelSettingsChanges();

			Assert.IsFalse(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Диалог настроек проекта не закрылся.");
		}

		[Test]
		public void SaveGeneralSettingsTest()
		{
			var deadline = DateTime.Today.AddDays(1);

			_projectSettingsPage.ClickSettingsButton();

			_generalTab
				.ClickRemoveDateButton()
				.OpenCalendar();
			_datePicker.SetDate<GeneralTab>(deadline.Day);

			_generalTab.SaveSettings();

			Assert.IsFalse(_settingsDialog.IsSettingsDialogOpened(),
				"Произошла ошибка:\n Диалог настроек проекта не закрылся.");
		}

		[Test]
		public void SaveQualityAssuranceSettingsTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_generalTab.ClickQualityAssuranceSettingsButton();

			_qualityAssuranceSettings
				.UncheckErrotTitleCheckbox()
				.ClickApplyButton();

			Assert.IsTrue(_generalTab.IsGeneralTabOpened(), "Произошла ошибка:\n Не открылась вкладка General.");
		}

		[Test]
		public void CancelQualityAssuranceSettingsWithoutChangesTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_generalTab.ClickQualityAssuranceSettingsButton();

			_qualityAssuranceSettings.ClickCancelButtonWithoutChanges();

			Assert.IsTrue(_generalTab.IsGeneralTabOpened(), "Произошла ошибка:\n Не открылась вкладка General.");
		}

		[Test]
		public void CancelQualityAssuranceSettingsWithChangesTest()
		{
			_projectSettingsPage.ClickSettingsButton();

			_generalTab.ClickQualityAssuranceSettingsButton();

			_qualityAssuranceSettings
				.UncheckErrotTitleCheckbox()
				.ClickCancelButtonWithChanges();

			Assert.IsTrue(_cancelConfirmationDialog.IsCancelConfirmationDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог подтверждения отмены несохраненных настроек.");

			_cancelConfirmationDialog.ClickCloseButton();

			Assert.IsTrue(_generalTab.IsGeneralTabOpened(), "Произошла ошибка:\n Не открылась вкладка General.");
		}

		[Test]
		public void DocumentSettingsButtonTest()
		{
			_projectSettingsPage
				.ClickDocumentProgress(PathProvider.DocumentFile)
				.ClickDocumentSettings();

			Assert.IsTrue(_documentSettingsDialog.IsDocumentSettingsDialogOpened(),
				"Произошла ошибка:\n не открылся диалог настроек документа.");
		}

		[Test]
		public void DocumentDeleteButtonTest()
		{
			_projectSettingsPage.ClickDocumentCheckbox(PathProvider.DocumentFile);

			Assert.IsFalse(_projectSettingsPage.IsDeleteFileButtonDisplayed(),
				"Произошла ошибка:\n Кнопка удаления файла присутсвует на странице проекта.");
		}

		[Test]
		public void PretranslateButtonTest()
		{
			_projectSettingsPage.ClickPretranslateButton();

			Assert.IsTrue(_pretranslationDialog.IsPretranslationDialogOpened(),
				"Произошла ошибка:\n Не открылся диалог предварительного перевода.");
		}

		[Test]
		public void PretranslateSaveTest()
		{
			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectResourceOption(CatType.MT)
				.ClickSaveAndRunButton();

			Assert.IsFalse(_pretranslationDialog.IsPretranslationDialogOpened(),
				"Произошла ошибка:\n Не закрылся диалог предварительного перевода.");
		}

		[Test]
		public void PretranslateCancelTest()
		{
			_projectSettingsPage.ClickPretranslateButton();

			_pretranslationDialog
				.ClickAddInsertionRuleButton()
				.SelectResourceOption(CatType.MT)
				.ClickCancelButton();

			Assert.IsFalse(_pretranslationDialog.IsPretranslationDialogOpened(),
				"Произошла ошибка:\n Не закрылся диалог предварительного перевода.");
		}
	}
}
