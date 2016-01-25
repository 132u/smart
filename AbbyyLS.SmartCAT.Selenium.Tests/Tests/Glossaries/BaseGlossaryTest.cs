using System.Collections.Generic;
using System.IO;
using System.Threading;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Glossaries;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Workspace;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	public class BaseGlossaryTest<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void BaseGlossaryTestSetUp()
		{
			_workspacePage = new WorkspacePage(Driver);
			_glossaryPage = new GlossaryPage(Driver);
			_glossariesPage = new GlossariesPage(Driver);
			_newGlossaryDialog = new NewGlossaryDialog(Driver);
			_glossaryPropertiesDialog = new GlossaryPropertiesDialog(Driver);
			_glossaryImportDialog = new GlossaryImportDialog(Driver);
			_glossarySuccessImportDialog = new GlossarySuccessImportDialog(Driver);
			_glossaryStructureDialog = new GlossaryStructureDialog(Driver);
			_glossariesHelper = new GlossariesHelper(Driver);
			_filterDialog = new FilterDialog(Driver);
			_glossaryUniqueName = GlossariesHelper.UniqueGlossaryName();

			_workspacePage.GoToGlossariesPage();
		}

		[Test]
		public void DeleteGlossaryTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickDeleteGlossaryButton();

			Assert.IsTrue(_glossaryPropertiesDialog.IsConfirmDeleteMessageDisplayed(),
				"Произошла ошибка: \nне появилось сообщение с подтверждением удаления глоссария");

			_glossaryPropertiesDialog.ClickConfirmDeleteGlossaryButton();

			Assert.IsTrue(_glossariesPage.IsGlossaryNotExist(_glossaryUniqueName),
				"Произошла ошибка:\n глоссарий присутствует в списке.");
		}

		[Test]
		public void DeleteLanguageExistTermTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage
				.CreateTerm()
				.OpenGlossaryProperties();

			var languagesCountBefore = _glossaryPropertiesDialog.LanguagesCount();

			_glossaryPropertiesDialog.CancelDeleteLanguageInPropertiesDialog();

			var languagesCountAfter = _glossaryPropertiesDialog.LanguagesCount();

			Assert.AreEqual(languagesCountBefore, languagesCountAfter,
				"Произошла ошибка:\n количество языков {0} не совпадает с {1} в диалоге свойств глоссария.",
				languagesCountBefore, languagesCountAfter);
		}

		[Test]
		public void NewEntryEditModeTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.ClickNewEntryButton();
				
			Assert.IsTrue(_glossaryPage.IsCreationModeActivated(),
				"Произошла ошибка:\n новый термин не открыт в расширенном режиме");
		}

		[Test]
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

		[Test]
		public void OpenStructureDialogFromPropertiesDialogTest()
		{
			_glossariesHelper.CreateGlossary(_glossaryUniqueName);

			_glossaryPage.OpenGlossaryProperties();

			_glossaryPropertiesDialog.ClickAdvancedButton();

			Assert.IsTrue(_glossaryStructureDialog.IsGlossaryStructureDialogOpened(),
				"Произошла ошибка: не открылся диалог изменения структуры глоссария");
		}

		protected GlossariesHelper _glossariesHelper;
		protected WorkspacePage _workspacePage;
		protected GlossaryPage _glossaryPage;
		protected GlossariesPage _glossariesPage;
		protected NewGlossaryDialog _newGlossaryDialog;
		protected GlossaryPropertiesDialog _glossaryPropertiesDialog;
		protected GlossaryImportDialog _glossaryImportDialog;
		protected GlossarySuccessImportDialog _glossarySuccessImportDialog;
		protected GlossaryStructureDialog _glossaryStructureDialog;
		protected FilterDialog _filterDialog;
		protected string _glossaryUniqueName;
	}
}
