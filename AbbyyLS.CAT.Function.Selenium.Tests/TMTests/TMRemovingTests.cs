using System.Threading;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM
{
	public class TMRemovingTests : TMTest
	{
		public TMRemovingTests(string browserName) : base(browserName)
		{
		}

		/// <summary>
		/// Метод тестирования Delete с проверкой списка TM
		/// </summary>
		[Test]
		public void DeleteTMCheckTMListTest()
		{
			CreateTMIfNotExist(ConstTMName);

			// Отрыть информацию о ТМ, нажать кнопку Delete и сголаситься с предупреждением об удалении ТМ
			ClickButtonTMInfo(
				ConstTMName,
				TMPageHelper.TM_BTN_TYPE.Delete,
				isConfirmationQuestionExist: true);

			// Закрытие формы
			// TODO убрать sleep
			Thread.Sleep(1000);

			// Проверить, что ТМ удалилась из списка
			Assert.IsTrue(!GetIsExistTM(ConstTMName), "Ошибка: ТМ не удалилась из списка");
		}

		/// <summary>
		/// Метод тестирования Delete с проверкой списка TM и создание ТМ с таким же именем
		/// </summary>
		[Test]
		public void DeleteTMAndCreateTMWithTheSameName()
		{
			CreateTMIfNotExist(ConstTMName);

			// Отрыть информацию о ТМ, нажать кнопку Delete и сголаситься с предупреждением об удалении ТМ
			ClickButtonTMInfo(
				ConstTMName,
				TMPageHelper.TM_BTN_TYPE.Delete,
				isConfirmationQuestionExist: true);

			// Закрытие формы
			// TODO: убрать Sleep
			Thread.Sleep(1000);

			// Проверить, что ТМ удалилась из списка
			Assert.IsTrue(!GetIsExistTM(ConstTMName), "Ошибка: ТМ не удалилась из списка.");

			// Создание ТМ с именем, идентичным только удаленной
			CreateTMIfNotExist(ConstTMName);

			// Проверить, что ТМ появилась в списке
			Assert.IsTrue(GetIsExistTM(ConstTMName), "Ошибка: ТМ не появилась в списке.");
		}

		/// <summary>
		/// Метод тестирования Delete с проверкой списка TM при создании проекта
		/// </summary>
		[Test]
		public void DeleteTMCheckProjectCreateTMListTest()
		{
			CreateTMByNameAndSave(UniqueTmName);
			// Отрыть информацию о ТМ и нажать кнопку
			ClickButtonTMInfo(
				UniqueTmName,
				TMPageHelper.TM_BTN_TYPE.Delete,
				isConfirmationQuestionExist: true);

			// Перейти на вкладку Workspace и проверить, что TM нет в списке при создании проекта
			Assert.IsTrue(!GetIsExistTMCreateProjectList(UniqueTmName), "Ошибка: ТМ не удалилась из списка при создании проекта");
		}
	}
}
