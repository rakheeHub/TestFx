using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestFx.Core.PageDriver
{
    /// <summary>
    /// General page driver helper that wraps waiting logic into a single place to allow
    /// page drivers to reuse in their own automation
    /// </summary>
    public static class PageDriverHelper
    {
        public static void GoToUrl(IWebDriver webDriver, string url)
        {
            webDriver.Navigate().GoToUrl(url);
        }

        public static void Refresh(IWebDriver webDriver)
        {
            webDriver.Navigate().Refresh();
        }

        public static bool TryRefresh(IWebDriver webDriver, bool logWarning = false)
        {
            try
            {
                Refresh(webDriver);
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }

        public static bool IsElementPresent(IWebDriver webDriver, By by, int timeOutInSeconds = 180, bool logWarning = false)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(ExpectedConditions.ElementExists(by));
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }

        public static bool IsElementVisible(IWebDriver webDriver, By by, int timeOutInSeconds = 180, bool logWarning = false)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }

        public static void ClickBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
            var element = wait.Until(ExpectedConditions.ElementExists(by));
            if (element != null)
            {
                element.Click();
            }
        }

        public static void ClickButtonBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
            var element = wait.Until(ExpectedConditions.ElementExists(by));
            if (element != null)
            {
                try
                {
                    element.SendKeys(Keys.Enter);
                }
                catch (Exception)
                {
                    element.Click();
                }
            }
        }

        public static bool TryClickButtonBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => ClickButtonBy(webDriver, @by, timeOutInSeconds), logWarning);
        }

        public static void ClickRadioButtonBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
            var element = wait.Until(ExpectedConditions.ElementExists(by));
            if (element != null)
            {
                element.SendKeys(Keys.Space);
            }
        }

        public static bool TryClickRadioButtonBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => ClickRadioButtonBy(webDriver, @by, timeOutInSeconds), logWarning);
        }

        public static void ClickCheckboxBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            ClickBy(webDriver, by, timeOutInSeconds);
            //ClickRadioButtonBy(webDriver, by, timeOutInSeconds);
        }

        public static bool TryClickCheckboxBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => ClickBy(webDriver, @by, timeOutInSeconds), logWarning);
            //return TryAction(() => ClickCheckboxBy(webDriver, @by, timeOutInSeconds), logWarning);
        }

        public static IWebElement FillOutTextBoxById(IWebDriver webDriver, string textBoxId, string textToFillIn, int timeOutInSeconds = 180)
        {
            var textBox = WaitUtility.WaitToDisplayByControlId(webDriver, textBoxId, timeOutInSeconds);
            if (textBox != null)
            {
                if (textToFillIn.Length > 100)
                {
                    string javaScript = String.Format("document.getElementById(\"{0}\").value = \"{1}\";", textBoxId, textToFillIn);
                    ((IJavaScriptExecutor)webDriver).ExecuteScript(javaScript);
                }
                else
                {
                    textBox.Clear();
                    textBox.SendKeys(textToFillIn);
                }
            }

            return textBox;
        }

        public static bool TryFillOutTextboxById(IWebDriver webDriver, string textBoxId, string textToFillIn, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => FillOutTextBoxById(webDriver, textBoxId, textToFillIn, timeOutInSeconds), logWarning);
        }

        public static void FillOutTextBoxByName(IWebDriver webDriver, string textBoxName, string textToFillIn, int timeOutInSeconds = 180)
        {
            IWebElement element = WaitUtility.GetControl(webDriver, By.Name(textBoxName), timeOutInSeconds);
            if (element != null)
            {
                if (!element.Enabled)
                {
                    var js = (IJavaScriptExecutor)webDriver;
                    js.ExecuteScript("arguments[0].click();", element);
                    SleepHelper.Sleep(1000);
                }

                if (textToFillIn.Length > 100)
                {
                    string javaScript = String.Format("document.getElementsByName(\"{0}\")[0].value = \"{1}\";", textBoxName, textToFillIn);
                    ((IJavaScriptExecutor)webDriver).ExecuteScript(javaScript);
                }
                else
                {
                    element.Clear();
                    element.SendKeys(textToFillIn);
                }
            }
        }

        public static bool TryFillOutTextboxByName(IWebDriver webDriver, string textBoxName, string textToFillIn, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => FillOutTextBoxByName(webDriver, textBoxName, textToFillIn, timeOutInSeconds), logWarning);
        }

        public static void SelectItem(IWebDriver webDriver, By by, string textToSelect, int timeOutInSeconds = 180)
        {
            var item = WaitUtility.WaitToDisplay(webDriver, by, timeOutInSeconds);
            if (item != null)
            {
                var selectItem = new SelectElement(item);
                selectItem.SelectByText(textToSelect);
            }
        }

        public static bool TrySelectItem(IWebDriver webDriver, By by, string textToSelect, int timeOutInSeconds = 180, bool logWarning = false)
        {
            return TryAction(() => SelectItem(webDriver, @by, textToSelect, timeOutInSeconds), logWarning);
        }

        public static string GetTextBy(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            var item = WaitUtility.GetControl(webDriver, by, timeOutInSeconds);
            return item.Text;
        }

        public static bool TryGetTextBy(IWebDriver webDriver, By by, out string text, int timeOutInSeconds = 180, bool logWarning = false)
        {
            text = null;

            try
            {
                text = GetTextBy(webDriver, by, timeOutInSeconds);
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }

        public static IWebElement GetElement(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            return WaitUtility.GetControl(webDriver, by, timeOutInSeconds);
        }

        public static bool TryGetElement(IWebDriver webDriver,
            By by,
            out IWebElement element,
            int timeOutInSeconds = 180,
            bool logWarning = false)
        {
            element = null;

            try
            {
                element = GetElement(webDriver, by, timeOutInSeconds);
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Wait for the given element to appear AND have a text which is not an empty string, then return its text value. 
        /// If unable to achieve this within the allotted time, an exception will be thrown.
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="by"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns>The element's text/value.</returns>
        public static string WaitForNonEmptyValueThenGetValue(IWebDriver webDriver, By by, int timeoutInSeconds)
        {
            var element = WaitUtility.WaitForNonEmptyText(webDriver, by, timeoutInSeconds);
            return element.GetAttribute("value");
        }

        public static void ClickAlert(IWebDriver webDriver)
        {
            IAlert alert = WaitUtility.WaitAlertToBePresent(webDriver);
            if (alert != null)
            {
                alert.Accept();
            }
        }

        private static bool TryAction(Action action, bool logWarning)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning(e.ToString());
                }

                return false;
            }

            return true;
        }
    }
}
