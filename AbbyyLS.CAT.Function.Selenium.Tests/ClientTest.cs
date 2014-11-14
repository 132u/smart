﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки клиентов
	/// </summary>
	public class ClientTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public ClientTest(string browserName)
			: base(browserName)
		{

		}



		/// <summary>
		/// Начальная подготовка для каждого теста
		/// </summary>
		[SetUp]
		public void Setup()
		{
			// Не закрывать браузер
			quitDriverAfterTest = false;
			
			// Переходим к странице воркспейса
			GoToClients();
		}



		/// <summary>
		/// Метод тестирования создания Клиента
		/// </summary>
		[Test]
		public void CreateClientTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Проверить, что клиент сохранился
			Assert.IsTrue(GetIsClientExist(clientName), "Ошибка: клиент не сохранился");
		}

		/// <summary>
		/// Метод тестирования создания Клиента с существующим именем
		/// </summary>
		[Test]
		public void CreateClientExistingNameTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);
			// Создать клиента с таким же именем
			CreateClient(clientName, false);

			// Проверить, появилась ли ошибка существующего имени - Assert внутри
			AssertExistingClientNameError();
		}

		/// <summary>
		/// Метод тестирования создания Клиента с пустым именем
		/// </summary>
		[Test]
		public void CreateClientEmptyNameTest()
		{
			// Создать клиента с пустым именем
			CreateClient("", false);

			// Проверить, что клиент не сохранился, а остался в режиме редактирования
			Assert.IsTrue(ClientPage.GetIsNewClientEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования создания Клиента с пробельным именем
		/// </summary>
		[Test]
		public void CreateClientSpaceNameTest()
		{
			// Создать клиента с пробельным именем
			CreateClient("  ", false);

			// Проверить, что клиент не сохранился, а остался в режиме редактирования
			Assert.IsTrue(ClientPage.GetIsNewClientEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования создания Клиента, проверка, что клиент появился в списке при создании ТМ
		/// </summary>
		[Test]
		public void CreateClientCheckCreateTMTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Проверить, что клиент есть в списке при создании ТМ
			Assert.IsTrue(GetIsClientExistCreateTM(clientName),
				"Ошибка: клиента нет в списке при создании ТМ");
		}

		/// <summary>
		/// Метод тестирования создания Клиента, проверка, что клиент появился в списке при создании глоссария
		/// </summary>
		[Test]
		public void CreateClientCheckCreateGlossaryTest()
		{
			// Создать клиента
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Проверить, что клиент есть в списке при создании глоссария
			Assert.IsTrue(GetIsClientExistCreateGlossaryTest(clientName),
				"Ошибка: клиента нет в списке при создании глоссария");
		}

		/// <summary>
		/// Метод тестирования изменения имени Клиента
		/// </summary>
		[Test]
		public void ChangeClientNameTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Новое имя клиента
			string newClientName = GetClientUniqueName();
			// Изменить имя клиента
			SetClientNewName(clientName, newClientName);

			// Проверить, что клиента со старым названием нет
			Assert.IsTrue(!GetIsClientExist(clientName), "Ошибка: старый клиент не удалился");
			// Проверить, что есть клиент с новым названием
			Assert.IsTrue(GetIsClientExist(newClientName), "Ошибка: новый клиент не сохранился");
		}

		/// <summary>
		/// Метод тестирования изменения имени Клиента на пустое
		/// </summary>
		[Test]
		public void ChangeClientEmptyNameTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Изменить имя клиента
			SetClientNewName(clientName, "", false);
			// Проверить, что клиент не сохранился, а остался в режиме редактирования
			Assert.IsTrue(ClientPage.GetIsEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования изменения имени Клиента на пробельное
		/// </summary>
		[Test]
		public void ChangeClientSpaceNameTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Изменить имя клиента
			SetClientNewName(clientName, "  ", false);
			// Проверить, что клиент не сохранился, а остался в режиме редактирования
			Assert.IsTrue(ClientPage.GetIsEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования изменения имени Клиента на существующее
		/// </summary>
		[Test]
		public void ChangeClientExistingNameTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);
			// Создать другого клиента с уникальным именем
			string secondClientName = GetClientUniqueName();
			CreateClient(secondClientName);
			// Изменить имя клиента
			SetClientNewName(secondClientName, clientName, false);

			// Проверить, появилась ли ошибка существующего имени - Assert внутри
			AssertExistingClientNameError();
		}

		/// <summary>
		/// Метод тестирования удаления Клиента
		/// </summary>
		[Test]
		public void DeleteClientTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Удалить клиента
			ClickDeleteClient(clientName);
			// Проверить, что клиент удалился
			Assert.IsTrue(!GetIsClientExist(clientName), "Ошибка: клиент не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления Клиента, проверка списка при создании ТМ
		/// </summary>
		[Test]
		public void DeleteClientCheckCreateTM()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Удалить клиента
			ClickDeleteClient(clientName);
			// Проверить, что клиента нет в списке при создании TM
			Assert.IsTrue(!GetIsClientExistCreateTM(clientName),
				"Ошибка: клиент остался в списке при создании ТМ");
		}

		/// <summary>
		/// Метод тестирования удаления Клиента, проверка списка при создании глоссария
		/// </summary>
		[Test]
		public void DeleteClientCheckCreateGlossaryTest()
		{
			// Создать клиента с уникальным именем
			string clientName = GetClientUniqueName();
			CreateClient(clientName);

			// Удалить клиента
			ClickDeleteClient(clientName);
			// Проверить, что клиента нет в списке при создании глоссария
			Assert.IsTrue(!GetIsClientExistCreateGlossaryTest(clientName),
				"Ошибка: клиент остался в списке");
		}
		


		/// <summary>
		/// Перейти на страницу клиентов
		/// </summary>
		private void SwitchClientTab()
		{
			// Перейти на страницу Клиенты
			MainHelperClass.ClickOpenClientPage();
			ClientPage.WaitPageLoad();
		}

		/// <summary>
		/// Создать уникальное имя для клиента
		/// </summary>
		/// <returns>имя</returns>
		private string GetClientUniqueName()
		{
			return "TestClient" + DateTime.UtcNow.Ticks.ToString();
		}

		/// <summary>
		/// Создать нового клиента
		/// </summary>
		/// <param name="clientName">имя клиента</param>
		/// <param name="shouldSaveOk">должен успешно сохраниться</param>
		private void CreateClient(string clientName, bool shouldSaveOk = true)
		{
			// Нажать "Новый клиент"
			ClientPage.ClickCreateClientBtn();
			// Ввести имя
			ClientPage.EnterNewClientName(clientName);

			// Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
			Driver.Manage().Window.Maximize();
			// Сохранить клиента
			ClientPage.ClickSaveBtn();
			if (shouldSaveOk)
			{
				Assert.IsTrue(ClientPage.WaitSaveBtnDisappear(), "Ошибка: клиент не сохранился");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Вернуть, есть ли клиент в списке
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>есть</returns>
		private bool GetIsClientExist(string clientName)
		{
			return ClientPage.GetIsClientExist(clientName);
		}

		/// <summary>
		/// Проверить, есть ли ошибка существующего имени
		/// </summary>
		private void AssertExistingClientNameError()
		{
			// Проверить, появилась ли ошибка существующего имени
			Assert.IsTrue(ClientPage.GetIsNameErrorExist(),
				"Ошибка: не появилась ошибка существующего имени");
		}

		/// <summary>
		/// Вернуть, есть ли клиент в списке при создании ТМ
		/// </summary>
		/// <param name="clientName">название</param>
		/// <returns>есть</returns>
		private bool GetIsClientExistCreateTM(string clientName)
		{
			// Перейти на вкладку ТМ
			SwitchTMTab();

			// Нажать кнопку Создать TM
			TMPage.ClickCreateTM();

			// Нажать на открытие списка клиентов
			TMPage.ClickOpenClientListCreateTM();

			// Есть ли клиент в списке
			return TMPage.GetIsClientExistCreateTM(clientName);
		}

		/// <summary>
		/// Проверить, есть ли клиент в списке клиентов при создании глоссария
		/// </summary>
		/// <param name="clientName">название клиента</param>
		/// <returns>есть</returns>
		private bool GetIsClientExistCreateGlossaryTest(string clientName)
		{
			// Перейти на вкладку Глоссарии
			SwitchGlossaryTab();

			// Нажать кнопку Create a glossary
			OpenCreateGlossary();

			// Нажать, чтобы появился список клиентов
			GlossaryEditForm.ClickOpenClientList();
			// Есть ли клиент в списке
			return GlossaryEditForm.GetIsClientInList(clientName);
		}

		/// <summary>
		/// Переименовать
		/// </summary>
		/// <param name="clientName">старое имя</param>
		/// <param name="newClientName">новое имя</param>
		/// <param name="shouldSaveOk">должен сохраниться</param>
		private void SetClientNewName(string clientName, string newClientName, bool shouldSaveOk = true)
		{
			// Нажать Изменить
			ClientPage.ClickEdit(clientName);

			// Ввести новое имя клиента
			ClientPage.EnterNewName(newClientName);

			// Сохранить
			ClientPage.ClickSaveBtn();
			if (shouldSaveOk)
			{
				Assert.IsTrue(ClientPage.WaitSaveBtnDisappear(), "Ошибка: клиент не сохранился");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Кликнуть Удалить
		/// </summary>
		/// <param name="clientName">название</param>
		private void ClickDeleteClient(string clientName)
		{
			ClientPage.ClickDelete(clientName);
		}
	}
}