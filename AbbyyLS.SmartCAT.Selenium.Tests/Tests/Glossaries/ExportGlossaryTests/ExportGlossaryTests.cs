using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	class ExportGlossaryTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7294")]
		public void ExportGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.CreateTerm("secondTerm1", "secondTerm2")
				.ClickExportGlossary();

			Assert.IsTrue(_glossaryPage.IsGlossaryExportedSuccesfully(
				Path.Combine(PathProvider.ExportFiles, _glossaryUniqueName.Replace(":", "-") + ".xlsx")),
				"Произошла ошибка:\n файл не был скачан за отведенное время");
		}
	}
}
