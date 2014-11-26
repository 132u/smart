using System;
using System.IO;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMExportTests : TMTest
	{
		public TMExportTests(string browserName) 
			: base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования кнопки Export в открывающейся информации о пустой ТМ
		/// </summary>
		[Test]
		public void ExportClearTMButtonTest()
		{
			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			QuitDriverAfterTest = true;

			// Создать ТМ
			CreateTMByNameAndSave(UniqueTmName);

			// Отрыть информацию о ТМ и нажать кнопку
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Export);
			// Экспортировать - Assert внутри
			ExportTM();
		}

		/// <summary>
		/// Метод тестирования кнопки Export в открывающейся информации о ТМ с загруженным TMX (по списку ТМХ файлов для загрузки)
		/// </summary>
		/// <param name="importTMXFile">путь в файлу, импортируемого в проект</param>
		[Test, TestCaseSource("tmxFileList")]
		public void ExportTMXTest(string importTMXFile)
		{
			// После теста с экспортом необходимо выйти из брайзера,
			// чтобы сбросить выбор в диалоге экспорта (сохранить или открыть файл)
			QuitDriverAfterTest = true;

			// Создать ТМ с загрузкой файла ТМХ
			CreateTMWithUploadTMX(UniqueTmName, importTMXFile);

			// Отрыть информацию о ТМ и нажать кнопку
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Export);
			// Экспортировать - Assert внутри
			ExportTM();
		}

		private static readonly string[] tmxFileList = Directory.GetFiles(
														Path.Combine(
															Environment.CurrentDirectory,
															@"..\TestingFiles\",
															"TMTestFiles"));
	}
}
