using System;
using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor.ProgressBarTests
{
	class ProgressBarTests<TWebDriverProvider> : BaseTest<TWebDriverProvider>
		where TWebDriverProvider : IWebDriverProvider, new()
	{
		public ProgressBarTests()
		{
			StartPage = StartPage.SignIn;
		}

		[SetUp]
		public void SetUp()
		{
			_projectUniqueName = "Test project" + Guid.NewGuid();
			_editorPage = new EditorPage(Driver);
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
			_projectSettingsPage = new ProjectSettingsPage(Driver);

			_document = PathProvider.ProgressBarTxtFile;
			_documentName = Path.GetFileNameWithoutExtension(_document);

			_loginHelper.LogInSmartCat(
				ThreadUser.Login,
				ThreadUser.NickName,
				ThreadUser.Password,
				LoginHelper.PersonalAccountName);

			_firstSegment = "Один";

			_createProjectHelper.CreateNewProject(
				_projectUniqueName,
				new[] { _document },
				personalAccount: true);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(
					_projectUniqueName,
					_documentName);
		}

		[Test, Description("S-28998"), ShortCheckList]
		public void ProgressBarVisibleInEditorTest()
		{
			Assert.IsTrue(_editorPage.IsProgressBarDisplayed(),
				"Произошла ошибка:\n В редакторе не отобразился прогресс-бар.");
		}

		[Test, Description("S-28999"), ShortCheckList]
		public void ProgressBarTooltipVisibleTest()
		{
			_editorPage.HoverProgressBar();

			Assert.IsTrue(_editorPage.IsProgressBarToolTipDisplayed(),
				"Произошла ошибка:\n В редакторе не отобразился тултип прогресс-бара.");
		}

		[Test, Description("S-28987"), ShortCheckList]
		public void WordCountAndPerecentsCorrectTest()
		{
			var expectedPercents = 10.0f;
			var expectedWords = 1;

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.HoverProgressBar();

			float percents = _editorPage.GetPercentInProgressBar();
			int words = _editorPage.GetTranslatedWordCount();

			Assert.AreEqual(expectedPercents, percents,
				"Произошла ошибка:\n Неверное процентное отображение выполненной работы - {0}.", percents);

			Assert.AreEqual(expectedWords, words,
				"Произошла ошибка:\n Не совпадает количество - {0} переведенных в тултипе слов(в сорсе).", words);
		}

		[Test, Description("S-29000"), ShortCheckList]
		public void ProgressBarIncreaseInEditorTest()
		{
			var expectedWords = 1;
			var expectedPercents = 10;

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.HoverProgressBar();

			Assert.AreEqual(expectedPercents, _editorPage.GetPercentInProgressBar(),
				"Произошла ошибка:\n Прогресс-бар не содержит ожидаемое количество процентов.");

			Assert.AreEqual(expectedWords, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Количество слов(в сорсе) из прогресс-бара не совпадает с ожидаемым.");
		}

		[Test, Description("S-29001"), ShortCheckList]
		public void WordCountAfterUnconfirmSegmentTest()
		{
			var wordsAfterConfirm = 1;
			var wordsAfterUnConfirm = 0;

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.HoverProgressBar();

			Assert.AreEqual(wordsAfterConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не совпадает количество переведенных в тултипе слов(в сорсе)"
				+ " после подтверждения сегмента");

			_editorPage
				.RemoveTextFromTargetSegment(1)
				.ConfirmSegmentTranslation();

			Assert.AreEqual(wordsAfterUnConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не совпадает количество переведенных в тултипе слов(в сорсе) после"
				+ " удаления текста в подтвержденном сегменте.");
		}

		[Test, Description("S-29002"), ShortCheckList]
		public void WordCountAfterDoubleUnconfirmSegmentTest()
		{
			var expectedTranslatedWord = 1;

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.HoverProgressBar()
				.RemoveTextFromTargetSegment(1);

			_editorPage
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.HoverProgressBar();

			Assert.AreEqual(expectedTranslatedWord, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не совпадает количество переведенных в тултипе слов(в сорсе)");
		}

		[Test, Description("S-29299"), ShortCheckList]
		public void CheckWordsInProgressBarToolTipTest()
		{
			var sourceString = "1. Translation (T): 0 words out of 10 (0%)";

			var progressBarString = _editorPage.GetTextFromProgressBar();

			Assert.AreEqual(sourceString, progressBarString,
				"Произошла ошибка:\n Не совпала эталонная строка и строка из прогресс-бара {0}.", progressBarString);
		}

		[Test, Description("S-29303"), ShortCheckList]
		public void ProjectsPageProgressBarTest()
		{
			var percentsBeforeConfirm = 0;
			var percentsAfterConfirm = 10;

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.GoToProjectsPage();

			Assert.IsTrue(_projectsPage.IsProgressBarContainsExpectedPercents(_projectUniqueName, percentsAfterConfirm),
				"Произошла ошибка: /n У прогресс-бара отличный от {0} прогресс перевода", percentsAfterConfirm);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(
					_projectUniqueName,
					PathProvider.ProgressBarTxtFile);

			_editorPage
				.RemoveTextFromTargetSegment(1)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.GoToProjectsPage();

			Assert.IsTrue(_projectsPage.IsProgressBarContainsExpectedPercents(_projectUniqueName, percentsBeforeConfirm),
				"Произошла ошибка: /n У прогресс-бара отличный от {0} прогресс перевода", percentsBeforeConfirm);
		}

		[Test, Description("S-29301"), ShortCheckList]
		public void ProgressBarIncreaseOnProjectsPageTest()
		{
			var percentsBeforeConfirm = 0;
			var percentsAfterConfirm = 10;

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.GoToProjectsPage();

			Assert.IsTrue(_projectsPage.IsProgressBarContainsExpectedPercents(_projectUniqueName, percentsBeforeConfirm),
				"Произошла ошибка:\n У прогресс-бара процент выполнения отличный от {0}", percentsBeforeConfirm);

			_projectsPage
				.OpenProjectInfo(_projectUniqueName)
				.ClickDocumentRefExpectingEditorPage(
					_projectUniqueName,
					PathProvider.ProgressBarTxtFile);

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			_projectSettingsPage.GoToProjectsPage();

			Assert.IsTrue(_projectsPage.IsProgressBarContainsExpectedPercents(_projectUniqueName, percentsAfterConfirm),
				"Произошла ошибка:\n У прогресс-бара процент выполнения отличный от {0}", percentsAfterConfirm);
		}

		[Test, Description("S-29302"), ShortCheckList]
		public void ProgressBarIncreaseOnProjectSettingsPageTest()
		{
			var percentsBeforeConfirm = 0;
			var percentsAfterConfirm = 10;
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.ProgressBarTxtFile);

			_editorPage.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.IsTrue(_projectSettingsPage.IsProgressBarContainsExpectedPercents(fileName, percentsBeforeConfirm),
				"Произошла ошибка:\n У прогресс-бара процент выполнения отличный от {0}", percentsBeforeConfirm);

			_projectSettingsPage.OpenDocumentInEditorWithoutTaskSelect(fileName);
			
			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.IsTrue(_projectSettingsPage.IsProgressBarContainsExpectedPercents(fileName, percentsAfterConfirm),
				"Произошла ошибка:\n У прогресс-бара процент выполнения отличный от {0}", percentsAfterConfirm);
		}

		[Test, Description("S-29003"), ShortCheckList]
		public void ProgressAfterConfirnAndUnconfirmsSegementsTest()
		{
			var wordsBeforeConfirm = 0;
			var wordsAfterUnConfirm = 3;
			var wordsAfterConfirm = 5;
			var one = "Один";
			var two = "Два";
			var three = "Три";
			var four = "Четыре";
			var five = "Пять";

			_editorPage
				.FillSegmentTargetField(text: one, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: two, rowNumber: 2)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: three, rowNumber: 3)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: four, rowNumber: 4)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: five, rowNumber: 5)
				.ConfirmSegmentTranslation();

			Assert.AreEqual(wordsAfterConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не изменилось количество переведённых слов"
				+ " после подтверждения 5 сегментов.");

			_editorPage
				.RemoveTextFromTargetSegment(rowNumber: 1)
				.ConfirmSegmentTranslation()
				.RemoveTextFromTargetSegment(rowNumber: 2)
				.ConfirmSegmentTranslation()
				.RemoveTextFromTargetSegment(rowNumber: 3)
				.ConfirmSegmentTranslation()
				.RemoveTextFromTargetSegment(rowNumber: 4)
				.ConfirmSegmentTranslation()
				.RemoveTextFromTargetSegment(rowNumber: 5)
				.ConfirmSegmentTranslation();

			Assert.AreEqual(wordsBeforeConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не изменилось количество переведённых слов"
				+ " после удаления 5 сегментов.");

			_editorPage
				.FillSegmentTargetField(text: one, rowNumber: 1)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: two, rowNumber: 2)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: three, rowNumber: 3)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: four, rowNumber: 4)
				.ConfirmSegmentTranslation()
				.FillSegmentTargetField(text: five, rowNumber: 5)
				.ConfirmSegmentTranslation();

			Assert.AreEqual(wordsAfterConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не изменилось количество переведённых слов"
				+ " после подтверждения 5 сегментов.");

			_editorPage
				.RemoveTextFromTargetSegment(rowNumber: 1)
				.ConfirmSegmentTranslation()
				.RemoveTextFromTargetSegment(rowNumber: 3)
				.ConfirmSegmentTranslation();

			Assert.AreEqual(wordsAfterUnConfirm, _editorPage.GetTranslatedWordCount(),
				"Произошла ошибка:\n Не изменилось количество переведённых слов"
				+ " после удаления 2 из 5 сегментов.");
		}

		[Test, Description("S-29300"), ShortCheckList]
		public void ProgressBarAfterConfirmSegmentAtProjectSettingsPageTest()
		{
			var percentsAfterConfirm = 10;
			var fileName = Path.GetFileNameWithoutExtension(PathProvider.ProgressBarTxtFile);

			_editorPage
				.FillSegmentTargetField(text: _firstSegment)
				.ConfirmSegmentTranslation()
				.ClickHomeButtonExpectingProjectSettingsPage();

			Assert.IsTrue(_projectSettingsPage.IsProgressBarContainsExpectedPercents(fileName, percentsAfterConfirm),
				"Произошла ошибка:\n У прогресс-бара процент выполнения отличный от {0}", percentsAfterConfirm);
		}

		private string _document;
		private string _documentName;
		private string _firstSegment;
		private string _projectUniqueName;

		private EditorPage _editorPage;
		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
		private ProjectSettingsPage _projectSettingsPage;
	}
}