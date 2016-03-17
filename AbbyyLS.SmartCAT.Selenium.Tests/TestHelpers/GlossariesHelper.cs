﻿using System;
using System.Collections.Generic;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class GlossariesHelper
	{
		public WebDriver Driver { get; private set; }

		public GlossariesHelper(WebDriver driver)
		{
			Driver = driver;

			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
		}

		public static string UniqueGlossaryName()
		{
			return "TestGlossary" + "-" + Guid.NewGuid();
		}

		public GlossariesHelper CreateGlossary(
			string glossaryName,
			string comment = "Test Glossary Generated by Selenium",
			bool errorExpected = false,
			List<Language> languageList = null,
			string projectGroupName = null,
			string client = null)
		{
			_glossariesPage
				.ClickCreateGlossaryButton()
				.FillGlossaryName(glossaryName)
				.FillComment(comment);

			if (languageList != null && languageList.Count > 0)
			{
				_newGlossaryDialog
					.ClickDeleteLanguageButton()
					.ExpandLanguageDropdown(1)
					.SelectLanguage(languageList[0]);

				for (var i = 2; i <= languageList.Count; i++)
				{
					_newGlossaryDialog
						.ClickAddButton()
						.ExpandLanguageDropdown(i)
						.SelectLanguage(languageList[i - 1]);
				}
			}

			if (projectGroupName != null)
			{
				_newGlossaryDialog
					.ClickProjectGroupsList()
					.SelectProjectGroup(projectGroupName)
					.ClickProjectGroupsList();
			}

			if (client != null)
			{
				_newGlossaryDialog
					.ClickClientsList()
					.SelectClient(client)
					.ClickComment();
			}

			if (errorExpected)
			{
				_newGlossaryDialog.ClickSaveGlossaryButtonExpectingError();
			}
			else
			{
				_newGlossaryDialog.ClickSaveGlossaryButton();
			}

			return this;
		}

		private readonly GlossariesPage _glossariesPage;
		private readonly NewGlossaryDialog _newGlossaryDialog;
	}
}