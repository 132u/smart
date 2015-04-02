namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public interface IAbstractPage<out T> where T : class
	{
		/// <summary>
		/// Инициализация страницы
		/// </summary>
		T GetPage();

		/// <summary>
		/// Ожидаем загрузки страницы
		/// </summary>
		void LoadPage();
	}
}
