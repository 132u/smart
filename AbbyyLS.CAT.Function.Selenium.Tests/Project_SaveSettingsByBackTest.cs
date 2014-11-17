using NUnit.Framework;
using System.Collections.Generic;

namespace AbbyyLS.CAT.Function.Selenium.Tests
{
	/// <summary>
	/// Группа тестов для проверки сохранения шагов при создании проекта
	/// </summary>
	public class Project_SaveSettingsByBackTest : NewProjectTest
	{
		/// <summary>
		/// Конструктор теста
		/// </summary>
		/// <param name="browserName">Название браузера</param>
		public Project_SaveSettingsByBackTest(string browserName)
			: base(browserName)
		{

		}

		/// <summary>
		/// Тест: создание проекта, возврат на первый шаг
		/// Проверка, что настройки сохранились
		/// - имя проекта
		/// - target язык
		/// - Deadline дата
		/// </summary>
		[Test]
		public void BackFirstStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Подтвердить переход
			SkipNotSelectedTM();

			// Проверили, что вернулись на первый шаг
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsFirstStep(),
				"Ошибка: по кнопке Back не вернулись на первый шаг");

			// Получить прописанное имя проекта
			var resultProjectName = WorkspaceCreateProjectDialog.GetProjectInputName();
			// Target язык
			var resultTargetLanguageList = WorkspaceCreateProjectDialog.GetTargetLanguageList();
			// Deadline дата
			var resultDeadline = WorkspaceCreateProjectDialog.GetDeadlineValue();

			var isError = false;
			var errorMessage = "Ошибка: при возврате на первый шаг не сохранились настройки:\n";

			if (resultProjectName != ProjectName)
			{
				isError = true;
				errorMessage += "- имя проекта не сохранилось\n";
			}

			if (resultTargetLanguageList.Count != 1 || resultTargetLanguageList[0] != "Russian")
			{
				isError = true;
				errorMessage += "- язык Target не сохранился\n";
			}

			if (resultDeadline != DeadlineDate)
			{
				isError = true;
				errorMessage += "- Deadline дата не сохранилась\n";
			}

