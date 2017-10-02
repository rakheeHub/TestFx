using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using OpenQA.Selenium;

namespace TestFx.Core.WebDriver
{
    internal static class ScreenshotHelper
    {
        public static string TakeScreenShot(IWebDriver webDriver, string screenShotFolder, string screenShotName = null)
        {
            if (!Directory.Exists(screenShotFolder))
            {
                Directory.CreateDirectory(screenShotFolder);
            }

            if (string.IsNullOrWhiteSpace(screenShotName))
            {
                screenShotName = string.Format("screenshot{0}", DateTime.UtcNow.Ticks);
            }

            var screenShot = webDriver as ITakesScreenshot;
            if (screenShot == null)
            {
                Trace.TraceWarning("Unable to cast webDriver to ITakesScreenshot");
                return null;
            }

            string screenShotFile = screenShotFolder + "\\" + screenShotName + ".jpeg";
            screenShot.GetScreenshot().SaveAsFile(screenShotFile, ImageFormat.Jpeg);

            return screenShotFile;
        }
    }
}
