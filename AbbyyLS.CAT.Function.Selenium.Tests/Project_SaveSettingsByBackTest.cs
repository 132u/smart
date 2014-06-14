using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class Project_SaveSettingsByBackTest : NewProjectTest
    {
        public Project_SaveSettingsByBackTest(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
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
            // Проверили, что вернулись на первый шаг
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsFirstStep(),
                "Ошибка: по кнопке Back не вернулись на первый шаг");

            // Получить прописанное имя проекта
            string resultProjectName = WorkspaceCreateProjectDialog.GetProjectInputName();
            // Target язык
            string resultTargetLanguage = WorkspaceCreateProjectDialog.GetTargetValue();
            // Deadline дата
            string resultDeadline = WorkspaceCreateProjectDialog.GetDeadlineValue();

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на первый шаг не сохранились настройки:\n";

            if (resultProjectName != ProjectName)
            {
                isError = true;
                errorMessage += "- имя проекта не сохранилось\n";
            }

            if (resultTargetLanguage != "Russian")
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
            Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
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
            Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
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
            Workspace_CreateProjectDialogHelper.MT_TYPE mtType = Workspace_CreateProjectDialogHelper.MT_TYPE.DefaultMT;
            WorkspaceCreateProjectDialog.ChooseMT(mtType);
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();
            // Выбрать Stage
            string stageText = "Editing";
            WorkspaceCreateProjectDialog.OpenStageList();
            Assert.IsTrue(WorkspaceCreateProjectDialog.SelectStage(stageText),
                "Ошибка: нет такого stage: " + stageText);

            // Нажать Back
            WorkspaceCreateProjectDialog.ClickBackBtn();
            WorkspaceCreateProjectDialog.GetIsStepChooseMT();
            // Next
            WorkspaceCreateProjectDialog.ClickNextStep();

            // Проверили, что вернулись на шаг выбора stage
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseStage(),
                "Ошибка: не вернулись на предыдущий шаг (выбор Stage)");

            // Значение Stage
            string resultStage = WorkspaceCreateProjectDialog.GetCurrentStage();

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором Stage не сохранились настройки:\n";

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
            bool isFirstMTCheck = WorkspaceCreateProjectDialog.GetIsMTChecked(mtType);

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором MT не сохранились настройки:\n";

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
            bool isFirstGlossaryCheck = WorkspaceCreateProjectDialog.GetIsFirstGlossaryChecked();

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором глоссария не сохранились настройки:\n";

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
            Assert.IsTrue(WorkspaceCreateProjectDialog.GetIsStepChooseTM(),
                "Ошибка: не вернулись на шаг выбора ТМ");

            // Значение checkbox первого ТМ
            bool isFirstTMCheck = WorkspaceCreateProjectDialog.GetIsTMChecked(1);
            // Значение radio первого ТМ
            bool isFirstTMRadio = WorkspaceCreateProjectDialog.GetIsTMRadioChecked(1);

            bool isError = false;
            string errorMessage = "Ошибка: при возврате на шаг с выбором ТМ не сохранились настройки:\n";

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
