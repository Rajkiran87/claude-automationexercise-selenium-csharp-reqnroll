using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

public class AccountCreatedPage : BasePage
{
    private readonly By _heading = By.CssSelector("h2[data-qa='account-created']");
    private readonly By _continueButton = By.CssSelector("a[data-qa='continue-button']");

    public AccountCreatedPage(IWebDriver driver) : base(driver) { }

    public string GetHeading() => GetText(_heading);

    public void ClickContinue() => Click(_continueButton);
}
