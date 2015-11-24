using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Projects
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	class DocumentFormatTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_createProjectHelper = new CreateProjectHelper(Driver);
			_projectsPage = new ProjectsPage(Driver);
		}

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
		[TestCase("idmlFile.idml", Ignore = "SCAT-665")]
		[TestCase("sdlxliffFile.sdlxliff")]
		[TestCase("xlsxFile.xlsx")]
		[TestCase("Source.zip")]
		public void DocumentFormatsTest(string file)
		{
			var projectUniqueName = _createProjectHelper.GetProjectUniqueName();

			_createProjectHelper.CreateNewProject(projectUniqueName,
				filePath: Path.Combine(PathProvider.FilesForStandaloneDifferentFormatsFolder, file));

			Assert.IsTrue(_projectsPage.IsProjectAppearInList(projectUniqueName),
				"Произошла ошибка:\n проект {0} не появился в списке проектов.", projectUniqueName);

			Assert.IsTrue(_projectsPage.IsProjectLoaded(projectUniqueName),
				"Произошла ошибка: не исчезла пиктограмма загрузки проекта");

			Assert.IsFalse(_projectsPage.IsFatalErrorSignDisplayed(projectUniqueName),
				"Произошла ошибка: появилась пиктограмма ошибки напротив проекта");

			Assert.IsFalse(_projectsPage.IsWarningSignDisplayed(projectUniqueName),
				"Произошла ошибка: появилась пиктограмма предупреждения напротив проекта");
		}

		private CreateProjectHelper _createProjectHelper;
		private ProjectsPage _projectsPage;
	}
}
