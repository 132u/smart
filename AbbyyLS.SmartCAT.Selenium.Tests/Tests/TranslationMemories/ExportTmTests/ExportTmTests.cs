using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	[TranslationMemories]
	class ExportTmTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_exportNotification = new ExportNotification(Driver);
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ExportClearTMButtonTest(bool withImportFile)
		{
			TranslationMemoriesHelper
				.CreateTranslationMemory(UniqueTMName, importFilePath: withImportFile ? PathProvider.TmxFile : null);

			TranslationMemoriesPage.ExportTM(UniqueTMName);

			Assert.IsTrue(_exportNotification.IsFileDownloaded(String.Format("{0}*-export.tmx", UniqueTMName)),
				"Произошла ошибка: файл не загрузился");
		}

		protected ExportNotification _exportNotification;
	}
}
