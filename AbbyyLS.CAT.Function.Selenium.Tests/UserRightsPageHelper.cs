using System;
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
    public class UserRightsPageHelper : CommonHelper
    {
        public UserRightsPageHelper(IWebDriver driver, WebDriverWait wait) :
            base(driver, wait)
        {
        }

        /// <summary>
        /// Перейти на страницу
        /// </summary>
        public void OpenPage()
        {
            ClickElement(By.XPath(PAGE_LINK_XPATH));
        }

        /// <summary>
        /// Открыть группы
        /// </summary>
        public void OpenGroups()
        {
            // TODO попробовать через link text
            ClickElement(By.XPath(GROUP_LINK_XPATH));
        }

        /// <summary>
        /// Выбрать Администраторы
        /// </summary>
        public void SelectAdmins()
        {
            ClickElement(By.XPath(ADMIN_GROUP_XPATH));
        }

        /// <summary>
        /// Нажать Edit
        /// </summary>
        public void ClickEdit()
        {
            ClickElement(By.XPath(EDIT_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Add Rights
        /// </summary>
        public void ClickAddRights()
        {
            ClickElement(By.XPath(ADD_RIGHTS_BTN_XPATH));
        }

        /// <summary>
        /// Добавить возможность предлагать без указания глоссария
        /// </summary>
        public void SelectSuggestWithoutGlossary()
        {
            ClickElement(By.XPath(SUGGEST_WITHOUT_GLOSSARY_INPUT_XPATH));
        }

        /// <summary>
        /// Добавить возможность поиска по глоссарию
        /// </summary>
        public void SelectGlossarySearch()
        {
            ClickElement(By.XPath(GLOSSARY_SEARCH_INPUT_XPATH));
        }

        /// <summary>
        /// Кликнуть Next
        /// </summary>
        public void ClickNext()
        {
            ClickElement(By.XPath(NEXT_BTN_XPATH));
        }

        /// <summary>
        /// Кликнуть Add
        /// </summary>
        public void ClickAdd()
        {
            ClickElement(By.XPath(ADD_BTN_XPATH));
        }

        /// <summary>
        /// Выбрать Все глоссарии
        /// </summary>
        public void SelectAllGlossaries()
        {
            ClickElement(By.XPath(ALL_GLOSSARIES_SELECT_XPATH));
        }

        /// <summary>
        /// Кликнуть Сохранить
        /// </summary>
        public void ClickSave()
        {
            ClickElement(By.XPath(SAVE_BTN_XPATH));
        }

        protected const string PAGE_LINK_XPATH = "//a[contains(@href,'/Enterprise/Users')]";
        protected const string GROUP_LINK_XPATH = "//a[contains(@href,'/Enterprise/Groups')]";
        protected const string ADMIN_GROUP_XPATH = "//td[contains(@class,'js-group-name')][text()='Administrators']";
        protected const string EDIT_BTN_XPATH = "//span[contains(@class,'js-editgroup-btn')]";
        protected const string ADD_RIGHTS_BTN_XPATH = "//span[contains(@class,'js-add-right-btn')]";
        protected const string SUGGEST_WITHOUT_GLOSSARY_INPUT_XPATH = "//li[@data-type='AddSuggestsWithoutGlossary']//input";
        protected const string GLOSSARY_SEARCH_INPUT_XPATH = "//li[@data-type='GlossarySearch']//input";
        protected const string NEXT_BTN_XPATH = "//span[contains(@class,'js-next')]";
        protected const string ADD_BTN_XPATH = "//span[contains(@class,'js-add')]//a[contains(text(),'Add')]";
        protected const string ALL_GLOSSARIES_SELECT_XPATH = "//div[contains(@class,'js-scope-section')][2]//input[contains(@name,'accessRightScopeType')]";
        protected const string SAVE_BTN_XPATH = "//span[contains(@class,'js-save-btn')]";
    }
}