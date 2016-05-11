using System.Collections.Generic;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Glossaries]
	class CreateGlossaryTests<TWebDriverProvider>
		: BaseGlossaryTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[Test, Description("S-7287"), ShortCheckList]
		public void CreateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");
		}

		[Test]
		public void CheckLanguageNotExistTest()
		{
			_glossariesPage.ClickCreateGlossaryButton();

			_newGlossaryDialog
				.ExpandLanguageDropdown(dropdownNumber: 1)
				.SelectLanguage(Language.German)
				.ExpandLanguageDropdown(dropdownNumber: 2);

			Assert.IsFalse(_newGlossaryDialog.IsLanguageExistInDropdown(Language.German),
				"Произошла ошибка:\n язык присутствует в дропдауне");
		}

		[Test]
		public void DeleteLanguageCreateGlossaryTest()
		{
			_glossariesPage.ClickCreateGlossaryButton();

			var languagesCountBefore = _newGlossaryDialog.GetGlossaryLanguageCount();

			_newGlossaryDialog.ClickDeleteLanguageButton(languagesCountBefore);

			var languagesCountAfter = _newGlossaryDialog.GetGlossaryLanguageCount();

			Assert.AreNotEqual(languagesCountAfter, languagesCountBefore,
				"Произошла ошибка:\n количество языков не изменилось.");
		}

		[Ignore("PRX-10784"), Test]
		public void CreatedDateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsDateModifiedMatchCurrentDate(_glossaryUniqueName),
				"Произошла ошибка:\n дата создания глоссария не совпадет с текущей датой");
		}

		[Test]
		public void ModifiedDateGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			var modifiedDateBefore = _glossariesPage.GlossaryDateModified(_glossaryUniqueName);

			_glossariesPage.ClickGlossaryRow(_glossaryUniqueName);

			// Sleep нужен, чтобы дата изменения изменилась (точность до минуты)
			Thread.Sleep(60000);

			_glossaryPage.CreateTerm();

			_workspacePage.GoToGlossariesPage();

			var modifiedDateAfter = _glossariesPage.GlossaryDateModified(_glossaryUniqueName);

			Assert.AreNotEqual(modifiedDateBefore, modifiedDateAfter,
				"Произошла ошибка:\n неверная дата изменния глоссария.");
		}

		[Test]
		public void ModifiedByAuthorTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_workspacePage.GoToGlossariesPage();

			var modifiedBy = _glossariesPage.GetModifiedByAuthor(_glossaryUniqueName);

			Assert.AreEqual(modifiedBy, ThreadUser.NickName,
				"Произошла ошибка:\n имя {0} не совпадает с {1}.", modifiedBy, ThreadUser.NickName);
		}

		[Test]
		public void CreateMultiLanguageGlossaryTest()
		{
			_glossariesHelper
				.CreateGlossary(
					_glossaryUniqueName,
					languageList: new List<Language>
						{
							 Language.German,
							 Language.French,
							 Language.Japanese,
							 Language.Lithuanian
						});

			_workspacePage.GoToGlossariesPage();

			Assert.IsTrue(_glossariesPage.IsGlossaryExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий отсутствует в списке");
		}

		[Test]
		public void OpenStructureDialogFromPropertiesDialogTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickAdvancedButton();

			Assert.IsTrue(_glossaryStructureDialog.IsGlossaryStructureDialogOpened(),
				"Произошла ошибка: не открылся диалог изменения структуры глоссария");
		}
	}
}
