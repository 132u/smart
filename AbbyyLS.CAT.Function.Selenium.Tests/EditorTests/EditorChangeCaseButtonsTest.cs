using NUnit.Framework;
using System.Collections.Generic;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.ChangeCase
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorChangeCaseButtonsTest : EditorBaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public EditorChangeCaseButtonsTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для всего текста
		/// </summary>
		[Test]
		public void ChangeCaseTextButtonTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "the example sentence");
			// Нажать хоткей выделения всего содержимого ячейки
			EditorPage.SelectAlltextByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для всего текста
		/// </summary>
		[Test]
		public void ChangeCaseTextHotkeyTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "the example sentence");
			// Нажать хоткей выделения всего содержимого ячейки
			EditorPage.SelectAlltextByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("the example sentence", "The Example Sentence", "THE EXAMPLE SENTENCE", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectLastWordByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectLastWordByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some words for example", "some words for Example", "some words for EXAMPLE", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки и хоткея изменения регистра для первого слова
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordTestByHotKey()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some words for example", "Some words for example", "SOME words for example", false, segmentNumber);

		}

		[Test]
		public void ChangeCaseFirstWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по кнопке
			CheckChangeCase("some words for example", "Some words for example", "SOME words for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слов через дефис 
		/// </summary>
		[Test]
		public void ChangeCaseHyphenWordTestByHotKey()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some-words for example", "Some-Words for example", "SOME-WORDS for example", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слов через дефис 
		/// </summary>
		[Test]
		public void ChangeCaseHyphenWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перехода в начало строки
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			// Нажать хоткей выделения первого слова
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some-words for example", "Some-Words for example", "SOME-WORDS for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для части слова
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByBtn()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some-words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по кнопке
			CheckChangeCase("some words for example", "some words for eXample", "some words for eXAMple", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для части слова
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByHotKey()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some words for example", "some words for eXample", "some words for eXAMple", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для части слова ЭТО ВРЕМЕННЫЙ ТЕСТ ПОКА НЕ БУДЕТ ПОФИКШЕН БАГ PRX-4037
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByHotKeyCurentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some words for eXAMple", "some words for eXAMple", "some words for example", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для части слова  ЭТО ВРЕМЕННЫЙ ТЕСТ ПОКА НЕ БУДЕТ ПОФИКШЕН БАГ PRX-4037
		/// </summary>
		[Test]
		public void ChangeCasePartWordTestByBtnCurentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some words for example");
			// Нажать хоткей перемещения курсора к четвертому слову
			EditorPage.PutCursorAfterThirdWordByHotkey(segmentNumber);
			// Нажать хоткей выделения трех символов в слове
			EditorPage.SelectNextThreeSymbolsByHotkey(segmentNumber);
			// Запустить проверку по хоткею
			CheckChangeCase("some words for eXAMple", "some words for eXAMple", "some words for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого) 
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonNonStandardTest()
		{
			int segmentNumber = 1;
			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения второго и третьего слов
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку кнопка
			CheckChangeCase("some WORDS FOR example", "some words for example", "some Words For example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого)
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения второго и третьего слов
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку хоткей
			CheckChangeCase("some WORDS FOR example", "some words for example", "some Words For example", false, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова первого
		/// </summary>
		[Category("PRX_8449")]
		[Test]
		public void ChangeCaseFirstWordButtonNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some words for example", "Some words for example", "SOME Words For example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова первого
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordHotkeyNonStandardTest()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some words for example", "Some words for example", "SOME Words For example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова (не первого) текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordButtonNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some Words For example", "some WORDS FOR example", "some words for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова (не первого) текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseSomeWordHotkeyNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "some wOrDs fOr example");
			// Нажать хоткей выделения последнего слова
			EditorPage.SelectSecondThirdWordsByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("some Words For example", "some WORDS FOR example", "some words for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования кнопки изменения регистра для слова первого текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordButtonNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("Some words for example", "SOME words for example", "some words for example", true, segmentNumber);
		}

		/// <summary>
		/// Метод тестирования хоткея изменения регистра для слова первого текущая реализация
		/// </summary>
		[Test]
		public void ChangeCaseFirstWordHotkeyNonStandardTestCurrentRealization()
		{
			int segmentNumber = 1;

			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(segmentNumber, "sOMe words for example");
			EditorPage.CursorToTargetLineBeginningByHotkey(segmentNumber);
			EditorPage.SelectFirstWordTargetByHotkey(segmentNumber);
			// Запустить проверку
			CheckChangeCase("Some words for example", "SOME words for example", "some words for example", true, segmentNumber);
		}

		/// <summary>
		/// Проверить изменение регистра
		/// </summary>
		/// <param name="sourceText">начальный текст</param>
		/// <param name="textAfterFirstChange">текст после первого изменения</param>
		/// <param name="textAfterSecondChange">текст после второго изменения</param>
		/// <param name="byButtonTrueByHotkeyFalse">по кнопке или по хоткею</param>
		/// <param name="segmentNumber">порядковый номер сегмента</param>
		protected void CheckChangeCase(
			string sourceText,
			string textAfterFirstChange,
			string textAfterSecondChange,
			bool byButtonTrueByHotkeyFalse,
			int segmentNumber)
		{
			// Список текстов для сравнения после изменения регистра
			var listToCompare = new List<string> { textAfterFirstChange, textAfterSecondChange, sourceText };

			for (int i = 0; i < listToCompare.Count; ++i)
			{
				// Нажать изменениe регистра
				ClickChangeCase(byButtonTrueByHotkeyFalse, segmentNumber);

				// Убедиться, что регистр слова изменился правильно - сравнить со значением в listToCompare
				Assert.AreEqual(
					listToCompare[i],
					EditorPage.GetTargetText(segmentNumber),
					"Ошибка: не совпал текст при изменении регистра");
			}
		}

		/// <summary>
		/// Нажать Изменить регистр
		/// </summary>
		/// <param name="byButtonTrueByHotkeyFalse">true - по кнопке, false - по хоткею</param>
		/// <param name="segmentNumber">номер сегмента</param>
		protected void ClickChangeCase(bool byButtonTrueByHotkeyFalse, int segmentNumber)
		{
			if (byButtonTrueByHotkeyFalse)
			{
				// Нажать кнопку
				EditorPage.ClickChangeCaseBtn();
			}
			else
			{
				// Нажать хоткей
				EditorPage.ChangeCaseByHotkey(segmentNumber);
			}
		}
	}
}