using System;
using NUnit.Framework;
using AbbyyLS.CAT.Function.Selenium.Tests.Driver;
using AbbyyLS.CAT.Function.Selenium.Tests.Workspace.TM;

namespace AbbyyLS.CAT.Function.Selenium.Tests.TMTests
{
	[Category("Standalone")]
	public class TMFiltering<TWebDriverSettings> : TMTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			Logger.Info("Начало работы метода TestFixtureSetUp. Подготовка перед каждым тест-сетом TMFiltering.");

			try
			{
				GoToUrl(RelativeUrlProvider.TranslationMemories);

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
				ExitDriver();
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
			Logger.Info("Начало работы теста TmFiltrationOneSourceLanguage()");

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра исходного языка: задано два языка
		/// </summary>
		[Test]
		public void TmFiltrationTwoSourceLanguage()
		{
			Logger.Info("Начало работы теста TmFiltrationTwoSourceLanguage()");

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));
			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.German), clearFilters: false);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра языка перевода: задан один язык
		/// </summary>
		[Test]
		public void TmFiltrationOneTargetLanguage()
		{
			Logger.Info("Начало работы теста TmFiltrationOneTargetLanguage()");

			CreateNewTmFilter(() => CreateTargetLanguageFilter(CommonHelper.LANGUAGE.German));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}


		/// <summary>
		/// Метод тестирования ТМ фильтра языка перевода: задано два языка
		/// </summary>
		[Test]
		public void TmFiltrationTwoTargetLanguage()
		{
			Logger.Info("Начало работы теста TmFiltrationTwoTargetLanguage()");

			CreateNewTmFilter(() => CreateTargetLanguageFilter(CommonHelper.LANGUAGE.German));
			CreateNewTmFilter(() => CreateTargetLanguageFilter(CommonHelper.LANGUAGE.English), clearFilters: false);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по дате создания ТМ
		/// </summary>
		[Category("PRX_8974")]
		[Test]
		public void TmFiltrationCreationDateNotExist()
		{
			Logger.Info("Начало работы теста TmFiltrationCreationDateNotExist()");

			var futureDate = DateTime.Now.AddDays(1);

			CreateNewTmFilter(() => CreateCreationDateFilter(futureDate));

			Assert.IsFalse(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по теме топика: задан определенный топик
		/// </summary>
		[Test]
		public void TmFiltrationSpecificTopic()
		{
			Logger.Info("Начало работы теста TmFiltrationSpecificTopic()");

			CreateNewTmFilter(() => CreateTopicFilter(TopicName_1));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по теме топика: задан общий топик
		/// </summary>
		[Test]
		public void TmFiltrationCommonTopic()
		{
			Logger.Info("Начало работы теста TmFiltrationCommonTopic()");

			CreateNewTmFilter(() => CreateTopicFilter(CommonTopicName));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по проектной группе: одна проектная группа
		/// </summary>
		[Test]
		public void TmFiltrationOneProjectGroup()
		{
			Logger.Info("Начало работы теста TmFiltrationOneProjectGroup()");

			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_1));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по проектной группе: две проектные группы
		/// </summary>
		[Category("PRX_8976")]
		[Test]
		public void TmFiltrationTwoProjectGroup()
		{
			Logger.Info("Начало работы теста TmFiltrationTwoProjectGroup()");

			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_1));
			CreateNewTmFilter(() => CreateProjectGroupFilter(ProjectGroupName_2), clearFilters: false);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по клиенту: один клиент
		/// </summary>
		[Test]
		public void TmFiltrationOneClient()
		{
			Logger.Info("Начало работы теста TmFiltrationOneClient()");

			CreateNewTmFilter(() => CreateClientFilter(ClientName_1));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Метод тестирования ТМ фильтра по клиенту: два клиента
		/// </summary>
		[Category("PRX_8974")]
		[Test]
		public void TmFiltrationTwoClients()
		{
			Logger.Info("Начало работы теста TmFiltrationTwoClients()");

			CreateNewTmFilter(() => CreateClientFilter(ClientName_1));
			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}
		
		/// <summary>
		/// Метод тестирования ТМ фильтра по автору
		/// </summary>
		[Test]
		public void TmFiltrationAuthor()
		{
			Logger.Info("Начало работы теста TmFiltrationAuthor()");

			QuitDriverAfterTest = true;

			WorkspacePage.ClickAccount();
			WorkspacePage.ClickLogoff();

			Authorization(Login2, Password2);
			GoToUrl(RelativeUrlProvider.TranslationMemories);

			CreateTMIfNotExist(TMForFilteringName_3);
			GoToUrl(RelativeUrlProvider.TranslationMemories);

			CreateNewTmFilter(() => CreateAutorFilter("Ringo Star"));

			Assert.IsFalse(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
			Assert.IsTrue(
				FindTM(TMForFilteringName_3),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_3));
		}

		/// <summary>
		/// Отмена примененного ТМ фильтра
		/// </summary>
		[Test]
		public void CancelTmFiltration()
		{
			Logger.Info("Начало работы теста CancelTmFiltration()");

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			TMPage.ClearFiltersPanelIfExist();

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка работы кнопки Cancel при создании фильтров ТМ
		/// </summary>
		[Test]
		public void CheckCancelTmFiltrationButton()
		{
			Logger.Info("Начало работы теста CheckCancelTmFiltrationButton()");

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French), cancelFilterCreation: true);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка работы последовательно добавленных разнородных фильтров
		/// </summary>
		[Test]
		public void CheckDifferentTmFiltersApplying()
		{
			Logger.Info("Начало работы теста CheckDifferentTmFiltersApplying()");

			CreateNewTmFilter(() => CreateTopicFilter(CommonTopicName));

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsTrue(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_2));

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French), clearFilters: false);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsFalse(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));
		}

		/// <summary>
		/// Проверка удаления одного из множества фильтров с панели фиьтров ТМ
		/// </summary>
		[Test]
		public void TmFiltersCheckOneOfManyFilterRemoving()
		{
			Logger.Info("Начало работы теста TmFiltersCheckOneOfManyFilterRemoving()");

			CreateNewTmFilter(() => CreateSourceLanguageFilter(CommonHelper.LANGUAGE.French));
			CreateNewTmFilter(() => CreateClientFilter(ClientName_2), clearFilters: false);

			Assert.IsFalse(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
				string.Format("Ошибка фильтрации: ТМ {0} найдена в списке ТМ.", TMForFilteringName_2));

			RemoveFilterfromTmPanel("Client", ClientName_2);

			Assert.IsTrue(
				FindTM(TMForFilteringName_1),
				string.Format("Ошибка фильтрации: ТМ {0} не найдена в списке ТМ.", TMForFilteringName_1));
			Assert.IsFalse(
				FindTM(TMForFilteringName_2),
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
			GoToUrl(RelativeUrlProvider.TranslationMemories);

			ClickButtonTMInfo(tmName, TMPageHelper.TM_BTN_TYPE.Edit);

			if (TMPage.GetTopicFromTmEditionDialog() != topicName)
			{
				TMPage.ClickTopicsListEditTm();
				TMPage.SelectTopicForTm(topicName);
				TMPage.ClickTopicsListEditTm();
			}

			if (TMPage.GetProjectGroupNameForTm() != projectGroupName)
			{
				TMPage.ClickToProjectsListAtTmEdditForm();
				TMPage.EditTMAddProject(projectGroupName);
				TMPage.ClickToProjectsListAtTmEdditForm();
			}

			TMPage.ClickOpenClientListEditTm();
			TMPage.EditTmSelectClient(clientName);

			TMPage.ClickEditSaveBtn();
			ClearSearch();
		}

		private void checkProjectGroupExist(string projectGroupName)
		{
			GoToUrl(RelativeUrlProvider.Domains);

			if (!DomainPage.GetIsDomainExist(projectGroupName))
			{
				CreateDomain(projectGroupName);
			}
		}

		private void checkClientExist(string clientName)
		{
			GoToUrl(RelativeUrlProvider.Clients);

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
