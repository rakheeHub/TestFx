using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestFx.Core.PageDriver
{
    /// <summary>
    /// This class uses WebDriverWait (explicit wait) for waiting an element or javaScript.
    /// </summary>
    internal static class WaitUtility
    {
        public static bool WaitForPageRedirect(IWebDriver webDriver,
            string oldUrl,
            int timeOutInSeconds = 180,
            bool logWarning = false)
        {
            bool isPageTheSame;
            var timeOutTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(timeOutInSeconds));

            do
            {
                SleepHelper.Sleep(1000);
                isPageTheSame = webDriver.Url == oldUrl;
            }
            while (isPageTheSame && DateTime.UtcNow < timeOutTime);

            if (!isPageTheSame && logWarning)
            {
                Trace.TraceWarning("Page did not change after [{0}] seconds", timeOutInSeconds);
            }

            return !isPageTheSame;
        }

        /// <summary>
        /// This method waits for the element to appear and checks its text. 
        /// If the text is not empty, the element is returned. 
        /// If the element is not present and/or the text is still empty when the timeout expires, a timeout exception is trhown.
        /// </summary>
        /// <param name="webdriver"></param>
        /// <param name="by"></param>
        /// <param name="timeOutInSeconds"></param>
        internal static IWebElement WaitForNonEmptyText(IWebDriver webdriver, By by, int timeOutInSeconds)
        {
            string text = string.Empty;
            Stopwatch stopWatch = Stopwatch.StartNew();
            do
            {
                var element = webdriver.FindElement(by);
                if (element != null)
                {
                    text = element.GetAttribute("value");
                }

                if (string.IsNullOrEmpty(text))
                {
                    SleepHelper.Sleep();
                }
                else
                {
                    return element;
                }
            }
            while (stopWatch.Elapsed.TotalSeconds <= timeOutInSeconds);

            throw new TimeoutException();
        }

        /// <summary>
        /// This method waits for the specified amount of seconds for the current page to finish loading. If the loading 
        /// process is not complete before the given TimeSpan runs out, an exception is thrown.
        /// </summary>
        internal static bool WaitForPageLoadComplete(IWebDriver webdriver, int timeOutInSeconds, bool logWarning = false)
        {
            try
            {
                var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
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
        /// This method waits for the loading page to no longer be visible
        /// </summary>
        internal static void WaitForLoadingPageToComplete(IWebDriver webDriver, int timeOutInSeconds, bool logWarning = false)
        {
            var waitUntil = DateTime.UtcNow.AddSeconds(timeOutInSeconds);
            IWebElement hiddenLoadingPage;
            IWebElement loadingPage = null;
            do
            {
                try
                {
                    loadingPage = webDriver.FindElement(By.ClassName("loading-page"));
                    hiddenLoadingPage = webDriver.FindElement(By.CssSelector("div.loading-page.hide"));
                }
                catch (Exception)
                {
                    // element not found, which means the loading page is still showing
                    hiddenLoadingPage = null;
                    SleepHelper.Sleep(1000);
                }
            }
            while (DateTime.UtcNow < waitUntil && hiddenLoadingPage == null && loadingPage != null);

            if (hiddenLoadingPage == null && logWarning)
            {
                Trace.TraceWarning("Loading page was found on the page for [{0}] seconds", timeOutInSeconds);
            }
        }

        /// <summary>
        ///  Wait for a link to be present in the DOM/page
        /// </summary>
        /// <param name="webDriver">The UI automation driver</param>
        /// <param name="by">Used to determine how we find an element on a page</param>
        /// <param name="timeOutInSeconds">The timeout to wait in seconds</param>
        internal static IWebElement WaitUntilElementIsPresent(IWebDriver webDriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementExists(by));
        }

        /// <summary>
        /// Wait for an element to be Visible in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="by">selector to find the element</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static IWebElement WaitToVisibleByControlId(IWebDriver webdriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Wait for an element to be present in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="controlId">selector to find the element</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static IWebElement WaitByControlId(IWebDriver webdriver, string controlId, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementExists(By.Id(controlId)));
        }

        internal static IWebElement WaitByControlId(IWebDriver webdriver, string controlId, int timeOutInSeconds, out Exception exception)
        {
            exception = null;

            try
            {
                return WaitByControlId(webdriver, controlId, timeOutInSeconds);
            }
            catch (Exception e)
            {
                exception = e;
            }

            return null;
        }

        /// <summary>
        /// Wait for an element to be present in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="by">selector to find the element</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static IWebElement GetControl(IWebDriver webdriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementExists(by));
        }

        /// <summary>
        /// Wait for an element to be present in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="controlId">selector to find the element</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static void WaitElementToDisappear(IWebDriver webdriver, string controlId, int timeOutInSeconds = 120)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            wait.Until(d => d.FindElements(By.Id(controlId)).Count == 0);
        }

        /// <summary>
        /// Wait for an element to be Visible in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="controlId">selector to find the element</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static IWebElement WaitToDisplayByControlId(IWebDriver webdriver, string controlId, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementExists(By.Id(controlId)));
        }

        internal static IWebElement WaitToDisplay(IWebDriver webdriver, By by, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        /// <summary>
        /// Wait for an element to be Visible in the DOM//page, and returns the first WebElement using the given method.
        /// </summary>
        /// <param name="webdriver">IWebDriver</param>
        /// <param name="className">selector to find the element by class name</param>
        /// <param name="timeOutInSeconds">The time in seconds to wait until returning a failure</param>
        /// <returns>WebElement	the first WebElement using the given method, or null (if the timeout is reached)</returns>
        internal static IWebElement WaitByClassName(IWebDriver webdriver, string className, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
        }

        internal static IWebElement WaitByClassName(IWebDriver webdriver, string className, int timeOutInSeconds, out Exception exception)
        {
            exception = null;
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            try
            {
                return wait.Until(ExpectedConditions.ElementExists(By.ClassName(className)));
            }
            catch (Exception e)
            {
                exception = e;
            }

            return null;
        }

        /// <summary>
        ///  Wait for a link to be present in the DOM//page
        /// </summary>
        internal static IWebElement WaitByPartialLinkText(IWebDriver webDriver, string linkText, int timeout = 180, bool logWarning = false)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
                return wait.Until(ExpectedConditions.ElementExists(By.PartialLinkText(linkText)));
            }
            catch (WebDriverTimeoutException e)
            {
                if (logWarning)
                {
                    Trace.TraceInformation("Unable to find [{0}] within the time limit", linkText);
                    Trace.TraceWarning(e.ToString());
                }
            }

            return null;
        }

        internal static IWebElement WaitToDisplayByCssSelector(IWebDriver webdriver, string CSSSelector, int timeOutInSeconds = 180)
        {
            var wait = new WebDriverWait(webdriver, TimeSpan.FromSeconds(timeOutInSeconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(CSSSelector)));
        }

        /// <summary>
        /// Wait for the page's Title to change to the given value. Timeout exception may be thrown.
        /// </summary>
        /// <param name="webDriver">The current WebDriver.</param>
        /// <param name="pageTitle">The expected page Title string.</param>
        /// <param name="timeout">Max amount of seconds to wait before a Timeout exception is thrown.</param>
        /// <returns></returns>
        internal static bool WaitForPageTitle(IWebDriver webDriver, string pageTitle, int timeout = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
            return wait.Until(d => d.Title == pageTitle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webDriver">The current WebDriver.</param>
        /// <param name="timeout">Max amount of seconds to wait before a Timeout exception is thrown.</param>
        /// <returns>The IAlert web element.</returns>
        internal static IAlert WaitAlertToBePresent(IWebDriver webDriver, int timeout = 180)
        {
            IAlert alert = null;
            const int waitForAlert_seconds = 60;
            var stopWatch = Stopwatch.StartNew();
            while (alert == null)
            {
                System.Threading.Thread.Sleep(1000);
                alert = AlertIsPresent(webDriver);
                if (stopWatch.Elapsed.TotalSeconds > waitForAlert_seconds)
                {
                    stopWatch.Stop();
                    throw new NoAlertPresentException(string.Format("No alert for {0} seconds.", waitForAlert_seconds));
                }
            }
            stopWatch.Stop();
            return alert;
        }

        private static IAlert AlertIsPresent(IWebDriver drv)
        {
            try
            {
                // Attempt to switch to an alert
                return drv.SwitchTo().Alert();
            }
            catch (NoAlertPresentException)
            {
                // We ignore this execption, as it means there is no alert present...yet.
                return null;
            }

            // Other exceptions will be ignored and up the stack
        }

        internal static bool WaitElementToShowByJavaScript(IWebDriver webDriver, string javaScriptCommand, int timeoutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => ((IJavaScriptExecutor)webDriver).ExecuteScript(@"return " + javaScriptCommand).Equals(true));
        }

        internal static bool WaitElementToHideByJavaScript(IWebDriver webDriver, string javaScriptCommand, int timeoutInSeconds = 180)
        {
            var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(d => ((IJavaScriptExecutor)webDriver).ExecuteScript(@"return " + javaScriptCommand).Equals(false));
        }

        internal static bool IsElementPresentAndDisplay(IWebDriver webDriver, By by)
        {
            try
            {
                var element = webDriver.FindElement(by);
                if (element != null)
                {
                    return element.Enabled;
                }

                return false;
            }
            catch (NoSuchElementException e)
            {
                Trace.TraceWarning(e.ToString());
                return false;
            }
        }

        public static ReadOnlyCollection<IWebElement> WaitForElementsSafe(IWebDriver webDriver, By by, int timeoutInSeconds = 180, bool logWarning = false)
        {
            try
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(d => webDriver.FindElements(by));
            }
            catch (Exception e)
            {
                if (logWarning)
                {
                    Trace.TraceWarning("Unable to get elements by [{0}] due to: {1}", by, e);
                }
            }

            return null;
        }
    }
}
