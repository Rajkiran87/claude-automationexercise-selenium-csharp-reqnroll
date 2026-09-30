using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

public class HomePage : BasePage
{
    // The footer newsletter box is on every page, so it shows the home page has fully loaded
    private readonly By _subscribeEmail = By.Id("susbscribe_email");

    public HomePage(IWebDriver driver) : base(driver) { }

    public void Open() => OpenPath("/", _subscribeEmail);
}
