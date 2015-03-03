using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	public class TMUpdatingTest : TMTest
	{
		public TMUpdatingTest(string browserName) 
			: base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования кнопки Update TM в открывающейся информации о ТМ
		/// </summary>
		[Test]
		public void UpdateTMButtonTest()
		{
			// Создать ТМ и загрузить TMX файл
			CreateTMWithUploadTMX(UniqueTmName, PathProvider.EditorTmxFile);

			// Получить количество сегментов
			int segCountBefore = GetSegmentCount(UniqueTmName);

			// Отрыть информацию о ТМ и нажать кнопку Update
			ClickButtonTMInfo(UniqueTmName, TMPageHelper.TM_BTN_TYPE.Update);

			// Загрузить докумет, соглсившись на предложение переписать ТМ на новые
			UploadDocumentTM(PathProvider.SecondTmFile, UniqueTmName, acceptConfirmationMessage: true);
			
			// Получить количество сегментов
			int segCountAfter = GetSegmentCount(UniqueTmName);

			// Если количество не изменилось, возможно, просто не обновилась страница - принудительно обновить
			if (segCountAfter == segCountBefore)
			{
				ReopenTMInfo(UniqueTmName);
				segCountAfter = GetSegmentCount(UniqueTmName);
			}
			// Проверить, что количество сегментов изменилось
			Assert.IsTrue(segCountBefore != segCountAfter, "Ошибка: количество сегментов должно измениться");
		}
	}
}
