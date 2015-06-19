using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMUpdatingTest<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Метод тестирования кнопки Update TM в открывающейся информации о ТМ
		/// </summary>
		[Test]
		public void UpdateTMButtonTest()
		{
			Logger.Info("Начало работы теста UpdateTMButtonTest().");

			CreateTMWithUploadTMX(UniqueTmName, PathProvider.EditorTmxFile);

			// Получить количество сегментов
			var segCountBefore = GetSegmentCount(UniqueTmName);

			// Отрыть информацию о ТМ и нажать кнопку Update
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Update);

			// Загрузить докумет, соглсившись на предложение переписать ТМ на новые
			UploadDocumentTM(PathProvider.SecondTmFile, UniqueTmName, acceptConfirmationMessage: true);
			
			// Получить количество сегментов
			var segCountAfter = GetSegmentCount(UniqueTmName);

			// Если количество не изменилось, возможно, просто не обновилась страница - принудительно обновить
			if (segCountAfter == segCountBefore)
			{
				ReopenTMInfo(UniqueTmName);
				segCountAfter = GetSegmentCount(UniqueTmName);
			}

			Assert.IsTrue(segCountBefore != segCountAfter, "Ошибка: количество сегментов не изменилось.");
		}
	}
}
