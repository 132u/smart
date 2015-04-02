﻿using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Editor.Confirm
{
	/// <summary>
	/// Группа тестов кнопок редактора
	/// </summary>
	[Category("Standalone")]
	public class EditorConfirmTest<TWebDriverSettings> : EditorBaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования кнопки подтвеждения сегмента
		/// </summary>
		[Test]
		public void ConfirmButtonTest()
		{
			// Добавить текст, подтвердить, проверка подтверждвения
			AddTranslationAndConfirm();
		}

		/// <summary>
		/// Проверка работы в редакторе Confirm по хоткею Ctrl+Enter
		/// </summary>
		[Test]
		[Category("SCAT_102")]
		public void ConfirmHotkeyTest()
		{
			// Написать текст в первом сегменте в target
			EditorPage.AddTextTarget(1, "some words for example");
			EditorPage.PressHotKey(1, OpenQA.Selenium.Keys.Control + OpenQA.Selenium.Keys.Return);
			// Убедиться что сегмент подтвержден
			Assert.IsTrue(WaitSegmentConfirm(1), "Ошибка: Подтверждение (Confirm) не прошло");
		}

	}
}