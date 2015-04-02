using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	[Category("Standalone")]
	class TMInProjectTets<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new() 
	{
		/// <summary>
		/// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
		/// </summary>
		[Test]
		public void CreateTMCheckProjectCreateTMListTest()
		{
			Logger.Info("Начало работы теста CreateTMCheckProjectCreateTMListTest().");

			// Создать ТМ
			var srcLang = CommonHelper.LANGUAGE.English;
			var trgLang = CommonHelper.LANGUAGE.Russian;

			CreateTMByNameAndSave(UniqueTmName, srcLang, trgLang);

			Assert.IsTrue(GetIsExistTmInListDuringProjectCreation(UniqueTmName), "Ошибка: ТМ нет в списке при создании проекта");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
		/// </summary>
		[Test]
		public void CreateTMAnotherLangCheckProjectCreateTMListTest()
		{
			Logger.Info("Начало работы теста CreateTMAnotherLangCheckProjectCreateTMListTest().");

			// Создать ТМ
			var srcLang = CommonHelper.LANGUAGE.French;
			var trgLang = CommonHelper.LANGUAGE.German;
			
			CreateTMByNameAndSave(UniqueTmName, srcLang, trgLang);
			
			Assert.IsTrue(GetIsExistTmInListDuringProjectCreation(UniqueTmName, true, srcLang, trgLang), "Ошибка: ТМ нет в списке при создании проекта");
		}
	}
}
