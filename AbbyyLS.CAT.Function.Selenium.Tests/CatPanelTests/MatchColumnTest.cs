using System;
using System.Threading;
using NLog;

using NUnit.Framework;

using AbbyyLS.CAT.Function.Selenium.Tests.Driver;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки колонки match в таргете при выдачах из cat-панели
	/// </summary>
	[Category("ForLocalRun")]
	class MatchColumnTest<TWebDriverSettings> : BaseTest<TWebDriverSettings> where TWebDriverSettings : IWebDriverSettings, new()
	{
		[SetUp]
		public void SetUp()
		{
			Logger.Info("Начало работы метода SetUp. Подготовка перед каждым тестом.");
			
			Logger.Debug("Значение параметра QuitDriverAfterTest = false. Не закрывать браузер после каждого теста.");
			QuitDriverAfterTest = false;
			if (
				!(TestContext.CurrentContext.Test.Name.Contains("MtSubstitution")
				  || TestContext.CurrentContext.Test.Name.Contains("GlossarySubstitution")))
			{
				if (!_projectTmCreated)
				{
					GoToUrl(RelativeUrlProvider.Workspace);

					CreateProject(
						projectName: _projectNameTmMatchTest,
						downloadFile: PathProvider.TxtFileForMatchTest,
						createNewTM: true,
						tmFile: PathProvider.TmxFileForMatchTest,
						setGlossary: Workspace_CreateProjectDialogHelper.SetGlossary.New,
						glossaryName: "",
						chooseMT: true,
						mtType: Workspace_CreateProjectDialogHelper.MT_TYPE.ABBYY,
						isNeedCheckProjectAppearInList: false);

					OpenAssignDialog(_projectNameTmMatchTest);

					SetResponsible(1, UserName, false);
					ResponsiblesDialog.ClickCloseBtn();

					// Sleep не убирать , проект не открывается
					Thread.Sleep(1000);

					OpenProjectPage(_projectNameTmMatchTest);
					ProjectPage.ClickAllAcceptBtns();
					Thread.Sleep(1000);

					OpenDocument();
					Thread.Sleep(1000);
					_projectTmCreated = true;
				}
			}
			else
			{
				if (!_projectMtCreated)
				{
					GoToUrl(RelativeUrlProvider.Workspace);

					CreateProject(
						projectName: _projectNameMtMatchTest,
						downloadFile: PathProvider.TxtFileForMatchTest,
						createNewTM: false,
						tmFile: "",
						setGlossary: Workspace_CreateProjectDialogHelper.SetGlossary.New,
						glossaryName: "",
						chooseMT: true,
						mtType: Workspace_CreateProjectDialogHelper.MT_TYPE.ABBYY,
						isNeedCheckProjectAppearInList: false);

					OpenAssignDialog(_projectNameMtMatchTest);

					SetResponsible(1, UserName, false);
					ResponsiblesDialog.ClickCloseBtn();

					// Sleep не убирать , проект не открывается
					Thread.Sleep(1000);

					OpenProjectPage(_projectNameMtMatchTest);
					ProjectPage.ClickAllAcceptBtns();
					Thread.Sleep(1000);

					OpenDocument();
					Thread.Sleep(1000);
					_projectMtCreated = true;
				}
			}

		}

		/// <summary>
		/// Для пяти сегментов с разным процентом совпадения - 102, 101, 100, 77-99, 50-75 - подставляет значение из TM
		/// проверка1: в таргете появилось значение в колонке match - ресурс ТМ  
		/// проверка2: проверка, что число совпадает с числом из СAT-панели
		/// проверка3: проверка цветового выделения
		/// </summary>
		/// <param name="segmentNumber">номер сегмента таргет для подстановки из CAT</param>
		[Test]
		public void CheckMatchAfterTmSubstitutionSegmentNumber(
			[Values(1, 2, 3, 4, 5)]
			int segmentNumber)
		{
			Logger.Info(string.Format("Начало работы теста CheckMatchAfterTmSubstitutionSegmentNumber(). Сегмент №{0}", segmentNumber));

			const int yellowUpperBound = 99;
			const int yellowLowerBound = 76;

			const string green = "green";
			const string yellow = "yellow";
			const string red = "red";

			//ищем в кат номер строки с подходящим термином из ТМ, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			var catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(
				segmentNumber, 
				EditorPageHelper.CAT_TYPE.TM);
			
			//проверка1: появилось значение в колонке match - ресурс ТМ  
			Assert.AreEqual(
				_catSubstitutionTmType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - ТМ");

			//проверяем, сколько процентов в колонке match таргета
			var targetMatchPercent = EditorPage.GetTargetMatchPercent(segmentNumber);

			//проверка2: проверка, что число совпадает с числом из СAT-панели
			Assert.AreEqual(
				CatPanel.GetCatTranslationProcentMatch(catSubstitutionLineNumber - 1), 
				targetMatchPercent, 
				"процент совпадения в таргет не совпадает с процентом на панели CAT"); 
			
			//проверка3: проверка цветового выделения
			if (targetMatchPercent > yellowUpperBound)
			{
				Assert.AreEqual(
					green, 
					EditorPage.GetTargetMatchColor(segmentNumber),
					"цвет не соответствует проценту совпадения");
			}


			if (targetMatchPercent <= yellowUpperBound &&
				targetMatchPercent >= yellowLowerBound)
			{
				Assert.AreEqual(
					yellow,
					EditorPage.GetTargetMatchColor(segmentNumber),
					"цвет не соответствует проценту совпадения");
			}

			if (targetMatchPercent < yellowLowerBound)
			{
				Assert.AreEqual(
					red, 
					EditorPage.GetTargetMatchColor(segmentNumber),
					"цвет не соответствует проценту совпадения");
			}
			
		}

		/// <summary>
		/// Для одного сегмента подставляет значение из MT
		/// проверка1: в таргете появилось значение в колонке match - ресурс MT
		/// проверка2: проверка, что число совпадает с числом из СAT-панели
		/// </summary>
		[Test]
		public void CheckMatchAfterMtSubstitution()
		{
			Logger.Info("Начало работы теста CheckMatchAfterMtSubstitution().");

			const int segmentNumber = 1;
			
			// кликаем в таргет, чтобы заполнилась панель CAT 
			EditorPage.ClickTargetCell(segmentNumber);
			
			//ищем в кат номер строки с подходящим термином из MT, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.MT);
			
			//проверка1: появилось значение в колонке match - ресурс MT  
			Assert.AreEqual(
				_catSubstitutionMtType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - МТ");
		}

		/// <summary>
		/// Для одного сегмента подставляет значение из MT
		/// проверка1: в таргете появилось значение в колонке match - ресурс MT
		/// Затем подставляет значение из TM
		/// проверка2: в таргете появилось значение в колонке match - ресурс TM
		/// </summary>
		[Test]
		public void CheckMatchAfterBothSubstitutions()
		{
			Logger.Info("Начало работы теста CheckMatchAfterBothSubstitutions().");

			const int segmentNumber = 1;

			// кликаем в таргет, чтобы заполнилась панель CAT 
			EditorPage.ClickTargetCell(segmentNumber);
			
			//ищем в кат номер строки с подходящим термином из МТ, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			var catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.MT);
			
			//проверка1: появилось значение в колонке match - ресурс MT  
			Assert.AreEqual(
				_catSubstitutionMtType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - МТ");
			
			//удаляем подставленный термин
			EditorPage.AddTextTarget(segmentNumber, "");

			//заполняем снова CATpanel
			EditorPage.ClickTargetCell(segmentNumber);

			//ищем в кат номер строки с подходящим термином из ТМ, подставляем в таргет
			catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.TM);
			
			//проверка1: появилось значение в колонке match - ресурс ТМ  
			Assert.AreEqual(
				_catSubstitutionTmType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - ТМ");

			//проверяем, сколько процентов в колонке match таргета
			var targetMatchPercent = EditorPage.GetTargetMatchPercent(segmentNumber);

			//проверка2: проверка, что число совпадает с числом из СAT-панели
			Assert.AreEqual(
				CatPanel.GetCatTranslationProcentMatch(catSubstitutionLineNumber - 1), 
				targetMatchPercent, 
				"процент совпадения в таргет не совпадает с процентом на панели CAT");
		}

		/// <summary>
		/// Для одного сегмента подставляет значение из TM
		/// В таргете дописали текст
		/// проверка: в таргете осталось значение в колонке match - ресурс TM
		/// </summary>
		[Test]
		[Category("Standalone")]
		public void CheckMatchAfterEditCell()
		{
			Logger.Info("Начало работы теста CheckMatchAfterEditCell().");

			const int segmentNumber = 1;

			// кликаем в таргет, чтобы заполнилась панель CAT 
			EditorPage.ClickTargetCell(segmentNumber);

			//ищем в кат номер строки с подходящим термином из TM, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			var catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.TM);

			//добавляем в таргет текст
			EditorPage.SendKeysTarget(segmentNumber, " hello ");

			//проверка1: значение в колонке match прежнее - ресурс ТМ  
			Assert.AreEqual(
				_catSubstitutionTmType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - ТМ");

			//проверяем, сколько процентов в колонке match таргета
			var targetMatchPercent = EditorPage.GetTargetMatchPercent(segmentNumber);

			//проверка2: проверка, что число совпадает с числом из СAT-панели
			Assert.AreEqual(
				CatPanel.GetCatTranslationProcentMatch(catSubstitutionLineNumber - 1), 
				targetMatchPercent, 
				"процент совпадения в таргет не совпадает с процентом на панели CAT");
		}
		
		/// <summary>
		/// Для одного сегмента подставляет значение из TM
		/// В таргете удалили текст и вставили новый
		/// проверка: в таргете осталось значение в колонке match - ресурс TM
		/// </summary>
		[Test]
		[Category("Standalone")]
		public void CheckMatchAfterDelete()
		{
			Logger.Info("Начало работы теста CheckMatchAfterDelete().");

			const int segmentNumber = 1;

			// кликаем в таргет, чтобы заполнилась панель CAT
			EditorPage.ClickTargetCell(segmentNumber);

			//ищем в кат номер строки с подходящим термином из TM, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			var catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.TM);

			//удаляем текст, вставляем новый
			EditorPage.ClickTargetCell(segmentNumber);
			EditorPage.AddTextTarget(segmentNumber, "new text");

			//проверка1: значение в колонке match прежнее - ресурс ТМ  
			Assert.AreEqual(
				_catSubstitutionTmType, 
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не появилось значение ресурса - ТМ");

			//проверяем, сколько процентов в колонке match таргета
			var targetMatchPercent = EditorPage.GetTargetMatchPercent(segmentNumber);

			//проверка2: проверка, что число совпадает с числом из СAT-панели
			Assert.AreEqual(
				CatPanel.GetCatTranslationProcentMatch(catSubstitutionLineNumber - 1), 
				targetMatchPercent, 
				"процент совпадения в таргет не совпадает с процентом на панели CAT");
		
		}

		/// <summary>
		/// Подстановка в непустой сегмент: для одного сегмента подставляет значение из TM, MT
		/// проверка: в таргете осталось значение в колонке match - ресурс TM/MT
		/// </summary>
		///  <param name="catType">тип подстановки из CAT</param>
		[Test]
		public void CheckTmMtMatchAfterAdd(
			[Values(EditorPageHelper.CAT_TYPE.TM, EditorPageHelper.CAT_TYPE.MT)] 
			EditorPageHelper.CAT_TYPE catType)
		{
			Logger.Info(string.Format("Начало работы теста CheckMatchAfterDelete(). Тип подстановки из CAT {0}", catType));
			
			const int segmentNumber = 1;

			EditorPage.ClickTargetCell(segmentNumber);
			EditorPage.SendKeysTarget(1, " hello ");

			//ищем в кат номер строки с подходящим термином из ТМ или МТ, подставляем в таргет
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			var catSubstitutionLineNumber = PasteFromCatReturnCatLineNumber(segmentNumber, catType);

			//проверка1: значение в колонке match прежнее - ресурс ТМ  
			if (catType == EditorPageHelper.CAT_TYPE.TM)
			{
				Assert.AreEqual(
					_catSubstitutionTmType,
					EditorPage.GetTargetSubstitutionType(segmentNumber),
					"в колонке match таргета не появилось значение ресурса - ТМ");

				//проверяем, сколько процентов в колонке match таргета
				var targetMatchPercent = EditorPage.GetTargetMatchPercent(segmentNumber);

				//проверка2: проверка, что число совпадает с числом из СAT-панели
				Assert.AreEqual(
					CatPanel.GetCatTranslationProcentMatch(catSubstitutionLineNumber - 1),
					targetMatchPercent,
					"процент совпадения в таргет не совпадает с процентом на панели CAT");
			}
			else
			{
				Assert.AreEqual(
					_catSubstitutionMtType, 
					EditorPage.GetTargetSubstitutionType(segmentNumber),
					"в колонке match таргета не появилось значение ресурса - МТ");
			}
		}

		/// <summary>
		/// Для одного сегмента подставляет значение из глоссария
		/// проверка: в таргете исчезло значение в колонке match
		/// </summary>
		[Test]
		[Category("Standalone")]
		public void AfterGlossarySubstitutionCheckMatch()
		{
			Logger.Info("Начало работы теста AfterGlossarySubstitutionCheckMatch().");

			const int segmentNumber = 1;

			EditorPage.ClickTargetCell(segmentNumber);

			var sourceTerm = EditorPage.GetSourceText(segmentNumber);

			
			EditorPage.ClickAddTermBtn();
			AddTermGlossary(sourceTerm, "термин глоссария");
			EditorPage.ClickTargetCell(segmentNumber);

			//ищем в кат номер строки с подходящим термином из глоссария, 
			// подставляем в таргет,
			//записываем номер строки CAT-панели, из которой выполнена подстановка
			PasteFromCatReturnCatLineNumber(segmentNumber, EditorPageHelper.CAT_TYPE.TB);

			Assert.AreEqual(
				_catSubstitutionTbType,
				EditorPage.GetTargetSubstitutionType(segmentNumber),
				"в колонке match таргета не пусто");
		}

		protected string _projectNameTmMatchTest = "TmMatchTest" + "_" + DateTime.UtcNow.Ticks;
		protected string _projectNameMtMatchTest = "MtTbMatchTest" + "_" + DateTime.UtcNow.Ticks;

		protected const string _catSubstitutionTmType = "TM";
		protected const string _catSubstitutionMtType = "MT";
		protected const string _catSubstitutionTbType = "";

		private bool _projectTmCreated;
		private bool _projectMtCreated;
	}
}

