using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbbyyLs.CAT.Function.Selenium.Tests
{
    public class GlossarySuggestedTerms : GlossaryTest
    {
        public GlossarySuggestedTerms(string url, string workspaceUrl, string browserName)
            : base(url, workspaceUrl, browserName)
        {

        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// Метод тестирования предложения термина без указания глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void SuggestWithoutGlossaryFromGlossaryListTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            // Проверить, что таких терминов стало больше
            Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения термина для глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void SuggestWithGlossaryFromGlossaryListTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Предложить термин для этого глоссария
            SuggestTermSetGlossary(glossaryName);

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Проверить, что терминов для этого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы глоссария
        /// </summary>
        [Test]
        public void SuggestWithGlossaryFromGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к предложенным терминам
            SwitchSuggestedTerms();
            // Проверить, что терминов для этого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(glossaryName) > 0, "Ошибка: нет предложенного термина для этого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы другого глоссария
        /// </summary>
        [Test]
        public void SuggestWithGlossaryFromAnotherGlossaryTest()
        {
            // Создать один глоссарий
            string firstGlossaryName = CreateGlossaryAndReturnToGlossaryList();
            // Создать другой глоссарий
            string secondGlossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(secondGlossaryName);

            // Предложить термин для первого глоссария на странице второго глоссария
            SuggestTermSetGlossary(firstGlossaryName);

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Проверить, что терминов для первого глоссария больше нуля
            Assert.IsTrue(GetCountSuggestTermsGlossary(firstGlossaryName) > 0, "Ошибка: нет предложенного термина для первого глоссария");
        }

        /// <summary>
        /// Метод тестирования предложения термина со страницы глоссария без привязки к глоссарию
        /// </summary>
        [Test]
        public void SuggestWithoutGlossaryFromAnotherGlossaryTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountBefore = GetCountSuggestTermsWithoutGlossary();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Создать  глоссарий
            CreateGlossaryByName(GetUniqueGlossaryName());

            // Предложить термин с отсутствием глоссария
            SuggestTermSetGlossary("");

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            // Проверить, что количество терминов без глоссария увеличилось
            Assert.IsTrue(unglossaryTermsCountAfter > unglossaryTermsCountBefore, "Ошибка: термин без указанного глоссария не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина со страницы глоссария, проверка появления предупреждения
        /// </summary>
        [Test]
        public void SuggestExistingTermWarningFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            // Предложить термин
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(".//div[contains(@class,'js-duplicate-warning')]")).Displayed,
                "Ошибка: не появилось предупреждение о существующем термине");
            // Нажать отмену
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]")).Click();
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что нет предложенных терминов
            Assert.IsTrue(GetCountOfSuggestTerms() == 0, "Ошибка: предложенный термин сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина со страницы глоссария, одобрение
        /// </summary>
        [Test]
        public void SuggestExistingTermAcceptFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);
            // Предложить термин
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2);
            // Согласиться
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//input[contains(@class,'js-save-btn')]")).Click();
            Thread.Sleep(500);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что предложенный термин сохранился
            Assert.IsTrue(GetCountOfSuggestTerms() > 0, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина из списка глоссариев, проверка появления предупреждения
        /// </summary>
        [Test]
        public void SuggestExistingTermWarningFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);

            // Перейти в список глоссариев
            SwitchGlossaryTab();

            // Предложить термин  с указанием глоссария
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
            // Проверить, что появилось предупреждение
            Assert.IsTrue(Driver.FindElement(By.XPath(".//div[contains(@class,'js-duplicate-warning')]")).Displayed,
                "Ошибка: не появилось предупреждение о существующем термине");
            // Нажать отмену
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//a[contains(@class,'g-popupbox__cancel js-popup-close')]")).Click();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что нет предложенных терминов
            Assert.IsTrue(GetCountOfSuggestTerms() == 0, "Ошибка: предложенный термин сохранился");
        }

        /// <summary>
        /// Метод тестирования предложения существующего термина из списка глоссариев, одобрение
        /// </summary>
        [Test]
        public void SuggestExistingTermAcceptFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            string uniquePrefix = DateTime.Now.ToString();
            string SuggestTerm1 = "Suggest Term 1" + uniquePrefix;
            string SuggestTerm2 = "Suggest Term 2" + uniquePrefix;
            // Создать термин
            CreateItemAndSave(SuggestTerm1, SuggestTerm2);

            // Перейти в список глоссариев
            SwitchGlossaryTab();
            // Предложить термин  с указанием глоссария
            SuggestTermAndSave(SuggestTerm1, SuggestTerm2, true, glossaryName);
            // Согласиться
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//input[contains(@class,'js-save-btn')]")).Click();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);
            // Перейти в предложенные термины
            SwitchSuggestTermCurrentGlossary();
            // Проверить, что предложенный термин сохранился
            Assert.IsTrue(GetCountOfSuggestTerms() > 0, "Ошибка: предложенный термин не сохранился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void AcceptWithGlossaryFromGlossaryListTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // В строке с термином для текущего глоссария нажать "Принять"
            ClickButtonSuggestTermRowByGlossary(glossaryName, "sugg js-accept-suggest");

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы этого глоссария
        /// </summary>
        [Test]
        public void AcceptWithGlossaryFromGlossaryTest()
        {
            // Создать  глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();
            // Перейти в предложенные термины для этого глоссария
            SwitchSuggestTermCurrentGlossary();
            // Одобрить термин
            ClickButtonSuggestTermRow("sugg js-accept-suggest");
            // Перейти в этот глоссарий
            SwitchGlossaryFromSuggestedTerm();

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина с указанным глоссарием со страницы другого глоссария
        /// </summary>
        [Test]
        public void AcceptWithGlossaryFromAnotherGlossaryTest()
        {
            // Создать глоссарий
            string glossaryNameWithSuggestTerm = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryNameWithSuggestTerm);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Создать другой глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти в предложенные термины для этого глоссария
            SwitchSuggestTermCurrentGlossary();

            // Открыть выпадающий список с выбором глоссария
            Driver.FindElement(By.XPath(".//span[contains(@class, 'js-dropdown sigggloss g-drpdwn g-iblock')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'js-dropdown__list sigggloss g-drpdwn__list')]")));
            // Выбрать в выпавшем списке наш первый созданный глоссарий
            string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item sigggloss g-drpdwn__item')][@title='" + glossaryNameWithSuggestTerm + "']";
            Driver.FindElement(By.XPath(xPathGlossary)).Click();
            Thread.Sleep(500);

            // Одобрить термин
            ClickButtonSuggestTermRow("sugg js-accept-suggest");
            
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в наш первый глоссарий
            SwitchCurrentGlossary(glossaryNameWithSuggestTerm);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования одобрения термина без глоссария со страницы со списком глоссариев
        /// </summary>
        [Test]
        public void AcceptWithoutGlossaryFromGlossaryListTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();

            // Предложить термин
            CreateSuggestTerm();
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();

            // Найти все термины
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Найти термин без указанного глоссария (с пустым полем глоссария)
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Одобрить этот термин
                    ClickButtonSuggestTermRow("sugg js-accept-suggest", true, (i + 1));
                    // Дождаться появления формы выбора глоссария
                    Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class, 'js-dropdown siggglossp')]"))).Click();
                    // Выбрать нужный глоссарий
                    string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item siggglosspp')][@title='" + glossaryName + "']";
                    Wait.Until((d) => d.FindElement(By.XPath(xPathGlossary))).Click();
                    // Сохранить
                    Driver.FindElement(By.XPath(".//input[contains(@class, 'js-glossary-selected-button')]")).Click();
                    Thread.Sleep(5000);
                    break;
                }
            }

            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);

            // Проверить количество терминов
            Assert.IsTrue(GetCountOfItems() > 0, "Ошибка: термин не добавился");
        }

        /// <summary>
        /// Метод тестирования удаления предложенного термина без глоссария
        /// </summary>
        [Test]
        public void DeleteWithoutGlossaryTest()
        {
            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Получить количество терминов без указанного глоссария
            int unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
            if (unglossaryTermsCount == 0)
            {
                // Перейти к списку глоссариев
                SwitchGlossaryTab();
                // Предложить термин
                CreateSuggestTerm();
                // Перейти к списку предложенных терминов
                SwitchSuggestedTerms();
                // Получить количество терминов без указанного глоссария
                unglossaryTermsCount = GetCountSuggestTermsWithoutGlossary();
            }

            // Найти все предложенные термины
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Найти термин без указанного глоссария (с пустым полем глоссария)
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Удалить термин
                    ClickButtonSuggestTermRow("js-reject-suggest", true, (i + 1));
                    break;
                }
            }
            // Проверить количество терминов без указанного глоссария
            int unglossaryTermsCountAfter = GetCountSuggestTermsWithoutGlossary();
            Assert.IsTrue(unglossaryTermsCountAfter < unglossaryTermsCount, "Ошибка: предложенный термин не удалился");
        }

        /// <summary>
        /// Метод тестирования удаления предложенного термина для глоссария
        /// </summary>
        [Test]
        public void DeleteWithGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Удалить термин
            ClickButtonSuggestTermRow("js-reject-suggest");

            // Проверить количество предложенных терминов глоссария
            int count = 0;
            IList<IWebElement> termList = Driver.FindElements(By.XPath("//tr[contains(@class, 'js-suggest-row')]"));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Проверить, что термин не скрыт (удаленные термины становятся скрытыми до обновления страницы)
                if (!termList[i].GetAttribute("class").Contains("g-hidden"))
                {
                    ++count;
                }
            }

            // Проверить количество предложенных терминов
            Assert.IsTrue(count == 0, "Ошибка: термин не удалился");
        }

        /// <summary>
        /// Метод тестирования редактирования предложенного термина из глоссария
        /// </summary>
        [Test]
        public void EditFromGlossaryTest()
        {
            // Предложить термин глоссарию и открыть редактирование
            SuggestWithGlossaryClickEdit();
            string newTermText = "New Term Text" + DateTime.UtcNow.ToString();

            // Ввести в термин новое значение
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            // Перейти в другому языку
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-viewer')]")).Click();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            // Принять термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            Assert.IsTrue(WaitUntilDisappearElement(".//span[contains(@class,'js-save-btn js-edit')]"), "Ошибка: предложенный термин не сохранился");
            // Перейти в глоссарий
            SwitchGlossaryFromSuggestedTerm();

            // Проверить термин в глоссарии
            bool isTermAccepted = false;
            IList<IWebElement> termLangList = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')]//p"));
            foreach (IWebElement el in termLangList)
            {
                if (el.Text.Trim() == newTermText)
                {
                    isTermAccepted = true;
                    break;
                }
            }
            Assert.IsTrue(termLangList.Count > 0, "Ошибка: термин не сохранился");
            Assert.IsTrue(isTermAccepted, "Ошибка: термин сохранился неотредактированным");
        }

        /// <summary>
        /// Метод тестирования редактирования предложенного термина из глоссария - добавление синонима
        /// </summary>
        [Test]
        public void EditFromGlossaryAddSynonymTest()
        {
            // Предложить термин глоссарию и открыть редактирование
            SuggestWithGlossaryClickEdit();
            string newTermText = "New Term Text" + DateTime.UtcNow.ToString();

            // Кликнуть добавить термин во второй язык
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-add-term')]")).Click();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//div[contains(@class,'js-term-node')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//div[contains(@class,'js-term-node')]//input")).SendKeys(newTermText);
            // Принять термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn')]")).Click();
            Assert.IsTrue(WaitUntilDisappearElement(".//span[contains(@class,'js-save-btn js-edit')]"), "Ошибка: предложенный термин не сохранился");
            // Перейти в глоссарий
            SwitchGlossaryFromSuggestedTerm();

            // Проверить термин в глоссарии
            bool isTermAccepted = false;
            IList<IWebElement> termLangList = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][2]//p"));
            foreach (IWebElement el in termLangList)
            {
                if (el.Text.Trim() == newTermText)
                {
                    isTermAccepted = true;
                    break;
                }
            }
            Assert.IsTrue(termLangList.Count > 0, "Ошибка: термин не сохранился");
            Assert.IsTrue(isTermAccepted, "Ошибка: термин сохранился неотредактированным");
        }

        /// <summary>
        /// Метод тестирования редактирования предложенного термина без глоссария
        /// </summary>
        [Test]
        public void EditWithoutGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Предложить термин
            CreateSuggestTerm();

            // Перейти к списку предложенных терминов
            SwitchSuggestedTerms();
            // Создать новое имя термина
            string newTermText = "New Term Text" + DateTime.UtcNow.ToString();

            // Получить номер строчки с термином без указанного глоссария
            int rowNumber = 0;
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Если после удаления пробелов нет символов, значит, что глоссарий не указан
                if (termList[i].Text.Trim().Length == 0)
                {
                    rowNumber = i + 1;
                    break;
                }
            }
            // Проверить, что термин предложился
            Assert.IsTrue(rowNumber > 0, "Ошибка: термин не предложился");

            // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
            Driver.Manage().Window.Maximize();
            // Нажать на редактирование
            ClickButtonSuggestTermRow("js-edit-suggest", true, rowNumber);

            // Дождаться появления формы выбора глоссария
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-select-glossary-popup')]//span[contains(@class, 'js-dropdown siggglossp')]"))).Click();
            // Выбрать нужный глоссарий
            string xPathGlossary = ".//span[contains(@class, 'js-dropdown__item siggglosspp')][@title='" + glossaryName + "']";
            Wait.Until((d) => d.FindElement(By.XPath(xPathGlossary))).Click();
            // Сохранить
            Driver.FindElement(By.XPath(".//input[contains(@class, 'js-glossary-selected-button')]")).Click();

            // Ввести в термин новое значение
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][1]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            // Перейти в другому языку
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-viewer')]")).Click();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).Clear();
            Driver.FindElement(By.XPath(".//div[contains(@class,'l-corprtree__langbox')][2]//span[contains(@class,'js-term-editor')]//input")).SendKeys(newTermText);
            // Принять термин
            Driver.FindElement(By.XPath(".//span[contains(@class,'js-save-btn js-edit')]")).Click();
            Thread.Sleep(5000);
            // Перейти к списку глоссариев
            SwitchGlossaryTab();
            // Перейти в глоссарий
            SwitchCurrentGlossary(glossaryName);

            // Проверить термин в глоссарии
            bool isTermAccepted = false;
            IList<IWebElement> termLangList = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-concept-row')]//td[contains(@class,'glossaryShort')][1]/p"));
            foreach (IWebElement el in termLangList)
            {
                if (el.Text.Trim() == newTermText)
                {
                    isTermAccepted = true;
                    break;
                }
            }
            Assert.IsTrue(termLangList.Count > 0, "Ошибка: термин не сохранился");
            Assert.IsTrue(isTermAccepted, "Ошибка: термин сохранился неотредактированным");
        }

        /// <summary>
        /// Тест: проверка автоматического переключения языков
        /// Создается глоссарий, открывается форма предложения термина
        /// Для первого термина открывается выпадающий список языков
        /// Проверяется, что там два языка: английский и русский
        /// Выбирается Русский
        /// Проверяется, что для второго термина язык меняется на Английский
        /// </summary>
        [Test]
        public void AutoSwitchingLanguageTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Нажать Предложить термин
            Driver.FindElement(By.XPath(".//span[contains(@class, 'g-redbtn js-add-suggest')]//a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-add-suggest-popup')]")).Displayed);

            // Кликнуть по English
            Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-language')][1]//span[contains(@class,'addsugglang')]")).Click();
            // Получить список элементов в выпавшем списке языков
            IList<IWebElement> langList = Driver.FindElements(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')]"));
            
            // Проверить, что там два элемента - Английский и Русский
            Assert.IsTrue(langList.Count == 2, "Ошибка: в списке должно быть два языка");
            Assert.IsTrue(int.Parse(langList[0].GetAttribute("data-id")) == 9, "Ошибка: первый язык должен быть Английский");
            Assert.IsTrue(int.Parse(langList[1].GetAttribute("data-id")) == 25, "Ошибка: первый язык должен быть Русский");

            // Выбрать Русский
            Driver.FindElement(By.XPath(
                ".//span[contains(@class,'js-dropdown__list')]//span[contains(@class,'js-dropdown__item')][@data-id='25']")).Click();
            
            WaitUntilDisappearElement(".//span[contains(@class,'js-dropdown__list')]");
            // Проверить, что вторым языком стал Английский
            Assert.IsTrue(int.Parse(Driver.FindElement(By.XPath(
                ".//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-language')][2]//span[contains(@class,'js-dropdown__text')]")).GetAttribute("data-id")) == 9,
                "Ошибка: второй язык не изменился на английский");
        }

        /// <summary>
        /// Тест: предложение пустого термина (из списка глоссариев)
        /// Проверка: появляется ошибка
        /// </summary>
        [Test]
        public void SuggestEmptyTermFromGlossaryListTest()
        {
            // Нажать Предложить термин
            Driver.FindElement(By.XPath(".//span[contains(@class, 'g-redbtn js-add-suggest')]//a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-add-suggest-popup')]")).Displayed);

            // Сохранить
            Driver.FindElement(By.XPath(".//input[contains(@class, 'js-save-btn')]")).Click();
            
            // Проверить, что появилась ошибка
            Assert.IsTrue(WaitUntilDisplayElement(".//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-error-message')]"),
                "Ошибка: не появилась ошибка пустого термина");
        }

        /// <summary>
        /// Тест: предложение пустого термина (из глоссария)
        /// Проверка: появляется ошибка
        /// </summary>
        [Test]
        public void SuggestEmptyTermFromGlossaryTest()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Нажать Предложить термин
            Driver.FindElement(By.XPath(".//span[contains(@class, 'g-redbtn js-add-suggest')]//a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//div[contains(@class,'js-popup-bd js-add-suggest-popup')]")).Displayed);

            // Сохранить
            Driver.FindElement(By.XPath(".//input[contains(@class, 'js-save-btn')]")).Click();
            
            // Проверить, что появилась ошибка
            Assert.IsTrue(WaitUntilDisplayElement(".//div[contains(@class,'js-add-suggest-popup')]//div[contains(@class,'js-error-message')]"),
                "Ошибка: не появилась ошибка пустого термина");
        }

        /// <summary>
        /// Не тест: перед выполнением остальных тестов нужно проверить права пользователя
        /// Если нет прав на предложение терминов без указания глоссария,
        /// добавить права
        /// </summary>
        [Test]
        public void aaaaaFirstTestCheckUserRights()
        {
            AddUserRights();
        }

        protected int GetCountSuggestTermsGlossary(string glossaryName)
        {
            // Найти термины, относящиеся к текущему глоссарию
            string xPathTermRow = GetSuggestTermRowsXPath() + "[contains(text(),'" + glossaryName + "')]";
            // Вернуть количество таких терминов
            return Driver.FindElements(By.XPath(GetSuggestTermRowsXPath() + "[contains(text(),'" + glossaryName + "')]")).Count;
        }


        protected void ClickButtonSuggestTermRowByGlossary(string glossaryName, string btnName)
        {
            // Найти термины, относящиеся к глоссарию
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                if (termList[i].Text.Trim() == glossaryName)
                {
                    // Нажать в строке с этим глоссарием кнопку
                    ClickButtonSuggestTermRow(btnName, true, (i + 1));
                    break;
                }
            }
        }


        protected void ClickButtonSuggestTermRow(string btnName, bool isNeedSelectCurrentRow = false, int rowNumber = 0, string glossaryName = "")
        {
            int countBefore = GetCountOfSuggestTerms();
            Console.WriteLine(countBefore);
            // Расширить окно, чтобы кнопка была видна, иначе Selenium ее "не видит" и выдает ошибку
            Driver.Manage().Window.Maximize();
            // Получить xPath ячейки с комментарием в строке
            string xPathTermRow = ".//tr[contains(@class, 'js-suggest-row')]";
            if (isNeedSelectCurrentRow)
            {
                xPathTermRow += "[" + rowNumber + "]";
            }
            xPathTermRow += "//td[contains(@class, 'l-corpr__td suggestComment')]";

            // Нажать на ячейку (если список длинный, то после первого клика браузер прокручивается до нужной строки, а по второму клику нажимает ячейку)
            Driver.FindElement(By.XPath(xPathTermRow)).Click();
            Driver.FindElement(By.XPath(xPathTermRow)).Click();
            // Нажать кнопку
            xPathTermRow += "//a[contains(@class, '" + btnName + "')]";
            Driver.FindElement(By.XPath(xPathTermRow)).Click();

            // Если не дождаться обновления списка предложенных терминов - появляется модальное окно с ошибкой, которое отлавливает Selenium
            if (GetCountOfSuggestTerms() == countBefore)
            {
                Thread.Sleep(3000);
            }
        }


        protected void SuggestTermSetGlossary(string glossaryName)
        {
            // Предложить термин и выбрать другой глоссарий
            SuggestTermAndSave("Suggest Term 1", "Suggest Term 2", true, glossaryName);
        }


        protected void CreateSuggestTerm()
        {
            // Предложить термин
            SuggestTermAndSave("Suggest Term 1", "Suggest Term 2");
        }


        protected int GetCountSuggestTermsWithoutGlossary()
        {
            int count = 0;
            // Получить все строки с предложенными терминами
            IList<IWebElement> termList = Driver.FindElements(By.XPath(GetSuggestTermRowsXPath()));
            for (int i = 0; i < termList.Count; ++i)
            {
                // Если после удаления пробелов нет символов, значит, что глоссарий не указан
                if (termList[i].Text.Trim().Length == 0)
                {
                    // Проверить, что строка не скрыта
                    string xPath = ".//tr[contains(@class, 'js-suggest-row')][" + (i + 1) + "]";
                    if (!Driver.FindElement(By.XPath(xPath)).GetAttribute("class").Contains("g-hidden"))
                    {
                        ++count;
                    }
                }
            }
            // Возвращаем количество терминов без указанного глоссария
            return count;
        }


        protected int GetCountOfSuggestTerms()
        {
            setDriverTimeoutMinimum();
            // Получить количество предложенных терминов
            int result = Driver.FindElements(By.XPath(".//tr[contains(@class, 'js-suggest-row')]")).Count;
            setDriverTimeoutDefault();
            return result;
        }

        protected void SwitchGlossaryFromSuggestedTerm()
        {
            // Перейти в глоссарий из его предложенных терминов
            Driver.FindElement(By.XPath(".//div[contains(@class, 'l-corprsubmn__data')]/ul//li[2]/a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__addbtnbox js-buttons-block')]")).Displayed);
        }


        protected void SwitchSuggestTermCurrentGlossary()
        {
            // Перейти в предложенные термины для этого глоссария
            Driver.FindElement(By.XPath(".//div[contains(@class, 'l-corprsubmn__data')]/ul//li[3]/a")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__sugggloss')]")).Displayed);
        }

        protected void SwitchSuggestedTerms()
        {
            // Перейти на страницу Предложенные термины (из списка глоссариев)
            Driver.FindElement(By.XPath(".//a[contains(@href,'/Suggests')]")).Click();
            Wait.Until((d) => d.FindElement(By.XPath(".//span[contains(@class,'l-corpr__sugggloss')]")).Displayed);
        }

        protected void SuggestWithGlossaryClickEdit()
        {
            // Создать глоссарий
            string glossaryName = GetUniqueGlossaryName();
            CreateGlossaryByName(glossaryName);

            // Предложить термин
            CreateSuggestTerm();
            // Перейти к списку предложенных терминов
            SwitchSuggestTermCurrentGlossary();

            // Расширить окно, чтобы кнопка была видна, иначе она недоступна для Selenium
            Driver.Manage().Window.Maximize();
            // Нажать на редактирование
            ClickButtonSuggestTermRow("js-edit-suggest");
        }
    }
}
