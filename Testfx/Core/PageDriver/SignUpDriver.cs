using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;

namespace TestFx.Core.PageDriver
{
    /// <summary>
    /// SignUpPageDriver is responsible for automating the sign up process for a user
    /// </summary>
    internal class SignUpDriver : WebPageDriverBase
    {
        /// <summary>
        /// Creates an instance of the SignUpPageDriver
        /// </summary>
        public static ISignUpPageDriver GetInstance(IWebDriver webDriver,
            IDictionary<string, string> pageElements,
            string userName,
            string title)
        {
            return new SignUpDriver(webDriver, pageElements, userName, title);
        }

        /// <summary>
        /// Automates the sign up process for a user
        /// </summary>
        public bool SignUp()
        {
            PageDriverHelper.GoToUrl(_webDriver, _pageElements["ApplicationURL"]));

            WaitUtility.WaitForPageLoadComplete(_webDriver, 180);
            WaitUtility.WaitForLoadingPageToComplete(_webDriver, 180);
            ScreenshotHelper.TaskScreenShotAndUpload(_webDriver, Directory.GetCurrentDirectory();
	    return true;
        }

        public bool SignOut()
        {
              var result = PageDriverHelper.ClickButtonById(_webDriver, _pageElements["btnSignOut"]);
	      return result;
        }

        private SignUpPageDriverV1(IWebDriver webDriver,
            IDictionary<string, string> pageElements,
            string userName,
            string title)
            : base(webDriver, pageElements)
        {
            _userName = userName;
            _title = title;
        }

        private readonly string _userName; 
        private readonly string _title;
    }
}
