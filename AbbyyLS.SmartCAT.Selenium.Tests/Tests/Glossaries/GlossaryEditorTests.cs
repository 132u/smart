using System.IO;

using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.DataStructures;
using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Projects.ProjectSettings;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Glossaries
{
	[TestFixture]
	[Standalone]
	class GlossaryEditorTests<TWebDriverProvider> : BaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void GlossariesSetUp()
		{
			_projectName = _createProjectHelper.GetProjectUniqueName();
			_glossaryName = GlossariesHelper.UniqueGlossaryName();

			WorkspaceHelper
				.GoToUsersRightsPage()
				.ClickGroupsButton()
				.CheckOrAddUserToGroup("Administrators", NickName)
				.GoToProjectsPage();

			_createProjectHelper
				.CreateNewProject(_projectName, createGlossary: true, glossaryName: _glossaryName)
				.GoToProjectSettingsPage(_projectName)
				.UploadDocument(PathProvider.DocumentFile)
				.RefreshPage<ProjectSettingsPage, ProjectSettingsHelper>()
				.AssignTasksOnDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), NickName)
				//TODO: Убрать шаг AddGlossaryToDocument, когда пофиксят PRX-11398
				.AddGlossaryToDocument(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile), _glossaryName)
				.AssertDialogBackgroundDisappeared()
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.CloseTutorialIfExist();
		}

		/// <summary>
		/// Открывает форму добавления термина в редакторе по нажатию кнопки на панели
		/// </summary>
		[Test]
		public void OpenAddTermFormByButton()
		{
			_editorHelper.OpenAddTermDialog();
		}

		/// <summary>
		/// Открывает форму добавления термина в редакторе по хоткею
		/// </summary>
		[Test]
		public void OpenAddTermFormByHotKey()
		{
			_editorHelper.OpenAddTermDialogWithHotKey();
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в сорсе
		/// </summary>
		[Test]
		public void AutofillAddTermFormWithSelectedSourceWord()
		{
			var word = _editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(word)
				.ConfirmAdditionTermWithoutTranslation();
		}

		/// <summary>
		/// Проверка автозапослнения формы при выделенном слове в тагрет
		/// </summary>
		[Test]
		public void AutofillAddTermFormWithSelectedTargetWord()
		{
			var word = _editorHelper
				.AddTextToSegment("Town")
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(target: word)
				.ConfirmAdditionTermWithoutTranslation();
		}

		/// <summary>
		/// Добавить одиночный термин из сорса в глоссарий
		/// </summary>
		[Test]
		public void AddSingleTermFromSourceToGlossary()
		{
			var word = _editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(word)
				.ConfirmAdditionTermWithoutTranslation()
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, word);
		}

		/// <summary>
		/// Добавить одиночный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSingleTermFromTargetToGlossary()
		{
			var word = _editorHelper
				.AddTextToSegment("Town")
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(target: word)
				.ConfirmAdditionTermWithoutTranslation()
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, "", word);
		}

		/// <summary>
		/// Добавить термин с сорсом и таргетом в глоссарий
		/// </summary>
		[Test]
		public void AddSourceTargetTermToGlossary()
		{
			var source = "Comet";
			var target = "Комета";

			_editorHelper
				.AddNewTerm(source, target)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
		}

		/// <summary>
		/// Добавить термин из сорса с таргетом в глоссарий игнорируя автоподстановку
		/// </summary>
		[Test]
		public void AddModifiedSourceTargetTermToGlossary()
		{
			var source = "Comet";
			var target = "Комета";
			var word = _editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Source)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(word)
				.FillAddTermForm(source, target)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
		}

		/// <summary>
		/// Добавить измененный термин из таргета в глоссарий
		/// </summary>
		[Test]
		public void AddSourceModifiedTargetTermToGlossary()
		{
			var source = "Comet";
			var target = "Комета";
			var word = _editorHelper
				.AddTextToSegment("Town")
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.GetFirstWordInSegment();

			_editorHelper
				.SelectFirstWordInSegment(1, SegmentType.Target)
				.OpenAddTermDialog()
				.CheckAutofillInAddTermDialog(target: word)
				.FillAddTermForm(source, target)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в сорс 
		/// </summary>
		[Test]
		public void AddExistedSourceTermToGlossary()
		{
			var source = "planet";
			var target = "планета";
			var modifiedTarget = "планетка";

			_editorHelper
				.AddNewTerm(source, target)
				.OpenAddTermDialog()
				.FillAddTermForm(source, modifiedTarget)
				.ConfirmAdditionExistedTerm()
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, modifiedTarget, termsCount: 2);
		}

		/// <summary>
		/// Добавить в глоссарий термин уже существующий в таргет
		/// </summary>
		[Test]
		public void AddExistedTargetTermToGlossary()
		{
			var source = "asteroid";
			var modifiedSource = "the asteroid";
			var target = "астероид";

			_editorHelper
				.AddNewTerm(source, target)
				.OpenAddTermDialog()
				.FillAddTermForm(modifiedSource, target)
				.ConfirmAdditionExistedTerm()
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, modifiedSource, target, termsCount: 2);
		}

		/// <summary>
		/// Добавить в глоссарий абсолютно идентичный термин
		/// </summary>
		[Test]
		public void AddExistedTermToGlossary()
		{
			var source = "sun";
			var target = "солнце";

			_editorHelper
				.AddNewTerm(source, target)
				.OpenAddTermDialog()
				.FillAddTermForm(source, target)
				.ConfirmAdditionExistedTerm()
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target, termsCount: 2);
		}

		/// <summary>
		/// Добавить в глоссарий термин с комментарием
		/// </summary>
		[Test]
		public void AddTermWithCommentToGlossary()
		{
			var source = "Neptun";
			var target = "Нептун";
			var comment = "Generated By Selenium";

			_editorHelper
				.AddNewTerm(source, target, comment)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target, comment);
		}

		/// <summary>
		/// Добавление удаленного термина в глоссарий
		/// </summary>
		[Test]
		public void DeleteAddTermToGlossary()
		{
			var source = "Galaxy";
			var target = "Галактика";

			_editorHelper
				.AddNewTerm(source, target)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target)
				.DeleteTerm(source)
				.GoToProjectsPage()
				.GoToProjectSettingsPage(_projectName)
				.OpenDocument<SelectTaskDialog>(Path.GetFileNameWithoutExtension(PathProvider.DocumentFile))
				.SelectTask()
				.AddNewTerm(source, target)
				.ClickHomeButton()
				.GoToGlossariesPage()
				.CheckTermInGlossary(_glossaryName, source, target);
		}

		private readonly CreateProjectHelper _createProjectHelper = new CreateProjectHelper();
		private readonly EditorHelper _editorHelper = new EditorHelper();

		private string _projectName;
		private string _glossaryName;
	}
}
