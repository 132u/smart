﻿using System;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.TranslationMemories
{
	[Standalone]
	class TMExportTests<TWebDriverProvider> : BaseTmTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[TestCase(true)]
		[TestCase(false)]
		public void ExportClearTMButtonTest(bool withImportFile)
		{
			var tmName = "TranslationMemory";

			TranslationMemoriesHelper
				.GetTranslationMemoryUniqueName(ref tmName)
				.CreateTranslationMemory(tmName, importFilePath: withImportFile ? PathProvider.TMTestFile2 : null)
				.AssertTranslationMemoryExists(tmName)
				.ExportTM(tmName);

			_exportFileHelper
				.AssertFileDownloaded(String.Format("{0}*-export.tmx", tmName));
		}

		private readonly ExportFileHelper _exportFileHelper = new ExportFileHelper();
	}
}