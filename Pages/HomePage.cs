using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

public class HomePage : BasePage
{
    public HomePage(IWebDriver driver) : base(driver) { }

    public void Open() => OpenPath("/");
}
