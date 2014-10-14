using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AbbyyLS.PEMT.Function.Selenium.Tests
{
	internal class AdminPageService : BaseTest
	{
		protected bool OpenAdmin()
		{
			bool isSuccess = false;
			Driver.Navigate().GoToUrl(AdminUrl);
			if (AdminPage.WaitPageLoad())
			{
				AdminPage.FillLogin(MainUserInfo.login);
				AdminPage.FillPassword(MainUserInfo.password);
				AdminPage.ClickSubmit();
				isSuccess = AdminPage.GetLoginSuccess();
			}
			return isSuccess;
		}

		protected void PreparePemtAccount()
		{
			if (OpenAdmin())
			{
				AdminPage.ClickOpenEntepriseAccounts();
				if (!AdminPage.GetIsAccountExist(PemtAccountName))
				{
					
				}

			}

		}

		protected void CreatePemtAccount()
		{
			AdminPage.ClickAddAccount();
			bool isWindowWithForm = false;
			Driver.SwitchTo().Window(Driver.WindowHandles[1]);
			isWindowWithForm = AdminPage.GetIsAddAccountFormDisplay();
			Assert.IsTrue(isWindowWithForm, "Ошибка: не нашли окно с формой создания аккаунта");
		}
	}
}
