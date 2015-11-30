using NUnit.Framework;

using AbbyyLS.SmartCAT.Selenium.Tests.Drivers;
using AbbyyLS.SmartCAT.Selenium.Tests.FeatureAttributes;
using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;

namespace AbbyyLS.SmartCAT.Selenium.Tests.Tests.Editor
{
	[Parallelizable(ParallelScope.Fixtures)]
	[Standalone]
	public class EditorErrorTests<TWebDriverProvider> : EditorBaseTest<TWebDriverProvider> where TWebDriverProvider : IWebDriverProvider, new()
	{
		[SetUp]
		public void Initialization()
		{
			_errorsDialog = new ErrorsDialog(Driver);
		}

		[Test(Description = "Проверяет открытие диалога поиска ошибок с помощью кнопки")]
		public void FindErrorButtonTest()
		{
			EditorPage.ClickFindErrorButton();

			Assert.IsTrue(_errorsDialog.IsErrorsDialogOpened(),
				"Произошла ошибка:\n не появился диалог поиска ошибок");
		}

		[Test(Description = "Проверяет открытие диалога поиска ошибок нажатием F7")]
		public void FindErrorHotkeyTest()
		{
			EditorPage.OpenFindErrorsDialogByHotkey();

			Assert.IsTrue(_errorsDialog.IsErrorsDialogOpened(),
				"Произошла ошибка:\n не появился диалог поиска ошибок");
		}

		private ErrorsDialog _errorsDialog;
	}
}
