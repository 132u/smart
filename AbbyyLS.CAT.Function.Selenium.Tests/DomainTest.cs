﻿using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки доменов
	/// </summary>
	public class DomainTest : BaseTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		 
		 
		/// <param name="browserName">Название браузера</param>
		public DomainTest(string browserName)
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
			GoToDomains();
		}



		/// <summary>
		/// Метод тестирования создания Проекта
		/// </summary>
		[Test]
		public void CreateDomainTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Проверить, что проект сохранился
			Assert.IsTrue(GetIsDomainExist(domainName), "Ошибка: проект не создался");
		}

		/// <summary>
		/// Метод тестирования создания Проекта с существующим именем
		/// </summary>
		[Test]
		public void CreateDomainExistingNameTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);
			// Создать проект с таким же именем
			CreateDomain(domainName, false);

			// Проверить, что появилась ошибка существующего имени - Assert внутри
			AssertExistingDomainNameError();
		}

		/// <summary>
		/// Метод тестирования создания Проекта с пустым именем
		/// </summary>
		[Test]
		public void CreateDomainEmptyNameTest()
		{
			// Создать проект с пустым именем
			CreateDomain("", false);

			// Проверить, что проект не сохранился, а остался в режиме редактирования
			Assert.IsTrue(DomainPage.GetIsNewDomainEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования создания Проекта с пробельным именем
		/// </summary>
		[Test]
		public void CreateDomainSpaceNameTest()
		{
			// Создать проект с пробельным именем
			CreateDomain("  ", false);

			// Проверить, что проект не сохранился, а остался в режиме редактирования
			Assert.IsTrue(DomainPage.GetIsNewDomainEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании ТМ
		/// </summary>
		[Test]
		public void CreateDomainCheckCreateTMTest()
		{
			// Создать проект
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Проверить, что проект есть в списке при создании ТМ
			Assert.IsTrue(GetIsDomainExistCreateTM(domainName),
				"Ошибка: проекта нет в списке при создании ТМ");
		}

		/// <summary>
		/// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании глоссария
		/// </summary>
		[Test]
		public void CreateDomainCheckCreateGlossaryTest()
		{
			// Создать проект
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Проверить проект в списке при создании глоссария
			Assert.IsTrue(GetIsDomainExistCreateGlossaryTest(domainName),
				"Ошибка: проекта нет в списке");
		}

		/// <summary>
		/// Метод тестирования создания Проекта, проверка, что проект появился в списке при создании термина глоссария
		/// </summary>
		[Test]
		public void CreateDomainCheckCreateGlossaryItemTest()
		{
			// Создать проект
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Проверить, что проект есть в списке при создании термина глоссария
			Assert.IsTrue(GetIsDomainExistCreateGlossaryItemTest(domainName),
				"Ошибка: проекта нет в списке");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта
		/// </summary>
		[Test]
		public void ChangeDomainNameTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Новое имя проекта
			string newDomainName = GetDomainUniqueName();
			// Изменить имя проекта
			SetDomainNewName(domainName, newDomainName);

			// Проверить, что проекта со старым названием нет
			Assert.IsTrue(!GetIsDomainExist(domainName), "Ошибка: старый проект не удалился");
			// Проверить, что есть проект с новым названием
			Assert.IsTrue(GetIsDomainExist(newDomainName), "Ошибка: новый проект не сохранился");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на пустое
		/// </summary>
		[Test]
		public void ChangeDomainEmptyNameTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Изменить имя проекта
			SetDomainNewName(domainName, "", false);

			// Проверить, что проект не сохранился, а остался в режиме редактирования
			Assert.IsTrue(DomainPage.GetIsEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на пробельное
		/// </summary>
		[Test]
		public void ChangeDomainSpaceNameTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Изменить имя проекта
			SetDomainNewName(domainName, "  ", false);

			// Проверить, что проект не сохранился, а остался в режиме редактирования
			Assert.IsTrue(DomainPage.GetIsEditMode(),
				"Ошибка: не остался в режиме редактирования");
		}

		/// <summary>
		/// Метод тестирования изменения имени Проекта на существующее
		/// </summary>
		[Test]
		public void ChangeDomainExistingNameTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);
			// Создать другой проект с уникальным именем
			string secondDomainName = GetDomainUniqueName();
			CreateDomain(secondDomainName);

			// Изменить имя проекта
			SetDomainNewName(secondDomainName, domainName, false);

			// Проверить, появилась ли ошибка существующего имени - Assert внутри
			AssertExistingDomainNameError();
		}

		/// <summary>
		/// Метод тестирования удаления Проекта
		/// </summary>
		[Test]
		public void DeleteDomainTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Удалить проект
			DomainPage.ClickDeleteDomain(domainName);
			// Проверить, что проект удалился
			Assert.IsTrue(!GetIsDomainExist(domainName), "Ошибка: проект не удалился");
		}

		/// <summary>
		/// Метод тестирования удаления Проекта, проверка списка при создании ТМ
		/// </summary>
		[Test]
		public void DeleteDomainCheckCreateTM()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Удалить проект
			DomainPage.ClickDeleteDomain(domainName);
			// Проверить, что проекта нет в списке при создании TM
			Assert.IsTrue(!GetIsDomainExistCreateTM(domainName),
				"Ошибка: проект остался в списке");
		}

		/// <summary>
		/// Метод тестирования удаления Проекта, проверка списка при создании глоссария
		/// </summary>
		[Test]
		public void DeleteDomainCheckCreateGlossaryTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Удалить проект
			DomainPage.ClickDeleteDomain(domainName);
			// Проверить, что проекта нет в списке при создании глоссария
			Assert.IsTrue(!GetIsDomainExistCreateGlossaryTest(domainName),
				"Ошибка: проект остался в списке");
		}

		/// <summary>
		/// Метод тестирования удаления Проекта, проверка списка при создании термина глоссария
		/// </summary>
		[Test]
		public void DeleteDomainCheckCreateGlossaryItemTest()
		{
			// Создать проект с уникальным именем
			string domainName = GetDomainUniqueName();
			CreateDomain(domainName);

			// Удалить проект
			DomainPage.ClickDeleteDomain(domainName);
			// Проверить, что проекта нет в списке при создании термина глоссария
			Assert.IsTrue(!GetIsDomainExistCreateGlossaryItemTest(domainName),
				"Ошибка: проект остался в списке");
		}
		


		/// <summary>
		/// Изменить имя домена
		/// </summary>
		/// <param name="domainName">старое имя</param>
		/// <param name="newDomainName">новое имя</param>
		/// <param name="shouldSaveOk">должен сохраниться успешно</param>
		private void SetDomainNewName(string domainName, string newDomainName, bool shouldSaveOk = true)
		{
			// Нажать на Edit
			DomainPage.ClickEditDomainBtn(domainName);

			// Ввести новое имя проекта
			DomainPage.EnterNewName(domainName, newDomainName);
			// Сохранить
			DomainPage.ClickSaveDomain();
			if (shouldSaveOk)
			{
				Assert.IsTrue(DomainPage.WaitUntilSave(), "Ошибка: не пропала кнопка Save");
			}
			else
			{
				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Получить уникальное имя домена
		/// </summary>
		/// <returns>имя</returns>
		private string GetDomainUniqueName()
		{
			return "TestDomain" + DateTime.UtcNow.Ticks.ToString();
		}

		/// <summary>
		/// Проверка, есть ли ошибка существующего имени домена (Assert)
		/// </summary>
		private void AssertExistingDomainNameError()
		{
			// Проверить, появилась ли ошибка существующего имени
			Assert.IsTrue(DomainPage.GetIsNameErrorExist(),
				"Ошибка: не появилась ошибка существующего имени");
		}

		/// <summary>
		/// Вернуть, есть ли domain в списке при создании ТМ
		/// </summary>
		/// <param name="domainName"></param>
		/// <returns></returns>
		private bool GetIsDomainExistCreateTM(string domainName)
		{
			// Перейти на вкладку ТМ
			SwitchTMTab();

			// Нажать кнопку Создать TM
			TMPage.ClickCreateTM();
			
			// Нажать на открытие списка проектов
			TMPage.ClickOpenDomainListCreateTM();

			// Проверить, есть ли domain в списке
			return TMPage.GetIsDomainExistCreateTM(domainName);
		}

		/// <summary>
		/// Вернуть, есть ли domain в списке доменов при создании глоссария
		/// </summary>
		/// <param name="domainName"></param>
		/// <returns></returns>
		private bool GetIsDomainExistCreateGlossaryTest(string domainName)
		{
			// Перейти на вкладку Глоссарии
			SwitchGlossaryTab();

			// Нажать кнопку Create a glossary
			OpenCreateGlossary();

			// Нажать, чтобы появился список проектов
			GlossaryEditForm.ClickOpenDomainList();
			// Есть ли domain в списке
			return GlossaryEditForm.GetIsDomainInList(domainName);
		}

		/// <summary>
		/// Вернуть, есть ли domain в списке доменов при создании термина глоссария
		/// </summary>
		/// <param name="domainName"></param>
		/// <returns></returns>
		private bool GetIsDomainExistCreateGlossaryItemTest(string domainName)
		{
			// Перейти на вкладку Глоссарии
			SwitchGlossaryTab();
			
			// Создать глоссарий
			CreateGlossaryByName("Test Glossary Check Domain" + DateTime.Now);

			// Открыть Редактирование глоссария
			GlossaryPage.OpenEditGlossaryList();
			// Открыть форму Редактирование структуры
			GlossaryPage.OpenEditStructureForm();
			GlossaryEditStructureForm.WaitPageLoad();
			// Проверить, что открыта нужнная таблица
			Assert.IsTrue(GlossaryEditStructureForm.GetIsConceptTableDisplay(), "Ошибка: в редакторе структуры отображается не та таблица");

			// Нажать на поле Domain
			Assert.IsTrue(GlossaryEditStructureForm.ClickFieldToAdd(GlossaryEditStructureFormHelper.ATTRIBUTE_TYPE.Domain),
				"Ошибка: не удалось выделить поле Domain");
			// Добавить
			GlossaryEditStructureForm.ClickAddToListBtn();

			// Сохранить
			GlossaryEditStructureForm.ClickSaveStructureBtn();
			// Дождаться закрытия формы
			GlossaryEditStructureForm.WaitFormClose();

			// Нажать New item
			GlossaryPage.ClickNewItemBtn();

			// Нажать на поле
			GlossaryPage.NewItemClickDomainField();
			// Вернуть, есть ли domain в списке
			return GlossaryPage.GetIsDomainExistInItemDomainList(domainName);
		}
	}
}