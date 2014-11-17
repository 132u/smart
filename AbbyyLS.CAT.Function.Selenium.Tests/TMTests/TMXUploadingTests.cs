using System;
using System.IO;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMXUploadingTests : TMTest
	{
		public TMXUploadingTests(string browserName) 
			: base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования кнопки Add TMX для пустого ТМ
		/// </summary>
		[Test]
		public void AddTMXOnClearTMButtonTest()
		{
			CreateTMByNameAndSave(UniqueTmName);
			// Загрузить ТМХ по кнопке в информации о ТМ
			UploadDocumentToTMbyButton(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Add, SecondTmFile);
			// Получить количество сегментов
			int segmentCount = GetSegmentCount(UniqueTmName);
			// Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
			if (segmentCount == 0)
			{
				ReopenTMInfo(UniqueTmName);
				segmentCount = GetSegmentCount(UniqueTmName);
			}

			// Проверить, что количество сегментов больше нуля (ТМХ загрузился)
			Assert.IsTrue(segmentCount > 0, "Ошибка: количество сегментов должно быть больше нуля");
		}

		/// <summary>
		/// Метод тестирования кнопки Add TMX для ТМ с ТМХ
		/// </summary>
		[Test]
		public void AddTMXExistingTMButtonTest()
		{
			// Создать ТМ и загрузить ТМХ файл
			CreateTMWithUploadTMX(UniqueTmName, EditorTMXFile);

			// Получить количество сегментов
			int segCountBefore = GetSegmentCount(UniqueTmName);

			// Загрузить TMX файл
			UploadDocumentToTMbyButton(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Add, SecondTmFile);
			// Получить количество сегментов после загрузки TMX
			int segCountAfter = GetSegmentCount(UniqueTmName);

			// Если количество сегментов не изменилось, возможно, страница не обновилась - принудительно обновить
			if (segCountAfter <= segCountBefore)
			{
				ReopenTMInfo(UniqueTmName);
				segCountAfter = GetSegmentCount(UniqueTmName);
			}

			// Проверить, что количество сегментов увеличилось (при AddTMX количество сегментов должно суммироваться)
			Assert.IsTrue(segCountAfter > segCountBefore, "Ошибка: количество сегментов должно увеличиться");
		}

		/// <summary>
		/// Создание ТМ с загрузкой ТМХ (по списку ТМХ файлов), проверка, что ТМХ загрузился
		/// </summary>
		/// <param name="TMXFileImport">путь в файлу, импортируемого в проект</param>
		[Test, TestCaseSource("tmxFileList")]
		public void ImportTMXTest(string TMXFileImport)
		{
			// Создать ТМ с загрузкой ТМХ
			CreateTMWithUploadTMX(UniqueTmName, TMXFileImport);

			// Проверить, сохранился ли ТМ
			Assert.IsTrue(GetIsExistTM(UniqueTmName), "Ошибка: ТМ не сохранился (не появился в списке)");

			int segmentCount = GetSegmentCount(UniqueTmName);
			// Если количество сегментов = 0, возможно, не обновилась страница - принудительно обновить
			if (segmentCount == 0)
			{
				ReopenTMInfo(UniqueTmName);
				segmentCount = GetSegmentCount(UniqueTmName);
			}

			// Проверить, что количество сегментов больше 0
			Assert.IsTrue(segmentCount > 0, "Ошибка: количество сегментов должно быть больше 0");
		}

		/// <summary>
		/// Проверка повляения уведамлений при загрузке TMX файла
		/// </summary>
		[TestCase(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.Russian)]
		[TestCase(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT.English)]
		public void CheckNotificationDuringTMXFileUploading(WorkSpacePageHelper.LOCALE_LANGUAGE_SELECT locale)
		{
			//Выбираем необходимую для теста локализацию
			WorkspacePage.SelectLocale(locale);

			//Формируем путь до TMX файла для загрузки
			string tmxFileForBaloonChecking = Path.Combine(
				PathTestFiles,
				"TMTestFiles",
				"TMFile2.tmx");

			// Создать ТМ с загрузкой ТМХ и передать флаг с проверкой существования всплывающего окна с информацией
			CreateTMWithUploadTMX(
				UniqueTmName,
				tmxFileForBaloonChecking,
				checkBaloonExisting: true);

			TMPage.ClickCancelButtonOnNotificationBaloon();

			Assert.IsFalse(TMPage.IsInformationBaloonExist(),
				"Ошибка: плашка с информацией о загружаемых ТU не закрыта.");
		}

		private static readonly string[] tmxFileList = Directory.GetFiles(
												Path.Combine(
													Environment.CurrentDirectory,
													@"..\..\TestingFiles\",
													"TMTestFiles"));
	}
}
