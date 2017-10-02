using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace TestFx.Core.WebDriver
{
    /// <summary>
    /// This class is a factory for creating and managing specific Selenium drivers, such as Internet Expolorer, Firefox, etc. 
    /// Due to some limitations inherent to Selenium and AzureBilling.Test application, 
    /// we prefer to not have more than 1 browser window of the same browser type at the same time (such as, 2 windows of IE), 
    /// hence the usage of singletons.
    /// </summary>
    internal class SeleniumBrowserFactory
    {
        public static IWebDriver GetInternetExplorerDriver()
        {
            return _internetExplorer.Value;
        }

        public static IWebDriver GetChromeDriver()
        {
            return _chrome.Value;
        }

        public static IWebDriver GetPhantomJSDriver()
        {
            return _phantomJS.Value;
        }

        public static void Cleanup()
        {
            Cleanup(_internetExplorer);
            _internetExplorer = GetIEWebDriver();

            Cleanup(_chrome);
            _chrome = GetChromeWebDriver();

            Cleanup(_phantomJS);
            _phantomJS = GetPhantomJSWebDriver();
        }

        private static void Cleanup(Lazy<IWebDriver> webDriver)
        {
            if (webDriver != null && webDriver.IsValueCreated && webDriver.Value != null)
            {
                webDriver.Value.Quit();
                webDriver.Value.Dispose();
            }
        }

        private static Lazy<IWebDriver> GetIEWebDriver()
        {
            return new Lazy<IWebDriver>(() =>
            {
                var options = new InternetExplorerOptions
                {
                    IgnoreZoomLevel = true,
                    EnsureCleanSession = true
                };

                var ie = new InternetExplorerDriver(options);
                ie.Manage().Timeouts().SetPageLoadTimeout(new TimeSpan(0, 500, 0));
                ie.Manage().Cookies.DeleteAllCookies();

                return ie;
            });
        }

        private static Lazy<IWebDriver> GetChromeWebDriver()
        {
            return new Lazy<IWebDriver>(() =>
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("no-sandbox", "start-maximized", "no-default-browser-check");
                chromeOptions.AddUserProfilePreference("download.default_directory", currentDirectory + @"\DownloadBillingSheets");
                chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
                DesiredCapabilities capabilities = DesiredCapabilities.Chrome();
                ChromeDriverService chromeService = ChromeDriverService.CreateDefaultService(currentDirectory, "chromedriver.exe");
                capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);

                var chrome = new ChromeDriver(chromeService, chromeOptions);
                chrome.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                chrome.Manage().Cookies.DeleteAllCookies();
                return chrome;
            });
        }

        private static Lazy<IWebDriver> GetPhantomJSWebDriver()
        {
            return new Lazy<IWebDriver>(() => new PhantomJSDriver());
        }

        private static Lazy<IWebDriver> _internetExplorer = GetIEWebDriver();
        private static Lazy<IWebDriver> _chrome = GetChromeWebDriver();
        private static Lazy<IWebDriver> _phantomJS = GetPhantomJSWebDriver();
    }
}
