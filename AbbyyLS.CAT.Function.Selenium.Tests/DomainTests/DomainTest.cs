﻿using System;
using System.Threading;

﻿using NUnit.Framework;
 
 ﻿using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests.Workspace.Domains
{
	/// <summary>
	/// Группа тестов для проверки доменов
	/// </summary>
	[Category("Standalone")]
	public class DomainTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			Logger.Info("Начало работы метода SetUp. Подготовка перед каждым тестом.");

			Logger.Debug("Значение параметра QuitDriverAfterTest = false. Не закрывать браузер после каждого теста.");
			QuitDriverAfterTest = false;

			GoToUrl(RelativeUrlProvider.Domains);
		}

		/// <summary>
		/// Метод тестирования создания Проекта
		/// </summary>
		[Test]
		public void CreateDomainTest()
		{
			Logger.Info("Начало работы теста CreateDomainTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			Assert.IsTrue(DomainPage.GetIsDomainExist(domainName), "Ошибка: проект не создался");
		}

		/// <summary>
		/// Метод тестирования создания Проекта с существующим именем
		/// </summary>
		[Test]
		public void CreateDomainExistingNameTest()
		{
			Logger.Info("Начало работы теста CreateDomainExistingNameTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			CreateDomain(domainName, false);

			DomainPage.AssertionIsNameErrorExist();
		}

		/// <summary>
		/// Метод тестирования создания Проекта с пустым именем
		/// </summary>
		[Test]
		public void CreateDomainEmptyNameTest()
		{
			Logger.Info("Начало работы теста CreateDomainEmptyNameTest().");

			CreateDomain("", false);

			DomainPage.AssertionIsNewDomainEditMode();
		}

		/// <summary>
		/// Метод тестирования создания Проекта с пробельным именем
		/// </summary>
		[Test]
		public void CreateDomainSpaceNameTest()
		{
			Logger.Info("Начало работы теста CreateDomainSpaceNameTest().");

			CreateDomain("  ", false);

			DomainPage.AssertionIsNewDomainEditMode();
		}

		/// <summary>
		/// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании ТМ
		/// </summary>
		[Test]
		public void CreateDomainCheckCreateTMTest()
		{
			Logger.Info("Начало работы теста CreateDomainCheckCreateTMTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			Assert.IsTrue(getIsDomainExistCreateTM(domainName), "Ошибка: проекта нет в списке при создании ТМ");
		}

		/// <summary>
		/// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании глоссария
		/// </summary>
		[Test]
		public void CreateDomainCheckCreateGlossaryTest()
		{
			Logger.Info("Начало работы теста CreateDomainCheckCreateGlossaryTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			Assert.IsTrue(getIsDomainExistCreateGlossaryTest(domainName), "Ошибка: проекта нет в списке при создании глоссария");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта
		/// </summary>
		[Test]
		public void ChangeDomainNameTest()
		{
			Logger.Info("Начало работы теста ChangeDomainNameTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			var newDomainName = getDomainUniqueName();
			setDomainNewName(domainName, newDomainName);

			Assert.IsFalse(DomainPage.GetIsDomainExist(domainName), "Ошибка: старый проект не удалился");
			Assert.IsTrue(DomainPage.GetIsDomainExist(newDomainName), "Ошибка: новый проект не сохранился");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на пустое
		/// </summary>
		[Test]
		public void ChangeDomainEmptyNameTest()
		{
			Logger.Info("Начало работы теста ChangeDomainEmptyNameTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			setDomainNewName(domainName, "", false);

			DomainPage.AssertionIsEditMode();
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на пробельное
		/// </summary>
		[Test]
		public void ChangeDomainSpaceNameTest()
		{
			Logger.Info("Начало работы теста ChangeDomainSpaceNameTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);

			setDomainNewName(domainName, "  ", false);

			DomainPage.AssertionIsEditMode();
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на существующее
		/// </summary>
		[Test]
		public void ChangeDomainExistingNameTest()
		{
			Logger.Info("Начало работы теста ChangeDomainExistingNameTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);
			
			var secondDomainName = getDomainUniqueName();
			CreateDomain(secondDomainName);

			setDomainNewName(secondDomainName, domainName, false);

			DomainPage.AssertionIsNameErrorExist();
		}

		/// <summary>
		/// Метод тестирования удаления Проекта
		/// </summary>
		[Test]
		public void DeleteDomainTest()
		{
			Logger.Info("Начало работы теста DeleteDomainTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);
			DomainPage.ClickDeleteDomain(domainName);
			
			Assert.IsTrue(!DomainPage.GetIsDomainExist(domainName), "Ошибка: проект не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления Проекта, проверка списка при создании ТМ
		/// </summary>
		[Test]
		public void DeleteDomainCheckCreateTM()
		{
			Logger.Info("Начало работы теста DeleteDomainCheckCreateTM().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);
			DomainPage.ClickDeleteDomain(domainName);

			Assert.IsTrue(!getIsDomainExistCreateTM(domainName), "Ошибка: проекта нет в списке при создании TM");
		}

		/// <summary>
		/// Метод тестирования удаления Проекта, проверка списка при создании глоссария
		/// </summary>
		[Test]
		public void DeleteDomainCheckCreateGlossaryTest()
		{
			Logger.Info("Начало работы теста DeleteDomainCheckCreateGlossaryTest().");

			var domainName = getDomainUniqueName();
			CreateDomain(domainName);
			DomainPage.ClickDeleteDomain(domainName);

			Assert.IsTrue(!getIsDomainExistCreateGlossaryTest(domainName), "Ошибка: проекта нет в списке при создании глоссария");
		}

		/// <summary>
		/// Изменить имя домена
		/// </summary>
		/// <param name="domainName">старое имя</param>
		/// <param name="newDomainName">новое имя</param>
		/// <param name="shouldSaveOk">должен сохраниться успешно</param>
		private void setDomainNewName(
			string domainName, 
			string newDomainName, 
			bool shouldSaveOk = true)
		{
			Logger.Debug(string.Format("Изменение имени домена с {0} на {1}", domainName, newDomainName));

			DomainPage.ClickEditDomainBtn(domainName);
			DomainPage.EnterNewName(domainName, newDomainName);
			DomainPage.ClickSaveDomain();
			
			if (shouldSaveOk)
			{
				DomainPage.WaitUntilSave();
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		private static string getDomainUniqueName()
		{
			Logger.Trace("Генерация новго имени домена");
			return "TestDomain" + DateTime.UtcNow.Ticks;
		}

		private bool getIsDomainExistCreateTM(string domainName)
		{
			Logger.Debug(string.Format("Проверка наличия домена {0} в списке при создании ТМ...", domainName));
			
			SwitchTMTab();
			
			TMPage.ClickCreateTM();
			TMPage.ClickOpenDomainListCreateTM();

			return TMPage.GetIsDomainExistCreateTM(domainName);
		}

		private bool getIsDomainExistCreateGlossaryTest(string domainName)
		{
			Logger.Debug(string.Format("Проверка наличия домена {0} в списке при создании глоссария...", domainName));

			SwitchGlossaryTab();
			OpenCreateGlossary();

			GlossaryEditForm.ClickOpenDomainList();

			return GlossaryEditForm.GetIsDomainInList(domainName);
		}
	}
}