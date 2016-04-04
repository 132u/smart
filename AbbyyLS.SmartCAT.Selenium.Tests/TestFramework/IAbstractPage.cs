namespace AbbyyLS.SmartCAT.Selenium.Tests.TestFramework
{
	public interface IAbstractPage<out T> where T : class
	{
		/// <summary>
		/// Ожидаем загрузки страницы
		/// </summary>
		T LoadPage();
	}
}
