using AbbyyLS.SmartCAT.Selenium.Tests.Pages.Editor;
using AbbyyLS.SmartCAT.Selenium.Tests.TestFramework;
using NUnit.Framework;

namespace AbbyyLS.SmartCAT.Selenium.Tests.TestHelpers
{
	public class EditorHelper
	{
		private readonly SelectTaskDialog _selectTask = new SelectTaskDialog();
		private readonly EditorPage _editorPage = new EditorPage();

		/// <summary>
		/// Выбрать задание (перевод или просмотр)
		/// </summary>
		/// <param name="mode">задание</param>
		public EditorHelper SelectTask(string mode = "translate")
		{
			BaseObject.InitPage(_selectTask);
			if (mode.Contains("translate"))
			{
				_selectTask.ClickTranslateBtn()
					.ClickContinueBtn();
			}
			else
			{
				_selectTask.ClickManagerBtn()
					.ClickContinueBtn();
			}

			return this;
		}

		/// <summary>
		/// Подтвердить перевод сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorHelper ConfirmTranslation(int rowNumber)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickConfirmBtn()
				.AssertIsSegmentConfirmed(rowNumber);

			return this;
		}

		/// <summary>
		/// Перемещаемся к следующему неподтвержденному сегменту  и получаем его номер
		/// </summary>
		public int MoveToNextSegmentGetRowNumber()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickUnfinishedBtn();

			return _editorPage.GetRowNumberActiveSegment();
		}

		/// <summary>
		/// Прокрутка до нужного сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorHelper ScrollToRequaredElement(int rowNumber)
		{
			while (!_editorPage.IsSegmentVisible(rowNumber))
			{
				var firstVisibleSegment = _editorPage.GetRowNumberFirstVisibleSegment();
				if (firstVisibleSegment < rowNumber)
				{
					_editorPage.ClickLastVisibleSegment();
				}
				else
				{
					_editorPage.ClickFirstVisibleSegment();
				}
			}

			return this;
		}


		/// <summary>
		/// Выбрать таргет сегмента
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorHelper SelectSegmentTarget(int rowNumber = 1)
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickTargetCell(rowNumber);

			return this;
		}

		/// <summary>
		/// Вернуться на страницу проекта
		/// </summary>
		public ProjectSettingsHelper GoBack()
		{
			BaseObject.InitPage(_editorPage);
			_editorPage.ClickHomeBtn();
			var projectSettingsHelper = new ProjectSettingsHelper();

			return projectSettingsHelper;
		}

		/// <summary>
		/// Проверка, заблокирован ли сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		/// <param name="locked">должен ли быть заблокирован</param>
		public EditorHelper AssertSegmentIsLocked(int rowNumber, bool locked = true)
		{
			if (locked)
			{
				Assert.IsTrue(_editorPage.IsSegmentLocked(rowNumber),
					"Произошла ошибка:\n сегмент с номером {0} не заблокирован.", rowNumber);
			}
			else
			{
				Assert.IsFalse(_editorPage.IsSegmentLocked(rowNumber),
					"Произошла ошибка:\n сегмент с номером {0} заблокирован.", rowNumber);
			}

			return this;
		}

		/// <summary>
		/// Проверка, нужный ли номер имеет активный сегмент
		/// </summary>
		/// <param name="rowNumber">номер сегмента</param>
		public EditorHelper AssertActiveSegmentHasRequiredNumber(int rowNumber)
		{
			Assert.AreEqual(_editorPage.GetRowNumberActiveSegment(), rowNumber,
				"Произошла ошибка:\n номер активного сегмента {0} отличен от предпологаемого {1}.",
				_editorPage.GetRowNumberActiveSegment(), rowNumber);

			return this;
		}
	}
}
