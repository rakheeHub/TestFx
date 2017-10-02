using System.Collections.Generic;
using OpenQA.Selenium;

namespace TestFx.Core.PageDriver
{
    internal abstract class WebPageDriverBase
    {
        protected WebPageDriverBase(IWebDriver webDriver, IDictionary<string, string> pageElements)
        {
            _webDriver = webDriver;
            _pageElements = pageElements;
        }

        protected IJavaScriptExecutor JavaScriptExecutor
        {
            get { return (IJavaScriptExecutor)_webDriver; }
        }

        protected readonly IWebDriver _webDriver;
        protected IDictionary<string, string> _pageElements;
    }
}
