namespace AbbyyLS.CAT.Function.Selenium.Tests.CommonDataStructures
{
	public class UserInfo
	{
		public UserInfo(string login, string passeord, bool isActivated)
		{
			Login = login;
			Password = passeord;
			Activated = isActivated;
		}

		public string Login { get; private set; }
		public string Password { get; private set; }
		public bool Activated { get; private set; }
	}
}
