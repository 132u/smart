﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class ProjectPageHelper : CommonHelper
    {
        public ProjectPageHelper(IWebDriver driver, WebDriverWait wait):
            base (driver, wait)
        {
        }

        /// <summary>
        /// Дождаться загрузки проекта
        /// </summary>
        /// <returns></returns>
        public bool WaitPageLoad()
        {
            return WaitUntilDisplayElement(By.XPath(PROJECT_TABLE_XPATH));
        }

        /// <summary>
        /// Нажать на кнопку Progress
        /// </summary>
        public void ClickProgressBtn()
        {
            ClickElement(By.XPath(PROGRESS_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться, пока появится диалог Progress
        /// </summary>
        /// <returns>открылся</returns>
        public bool WaitProgressDialogOpen()
        {
            return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_XPATH));
        }

        /// <summary>
        /// Кликнуть по ячейке с пользователем в диалоге
        /// </summary>
        public void ClickUserNameCell()
        {
            ClickElement(By.XPath(PROGRESS_DIALOG_TABLE_USERNAME_XPATH));
        }

        /// <summary>
        /// Дождаться появления списка пользователей
        /// </summary>
        /// <returns>появился</returns>
        public bool WaitAssignUserList()
        {
            return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_USERLIST_XPATH));
        }

        /// <summary>
        /// Кликнуть по пользователю в списке
        /// </summary>
        /// <param name="userName">имя пользователя</param>
        public void ClickAssignUserListUser(string userName)
        {
            string xPath = PROGRESS_DIALOG_USER_ITEM_LIST_XPATH + "[@title='" + userName + "']";
            ClickElement(By.XPath(xPath));
        }

        /// <summary>
        /// Кликнуть по Assign
        /// </summary>
        public void ClickAssignBtn()
        {
            ClickElement(By.XPath(PROGRESS_DIALOG_ASSIGN_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться появления кнопки Cancel
        /// </summary>
        /// <returns></returns>
        public bool WaitCancelAssignBtnDisplay()
        {
            return WaitUntilDisplayElement(By.XPath(PROGRESS_DIALOG_CANCEL_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Отмену назначения
        /// </summary>
        public void ClickCancelAssignBtn()
        {
            ClickElement(By.XPath(PROGRESS_DIALOG_CANCEL_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Close в диалоге
        /// </summary>
        public void CloseAssignDialogClick()
        {
            ClickElement(By.XPath(PROGRESS_DIALOG_CLOSE_BTN_XPATH));
        }

        /// <summary>
        /// Нажать все Accept
        /// </summary>
        public void ClickAllAcceptBtns()
        {
            // Нажать на Accept
            IList<IWebElement> acceptList = GetElementList(By.XPath(ACCEPT_BTN_XPATH));
            foreach (IWebElement el in acceptList)
            {
                el.Click();
            }
        }

        /// <summary>
        /// Вернуть, есть ли такой документ
        /// </summary>
        /// <param name="documentNumber">номер документа</param>
        /// <returns>есть</returns>
        public bool GetIsExistDocument(int documentNumber)
        {
            return GetIsElementExist(By.XPath(DOCUMENT_LIST_XPATH + "//tr[" + documentNumber + "]"));
        }

        /// <summary>
        /// Выделить документ
        /// </summary>
        /// <param name="documentNumber">номер документа</param>
        public bool SelectDocument(int documentNumber)
        {
            bool isDocumentExist = GetIsExistDocument(documentNumber);
            if (isDocumentExist)
            {
                // Нажать галочку у документа
                ClickElement(By.XPath(DOCUMENT_LIST_XPATH
                    + "//tr[" + documentNumber + "]//td[contains(@class,'checkbox')]//input"));
            }
            return isDocumentExist;
        }

        /// <summary>
        /// Открыть документ
        /// </summary>
        /// <param name="documentNumber">номер документа</param>
        public bool OpenDocument(int documentNumber)
        {
            bool isDocumentExist = GetIsExistDocument(documentNumber);
            if (isDocumentExist)
            {
                ClickElement(By.XPath(DOCUMENT_ROW_XPATH
                    + "[" + documentNumber + "]"
                    + DOCUMENT_ROW_EDITOR_LINK_XPATH));
            }
            return isDocumentExist;
        }

        /// <summary>
        /// Кликнуть Import
        /// </summary>
        public void ClickImportBtn()
        {
            ClickElement(By.XPath(IMPORT_DOCUMENT_BTN_XPATH));
            // TODO проверить, в оригинале:
            //ждем когда окно с настройками загрузится
            //WaitUntilDisplayElement(".//span[contains(@class,'js-document-import')]");
            // Кликнуть по Импорт
            //Driver.FindElement(By.XPath(".//span[contains(@class,'js-document-import')]")).Click();
        }

        /// <summary>
        /// Дождаться открытия диалога импорта
        /// </summary>
        /// <returns></returns>
        public bool WaitImportDialogDisplay()
        {
            return WaitUntilDisplayElement(By.XPath(IMPORT_DIALOG_XPATH));
        }

        /// <summary>
        /// Кликнуть Add в диалоге Импорт
        /// </summary>
        public void ClickAddDocumentInImport()
        {
            ClickElement(By.XPath(IMPORT_ADD_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Next в диалоге импорта
        /// </summary>
        public void ClickNextImportDialog()
        {
            ClickElement(By.XPath(IMPORT_NEXT_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться появления таблицы ТМ в диалоге импорта
        /// </summary>
        /// <returns></returns>
        public bool WaitImportTMTableDisplay()
        {
            return WaitUntilDisplayElement(By.XPath(IMPORT_TM_TABLE_XPATH));
        }

        /// <summary>
        /// Кликнуть finish
        /// </summary>
        public void ClickFinishImportDialog()
        {
            ClickElement(By.XPath(IMPORT_FINISH_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть по кнопке Выгрузка логов
        /// </summary>
        public void ClickDownloadLogs()
        {
            ClickElement(By.XPath(DOWNLOAD_LOGS_BTN_XPATH));
        }


        /// <summary>
        /// Дождаться загрузки
        /// </summary>
        /// <returns>загрузился документ</returns>
        public bool WaitDocumentDownloadFinish()
        {
            bool isDisappeared = true;
            if (GetIsElementDisplay(By.XPath(DOWNLOAD_DOC_IMG_XPATH)))
            {
                isDisappeared = false;
                for (int i = 0; i < 5; ++i)
                {
                    isDisappeared = WaitUntilDisappearElement(By.XPath(DOWNLOAD_DOC_IMG_XPATH), 40);
                    if (isDisappeared)
                    {
                        break;
                    }
                }
            }
            return isDisappeared;
        }

        /// <summary>
        /// Кликнуть кнопку Edit TM
        /// </summary>
        public void ClickEditTMBtn()
        {
            ClickElement(By.XPath(EDIT_TM_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться появления диалога изменения ТМ
        /// </summary>
        /// <returns>появился</returns>
        public bool WaitEditTMDialogAppear()
        {
            return WaitUntilDisplayElement(By.XPath(EDIT_TM_DIALOG_XPATH));
        }

        /// <summary>
        /// Дождаться, пока диалога изменения ТМ пропадет
        /// </summary>
        /// <returns>пропал</returns>
        public bool WaitUntilEditTMDialogDisappear()
        {
            return WaitUntilDisappearElement(By.XPath(EDIT_TM_DIALOG_XPATH));
        }

        /// <summary>
        /// Нажать Create в диалоге редактирования TM
        /// </summary>
        public void ClickCreateTMBtn()
        {
            ClickElement(By.XPath(EDIT_TM_CREATE_BTN_XPATH));
        }

        /// <summary>
        /// Нажать Save в диалоге редактирования TM
        /// </summary>
        public void ClickSaveTMBtn()
        {
            ClickElement(By.XPath(EDIT_TM_SAVE_BTN_XPATH));
        }

        /// <summary>
        /// Дождаться появления сообщение об ошибке при импорте документа
        /// </summary>
        /// <returns>появилось</returns>
        public bool WaitImportDocumentErrorMessage()
        {
            // TODO плохой id
            return WaitUntilDisplayElement(By.XPath(IMPORT_DOCUMENT_ERROR_XPATH));
        }

        /// <summary>
        /// Вернуть, есть ли ошибка - не указан файл
        /// </summary>
        /// <returns>есть ошибка</returns>
        public bool GetIsExistNoFileError()
        {
            SetDriverTimeoutMinimum();
            
            bool isExistError = GetIsElementDisplay(By.XPath(NO_FILE_ERROR_XPATH));
            SetDriverTimeoutDefault();

            return isExistError;
        }

        /// <summary>
        /// Кликнуть Delete
        /// </summary>
        public void ClickDeleteBtn()
        {
            ClickElement(By.XPath(DELETE_BTN_XPATH));
        }

        /// <summary>
        /// Подтвердить
        /// </summary>
        public void ConfirmClickYes()
        {
            if (WaitUntilDisplayElement(By.XPath(CONFIRM_DIALOG_XPATH)))
            {
                ClickElement(By.XPath(CONFIRM_YES_XPATH));
                WaitUntilDisappearElement(By.XPath(CONFIRM_DIALOG_XPATH));
            }
        }

        /// <summary>
        /// Вернуть: текущий статус not Assign?
        /// </summary>
        /// <returns>да</returns>
        public bool GetIsAssignStatusNotAssigned()
        {
            return GetElementClass(By.XPath(PROGRESS_DIALOG_ASSIGN_SPAN_XPATH)).Contains("notAssigned");
        }

        protected const string PROJECT_TABLE_XPATH = "//table[contains(@class,'l-project-panel-tbl')]";
        protected const string PROGRESS_BTN_XPATH = "//span[contains(@class,'js-document-progress')]";
        protected const string PROGRESS_DIALOG_XPATH = "//div[contains(@class,'js-popup-progress')][2]";
        protected const string PROGRESS_DIALOG_TABLE_USERNAME_XPATH =
            ".//table[contains(@class,'js-progress-table')]//tr[1]//td[3]//span";
        protected const string PROGRESS_DIALOG_USERLIST_XPATH = "//span[contains(@class,'js-dropdown__list')]";
        protected const string PROGRESS_DIALOG_USER_ITEM_LIST_XPATH = "//span[contains(@class,'js-dropdown__item')]";
        protected const string PROGRESS_DIALOG_ASSIGN_SPAN_XPATH = "//span[contains(@class,'js-assign')]";
        protected const string PROGRESS_DIALOG_ASSIGN_BTN_XPATH = PROGRESS_DIALOG_ASSIGN_SPAN_XPATH + "//a";
        protected const string PROGRESS_DIALOG_CANCEL_BTN_XPATH = "//span[contains(@class,'js-assigned-cancel')]";
        protected const string PROGRESS_DIALOG_CLOSE_BTN_XPATH = "//div[contains(@class,'js-popup-progress')][2]//span[contains(@class,'js-popup-close')]";

        protected const string ACCEPT_BTN_XPATH = ".//span[contains(@class,'js-accept')]";

        protected const string DOCUMENT_LIST_XPATH = ".//table[contains(@class,'js-documents-table')]//tbody";
        protected const string DOCUMENT_ROW_XPATH = ".//tr[contains(@class,'js-document-row')]";
        protected const string DOCUMENT_ROW_EDITOR_LINK_XPATH = "//a[contains(@class,'js-editor-link')]";

        protected const string IMPORT_DOCUMENT_BTN_XPATH = ".//span[contains(@class,'js-document-import')]";
        protected const string IMPORT_DIALOG_XPATH = ".//div[contains(@class,'js-popup-import-document')][2]";
        protected const string IMPORT_ADD_BTN_XPATH = IMPORT_DIALOG_XPATH + "//a[contains(@class,'js-add-file')]";
        protected const string IMPORT_NEXT_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-next')]";
        protected const string IMPORT_TM_TABLE_XPATH = IMPORT_DIALOG_XPATH + "//table[contains(@class,'js-tms-table')]";
        protected const string IMPORT_FINISH_BTN_XPATH = IMPORT_DIALOG_XPATH + "//span[contains(@class,'js-finish js-upload-btn')]"; // TODO проверить, в оригинале:  ".//div[contains(@class,'js-step last active')]
        protected const string DOWNLOAD_DOC_IMG_XPATH = "//img[contains(@title,'Processing job')]";
        protected const string NO_FILE_ERROR_XPATH = IMPORT_DIALOG_XPATH + "//p[contains(@class,'js-error-files-required')]";

        protected const string DOWNLOAD_LOGS_BTN_XPATH = "//span[contains(@class,'js-document-export-logs')]";

        protected const string EDIT_TM_BTN_XPATH = "//div[contains(@class,'l-project-panel')]//span[contains(@class,'js-tm-edit')]";
        protected const string EDIT_TM_DIALOG_XPATH = "//div[contains(@class,'js-popup-tm')][2]";
        protected const string EDIT_TM_CREATE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-tm-create')]";
        protected const string EDIT_TM_SAVE_BTN_XPATH = EDIT_TM_DIALOG_XPATH + "//span[contains(@class,'js-submit-btn')]";

        protected const string IMPORT_DOCUMENT_ERROR_XPATH = "//div[contains(@class,'js-info-popup')]";
        protected const string DELETE_BTN_XPATH = "//span[contains(@class,'js-document-delete')]";

        protected const string CONFIRM_DIALOG_XPATH = "//div[contains(@class,'js-popup-confirm')]";
        protected const string CONFIRM_YES_XPATH = CONFIRM_DIALOG_XPATH + "//input[contains(@class,'js-submit-btn')]";
    }
}
