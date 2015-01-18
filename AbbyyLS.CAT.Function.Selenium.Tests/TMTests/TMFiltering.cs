using System;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM;
using NUnit.Framework;

namespace AbbyyLS.CAT.Function.Selenium.Tests.TMTests
{
	public class TMFiltering : TMTest
	{
		public TMFiltering(string browserName) 
			: base(browserName)
		{
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			try
			{
				GoToTranslationMemories();

				CreateTMIfNotExist(
					TMForFilteringName_1,
					CommonHelper.LANGUAGE.French,
					CommonHelper.LANGUAGE.German);
				CreateTMIfNotExist(
					TMForFilteringName_2,
					CommonHelper.LANGUAGE.German,
					CommonHelper.LANGUAGE.English);

				checkInitialDataForTmExist(ProjectGroupName_1, ClientName_1);
				checkInitialDataForTmExist(ProjectGroupName_2, ClientName_2);

				checkTmForFilteringProperties(
					TMForFilteringName_1,
					ProjectGroupName_1,
					ClientName_1,
					TopicName_1);
				checkTmForFilteringProperties(
					TMForFilteringName_2,
					ProjectGroupName_2,
					ClientName_2,
					TopicName_2);
			}
			catch (Exception e)
			{
				Logger.ErrorException("Исключение было сгенерировано на этапе подготовки группы тестов. Исключение:" + e.Message, e);
				throw;
			}
		}

		[SetUp]
		public void Setup()
		{
			TMPage.ClearFiltersPanelIfExist();
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра исходного языка: задан один язык
		/// </summary>
		[Test]
		public void TmFiltrationOneSourceLanguage()
		{
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1), 
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра исходного языка: задано два языка
		/// </summary>
		[Test]
		public void TmFiltrationTwoSourceLanguage()
		{
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.German), clearFilters: false);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра языка перевода: задан один язык
		/// </summary>
		[Test]
		public void TmFiltrationOneTargetLanguage()
		{
			CreateNewTmFilter(() => CreateTargetLanguageFilter(CommonHelper.LANGUAGE.German));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}


		/// <summary>
		/// Метод тестирования ТМ фильтра языка перевода: задано два языка
		/// </summary>
		[Test]
		public void TmFiltrationTwoTargetLanguage()
		{
			CreateNewTmFilter(() => CreateTargetLanguageFilter(CommonHelper.LANGUAGE.German));
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.English), clearFilters: false);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по дате создания ТМ
		/// </summary>
		[Test]
		public void TmFiltrationCreationDateNotExist()
		{
			var futureDate = DateTime.Now.AddDays(1);

			CreateNewTmFilter(() => CreateCreationDateFilter(futureDate));

			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по теме топика: задан определенный топик
		/// </summary>
		[Test]
		public void TmFiltrationSpecificTopic()
		{
			CreateNewTmFilter(() => CreateTopicFilter(TopicName_1));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по теме топика: задан общий топик
		/// </summary>
		[Test]
		public void TmFiltrationCommonTopic()
		{
			CreateNewTmFilter(() => CreateTopicFilter(CommonTopicName));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по проектной группе: одна проектная группа
		/// </summary>
		[Test]
		public void TmFiltrationOneProjectGroup()
		{
			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_1));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по проектной группе: две проектные группы
		/// </summary>
		[Test]
		public void TmFiltrationTwoProjectGroup()
		{
			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_1));
			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_2), clearFilters: false);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по клиенту: один клиент
		/// </summary>
		[Test]
		public void TmFiltrationOneClient()
		{
			CreateNewTmFilter(() => CreateClientFilter(ClientName_1));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по клиенту: два клиента
		/// </summary>
		[Test]
		public void TmFiltrationTwoClients()
		{
			CreateNewTmFilter(() => CreateClientFilter(ClientName_1));
			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}
		
		/// <summary>
		/// Метод тестирования ТМ фильтра по автору
		/// </summary>
		[Test]
		public void TmFiltrationAuthor()
		{
			QuitDriverAfterTest = true;

			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			Authorization("TestAccount", true);
			GoToTranslationMemories();

			CreateTMIfNotExist(TMForFilteringName_3);
			GoToTranslationMemories();

			CreateNewTmFilter(() => CreateAutorFilter("Ringo Star"));

			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_3),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_3));
		}

		/// <summary>
		/// Отмена примененного ТМ фильтра
		/// </summary>
		[Test]
		public void CancelTmFiltration()
		{
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			TMPage.ClearFiltersPanelIfExist();

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка работы кнопки Cancel при создании фильтров ТМ
		/// </summary>
		[Test]
		public void CheckCancelTmFiltrationButton()
		{
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French), cancelFilterCreation: true);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка работы последовательно добавленных разнородных фильтров
		/// </summary>
		[Test]
		public void CheckDifferentTmFiltersApplying()
		{
			CreateNewTmFilter(() => CreateTopicFilter(CommonTopicName));

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French), clearFilters: false);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка удаления одного из множества фильтров с панели фиьтров ТМ
		/// </summary>
		[Test]
		public void TmFiltersCheckOneOfManyFilterRemoving()
		{
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));
			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			RemoveFilterfromTmPanel("Client", ClientName_2);

			Assert.IsTrue(
				GetIsExistTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				GetIsExistTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		private void checkInitialDataForTmExist(
			string projectGroupName,
			string clientName)
		{
			checkProjectGroupExist(projectGroupName);
			checkClientExist(clientName);
		}

		private void checkTmForFilteringProperties(
			string tmName,
			string projectGroupName,
			string clientName,
			string topicName)
		{
			GoToTranslationMemories();

			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			if (TMPage.GetTopicFromTmEditionDialog() != topicName)
			{
				TMPage.ClickTopicsListEditTm();
				TMPage.SelectTopicForTm(topicName);
			}

			if (TMPage.GetProjectGroupNameForTm() != projectGroupName)
			{
				TMPage.ClickToProjectsListAtTmEdditForm();
				TMPage.EditTMAddProject(projectGroupName);
			}

			TMPage.ClickOpenClientListEditTm();
			TMPage.EditTmSelectClient(clientName);

			TMPage.ClickEditSaveBtn();
		}

		private void checkProjectGroupExist(string projectGroupName)
		{
			GoToDomains();

			if (!GetIsDomainExist(projectGroupName))
			{
				CreateDomain(projectGroupName);
			}
		}

		private void checkClientExist(string clientName)
		{
			GoToClients();

			if (!ClientPage.GetIsClientExist(clientName))
			{
				ClientPage.ClickCreateClientBtn();
				ClientPage.EnterNewClientName(clientName);
				ClientPage.ClickSaveBtn();
			}
		}

		private const string TMForFilteringName_1 = "TmForFiltering_First";
		private const string TMForFilteringName_2 = "TmForFiltering_Second";
		private const string TMForFilteringName_3 = "TmForFiltering_Third";
		private const string ProjectGroupName_1 = "ProjectGroupForFiltering_1";
		private const string ProjectGroupName_2 = "ProjectGroupForFiltering_2";
		private const string ClientName_1 = "ClientForFiltering_1";
		private const string ClientName_2 = "ClientForFiltering_2";
		private const string TopicName_1 = "Life";
		private const string TopicName_2 = "Science and technology";
		private const string CommonTopicName = "All topics";
	}	
}
