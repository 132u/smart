using System.IO;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMXUploadingTests<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования кнопки Add TMX для пустого ТМ
		/// </summary>
		[Test]
		public void AddTMXOnClearTMButtonTest()
		{
			Logger.Info("Начало работы теста AddTMXOnClearTMButtonTest().");

			TMPage.OpenCreateTMDialog();
			CreateTMByNameAndSave(UniqueTmName);

			// Загрузить ТМХ по кнопке в информации о ТМ
			UploadDocumentToTMbyButton(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Add, PathProvider.SecondTmFile);
			// Получить количество сегментов
			var segmentCount = GetSegmentCount(UniqueTmName);

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
			Logger.Info("Начало работы теста AddTMXExistingTMButtonTest().");

			// Создать ТМ и загрузить ТМХ файл
			CreateTMWithUploadTMX(UniqueTmName, PathProvider.EditorTmxFile);
			// Получить количество сегментов
			var segCountBefore = GetSegmentCount(UniqueTmName);
			// Загрузить TMX файл
			UploadDocumentToTMbyButton(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Add, PathProvider.SecondTmFile);
			// Получить количество сегментов после загрузки TMX
			var segCountAfter = GetSegmentCount(UniqueTmName);

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
		/// <param name="tmxFileImport">путь в файлу, импортируемого в проект</param>
		[Test, TestCaseSource("tmxFileList")]
		public void ImportTMXTest(string tmxFileImport)
		{
			Logger.Info(string.Format("Начало работы теста ImportTMXTest(). Путь к файлу: {0}", tmxFileImport));

			// Создать ТМ с загрузкой ТМХ
			CreateTMWithUploadTMX(UniqueTmName, tmxFileImport);
			// Необходим рефреш страницы, иначе загрузка ТМХ не работает
			RefreshPage();

			// Проверить, сохранился ли ТМ
			Assert.IsTrue(TMPage.GetIsExistTM(UniqueTmName), 
				"Ошибка: ТМ не сохранился (не появился в списке)");

			var segmentCount = GetSegmentCount(UniqueTmName);

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
			Logger.Info(string.Format("Начало работы теста CheckNotificationDuringTMXFileUploading(). Локализация: {0}", locale));

			//Выбираем необходимую для теста локализацию
			WorkspacePage.SelectLocale(locale);
			//закрыть все сообщения(сообщения о неудачных загрузках других файлов).
			TMPage.CloseAllErrorNotifications();
			// Создать ТМ с загрузкой ТМХ и передать флаг с проверкой существования всплывающего окна с информацией
			CreateTMWithUploadTMX(
				UniqueTmName,
				PathProvider.TMTestFile2,
				checkBaloonExisting: true);

			TMPage.ClickCancelButtonOnNotificationBaloon();
			TMPage.AssertionIsInformationBaloonNotExist();
		}

		private static readonly string[] tmxFileList = Directory.GetFiles(PathProvider.TMTestFolder);
	}
}
