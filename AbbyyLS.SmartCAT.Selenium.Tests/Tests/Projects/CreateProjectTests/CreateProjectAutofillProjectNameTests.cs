using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[PriorityMajor]
	[Standalone]
	[Projects]
	class CreateProjectAutofillProjectNameTests<TWebDriverProvider> : BaseProjectTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void SetUpCreateProjectAutofillProjectNameTests()
		{
			_filePath = PathProvider.DocumentFile;
			_fileName = Path.GetFileName(PathProvider.DocumentFile);
			_projectName = Path.GetFileNameWithoutExtension(PathProvider.DocumentFile);
		}

		[Test(Description = "Проверяет автозаполнение имени проекта")]
		public void AutofillProjectName()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new []{ _filePath })
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет автозаполнение имени проекта по первому файлу")]
		public void AutofillProjectNameAddTwoFiles()
		{
			var secondFilePath = PathProvider.DocumentFile2;

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { _filePath, secondFilePath })
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(_projectName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет отсутствие автозаполнения при удалении единственного файла")]
		public void AutofillProjectNameDeleteFile()
		{
			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { _filePath })
				.DeleteDocument(_fileName)
				.ClickSkipDocumentUploadButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(string.Empty),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		[Test(Description = "Проверяет автозаполнение имени проекта по второму файлу")]
		public void AutofillProjectNameDeleteOneFile()
		{
			var secondFilePath = PathProvider.DocumentFile2;
			var secondFileName = Path.GetFileNameWithoutExtension(secondFilePath);

			_projectsPage.ClickCreateProjectButton();

			_newProjectDocumentUploadPage
				.UploadDocumentFiles(new[] { _filePath, secondFilePath })
				.DeleteDocument(_fileName)
				.ClickSettingsButton();

			Assert.IsTrue(_newProjectDocumentUploadPage.IsProjectNameMatchExpected(secondFileName),
				"Произошла ошибка:\n имя проекта не совпадает с ожидаемым");
		}

		private string _fileName;
		private string _filePath;
		private string _projectName;
	}
}
