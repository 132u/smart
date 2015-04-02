﻿using System.IO;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMExportTests<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования кнопки Export в открывающейся информации о пустой ТМ
		/// </summary>
		[Test]
		public void ExportClearTMButtonTest()
		{
			Logger.Info("Начало работы теста ExportClearTMButtonTest().");

			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			QuitDriverAfterTest = true;

			CreateTMByNameAndSave(UniqueTmName);

			// Отрыть информацию о ТМ и нажать кнопку экспорт
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Export);

			// Экспортировать - Assert внутри
			MoveTMFile();
		}

		/// <summary>
		/// Метод тестирования кнопки Export в открывающейся информации о ТМ с загруженным TMX (по списку ТМХ файлов для загрузки)
		/// </summary>
		/// <param name="importTMXFile">путь в файлу, импортируемого в проект</param>
		[Test, TestCaseSource("tmxFileList")]
		public void ExportTMXTest(string importTMXFile)
		{
			Logger.Info(string.Format("Начало работы теста ExportTMXTest(). Путь к файлу, импортируемому в проект {0}", importTMXFile));
			
			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			QuitDriverAfterTest = true;

			// Создать ТМ с загрузкой файла ТМХ
			CreateTMWithUploadTMX(UniqueTmName, importTMXFile);

			// Отрыть информацию о ТМ и нажать кнопку Экспорт
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Export);

			// Экспортировать - Assert внутри
			MoveTMFile();
		}

		private static readonly string[] tmxFileList = Directory.GetFiles(PathProvider.TMTestFolder);
	}
}
