using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class ExportGlossaryTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7294"), ShortCheckList]
		public void ExportGlossaryTest()
		{
			_exportNotification.CloseAllNotifications<GlossariesPage>();

			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.CreateTerm("secondTerm1", "secondTerm2")
				.ClickExportGlossary();

			_exportNotification.ClickDownloadNotifier<GlossaryPage>();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(
				Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");
		}
	}
}
