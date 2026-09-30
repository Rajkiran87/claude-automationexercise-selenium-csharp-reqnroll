using AutomationExercise.Tests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationExercise.Tests.Pages;

/// <summary>
/// Parent class for all page objects. Every action waits for the element first,
/// so tests do not fail just because the page was still loading.
/// </summary>
public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigReader.TimeoutSeconds));
        // Keep retrying while the element is missing or was re-rendered by the page
        Wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
    }

    /// <summary>Opens a page of the site, e.g. OpenPath("/login").</summary>
    protected void OpenPath(string path) => Driver.Navigate().GoToUrl(ConfigReader.BaseUrl + path);

    /// <summary>
    /// Opens a page and waits until <paramref name="readyLocator"/> is visible.
    /// The public demo site sometimes returns an empty or half-loaded page when it is busy.
    /// In that case we reload once and log a warning, so the slowness is still visible in the logs.
    /// If the page is still broken after the reload, the test fails as normal.
    /// </summary>
    protected void OpenPath(string path, By readyLocator)
    {
        LoadIgnoringSlowResources(() => OpenPath(path), path);
        try
        {
            WaitForVisible(readyLocator);
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"Warning: {path} did not finish loading in {ConfigReader.TimeoutSeconds}s, reloading once.");
            LoadIgnoringSlowResources(() => Driver.Navigate().Refresh(), path);
            WaitForVisible(readyLocator);
        }
    }

    /// <summary>
    /// Runs a navigation. If the browser's page-load timeout expires (usually a slow third-party
    /// script), we log it and carry on: the caller then checks whether the page is usable.
    /// </summary>
    private static void LoadIgnoringSlowResources(Action navigate, string path)
    {
        try
        {
            navigate();
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"Warning: {path} hit the page-load timeout; checking whether it is usable anyway.");
        }
    }

    protected IWebElement WaitForVisible(By locator) =>
        Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            return element.Displayed ? element : null;
        })!;

    protected void WaitForInvisible(By locator) =>
        Wait.Until(d => d.FindElements(locator).All(element => !element.Displayed));

    /// <summary>
    /// Waits until the element is visible and enabled, scrolls to it and clicks.
    /// If something (an ad, a sticky header) covers it, falls back to a JavaScript click.
    /// </summary>
    protected void Click(By locator)
    {
        var element = Wait.Until(d =>
        {
            var e = d.FindElement(locator);
            return e.Displayed && e.Enabled ? e : null;
        })!;

        ScrollIntoView(element);
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }

    protected void EnterText(By locator, string text)
    {
        var element = WaitForVisible(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetText(By locator) => WaitForVisible(locator).Text.Trim();

    protected void SelectByValue(By locator, string value) =>
        new SelectElement(WaitForVisible(locator)).SelectByValue(value);

    protected void SelectByText(By locator, string text) =>
        new SelectElement(WaitForVisible(locator)).SelectByText(text);

    /// <summary>Visible text of every matching element (empty list if none).</summary>
    protected List<string> GetAllTexts(By locator) =>
        Driver.FindElements(locator).Select(element => element.Text.Trim()).ToList();

    protected bool IsDisplayed(By locator)
    {
        var elements = Driver.FindElements(locator);
        return elements.Count > 0 && elements[0].Displayed;
    }

    public string CurrentUrl => Driver.Url;

    private void ScrollIntoView(IWebElement element) =>
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
}