			// Проверить ошибки
			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Тест: создание проекта, возврат на шаг выбора ТМ
		/// Проверка, что настройки сохранились
		/// - выбранный ТМ
		/// </summary>
		[Test]
		public void BackChooseTMStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Проверить TM
			CheckTMSettings();
		}

		/// <summary>
		/// Тест: создание проекта, выбор ТМ, возврат на предыдущий шаг, обратно к выбору ТМ
		/// Проверка, что настройки сохранились
		/// - выбранный ТМ
		/// </summary>
		[Test]
		public void BackNextChooseTMStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить 
			CheckTMSettings();
		}

		/// <summary>
		/// Тест: создание проекта, возврат на шаг выбора глоссария
		/// Проверка, что настройки сохранились
		/// - выбранный глоссарий
		/// </summary>
		[Test]
		public void BackChooseGlossaryStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать глоссарий
			WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Проверить, сохранился ли выбор глоссария
			CheckGlossarySettings();
		}

		/// <summary>
		/// Тест: создание проекта, выбор глоссария, возврат на предыдущий шаг, обратно к выбору глоссария
		/// Проверка, что настройки сохранились
		/// - выбранный глоссарий
		/// </summary>
		[Test]
		public void BackNextChooseGlossaryStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать глоссарий
			WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Проверить, сохранился ли выбор глоссария
			CheckGlossarySettings();
		}

		/// <summary>
		/// Тест: создание проекта, возврат на шаг выбора MT
		/// Проверка, что настройки сохранились
		/// - выбранный MT
		/// </summary>
		[Test]
		public void BackChooseMTStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать глоссарий
			WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать compreno
			var mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
			WorkspaceCreateProjectDialog.ChooseMT(mtType);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Проверить сохранился ли выбор МТ
			CheckMTSettings(mtType);
		}

		/// <summary>
		/// Тест: создание проекта, выбор МТ, возврат на предыдущий шаг, обратно к МТ
		/// Проверка, что настройки сохранились
		/// - выбранный MT
		/// </summary>
		[Test]
		public void BackNextChooseMTStep()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать глоссарий
			WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать compreno
			var mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
			WorkspaceCreateProjectDialog.ChooseMT(mtType);
			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();

			// Проверить, сохранился ли выбор МТ
			CheckMTSettings(mtType);
		}

		/// <summary>
		/// Тест: создание проекта, выбор stage, возврат к предудыщему, обратно к stage
		/// Проверка, что настройки сохранились
		/// - выбранный Stage
		/// </summary>
		[Test]
		public void BackNextChooseStage()
		{
			// Открыли форму создания проекта, заполнили поля
			FirstStepProjectWizard(ProjectName);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать ТМ
			WorkspaceCreateProjectDialog.ClickFirstTMInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать глоссарий
			WorkspaceCreateProjectDialog.ClickFirstGlossaryInTable();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать compreno
			var mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
			WorkspaceCreateProjectDialog.ChooseMT(mtType);
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();
			// Выбрать Stage
			var stageText = "Editing";
			WorkspaceCreateProjectDialog.OpenStageList();

			Assert.IsTrue(
				WorkspaceCreateProjectDialog.SelectStage(stageText),
				"Ошибка: нет такого stage: " + stageText);

			// Нажать Back
			WorkspaceCreateProjectDialog.ClickBackBtn();
			WorkspaceCreateProjectDialog.GetIsStepChooseMT();
			// Next
			WorkspaceCreateProjectDialog.ClickNextStep();

			// Проверили, что вернулись на шаг выбора stage
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsStepChooseStage(),
				"Ошибка: не вернулись на предыдущий шаг (выбор Stage)");

			// Значение Stage
			var resultStage = WorkspaceCreateProjectDialog.GetCurrentStage();

			var isError = false;
			var errorMessage = "Ошибка: при возврате на шаг с выбором Stage не сохранились настройки:\n";

			if (!resultStage.Contains(stageText))
			{
				isError = true;
				errorMessage += "- stage не сохранился\n";
			}

			// Проверить ошибки
			Assert.IsFalse(isError, errorMessage);
		}



		/// <summary>
		/// Проверка, что выбран Compreno MT
		/// </summary>
		protected void CheckMTSettings(Workspace_CreateProjectDialogHelper.MT_TYPE mtType)
		{
			// Проверили, что вернулись на шаг выбора MT
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseMT(),
				"Ошибка: не вернулись на шаг выбора МТ");

			// Значение checkbox у MT
			var isFirstMTCheck = WorkspaceCreateProjectDialog.GetIsMTChecked(mtType);

			var isError = false;
			var errorMessage = "Ошибка: при возврате на шаг с выбором MT не сохранились настройки:\n";

			if (!isFirstMTCheck)
			{
				isError = true;
				errorMessage += "- checkbox выбора MT не сохранился\n";
			}

			// Проверить ошибки
			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Проверка, что выбран 1й глоссарий
		/// </summary>
		protected void CheckGlossarySettings()
		{
			// Проверили, что вернулись на шаг выбора глоссария
			Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseGlossary(),
				"Ошибка: не вернулись на шаг выбор глоссария");

			// Значение checkbox у глоссария
			var isFirstGlossaryCheck = WorkspaceCreateProjectDialog.GetIsFirstGlossaryChecked();

			var isError = false;
			var errorMessage = "Ошибка: при возврате на шаг с выбором глоссария не сохранились настройки:\n";

			if (!isFirstGlossaryCheck)
			{
				isError = true;
				errorMessage += "- checkbox выбора глоссария не сохранился\n";
			}

			// Проверить ошибки
			Assert.IsFalse(isError, errorMessage);
		}

		/// <summary>
		/// Проверка:
		/// - выбран checkbox первого ТМ
		/// - выбран radio первого ТМ
		/// </summary>
		protected void CheckTMSettings()
		{
			// Проверили, что вернулись на шаг выбора ТМ
			Assert.IsTrue(
				WorkspaceCreateProjectDialog.GetIsStepChooseTM(),
				"Ошибка: не вернулись на шаг выбора ТМ");

			// Значение checkbox первого ТМ
			var isFirstTMCheck = WorkspaceCreateProjectDialog.GetIsTMChecked(1);
			// Значение radio первого ТМ
			var isFirstTMRadio = WorkspaceCreateProjectDialog.GetIsTMRadioChecked(1);

			var isError = false;
			var errorMessage = "Ошибка: при возврате на шаг с выбором ТМ не сохранились настройки:\n";

			if (!isFirstTMCheck)
			{
				isError = true;
				errorMessage += "- checkbox выбора ТМ не сохранился\n";
			}

			if (!isFirstTMRadio)
			{
				isError = true;
				errorMessage += "- radio Write выбора ТМ не сохранился\n";
			}

			// Проверить ошибки
			Assert.IsFalse(isError, errorMessage);
		}
	}
}
