using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	class TMInProjectTets : TMTest 
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public TMInProjectTets(string browserName)
			: base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
		/// </summary>
		[Test]
		public void CreateTMCheckProjectCreateTMListTest()
		{
			// Создать ТМ
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.English;
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.Russian;
			CreateTMByNameAndSave(UniqueTmName, srcLang, trgLang);

			// Перейти на вкладку Workspace и проверить, что TM есть в списке при создании проекта
			Assert.IsTrue(GetIsExistTMCreateProjectList(UniqueTmName), "Ошибка: ТМ нет в списке при создании проекта");
		}

		/// <summary>
		/// Метод тестирования создания ТМ с проверкой списка TM при создании проекта
		/// </summary>
		[Test]
		public void CreateTMAnotherLangCheckProjectCreateTMListTest()
		{
			// Создать ТМ
			CommonHelper.LANGUAGE srcLang = CommonHelper.LANGUAGE.French;
			CommonHelper.LANGUAGE trgLang = CommonHelper.LANGUAGE.German;
			CreateTMByNameAndSave(UniqueTmName, srcLang, trgLang);

			// Перейти на вкладку Workspace и проверить, что TM есть в списке при создании проекта
			Assert.IsTrue(GetIsExistTMCreateProjectList(UniqueTmName, true, srcLang, trgLang), "Ошибка: ТМ нет в списке при создании проекта");
		}
	}
}
