using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages.Components;

/// <summary>
/// Newsletter box in the footer. (The id "susbscribe_email" is misspelled on the real site.)
/// </summary>
public class FooterComponent : BasePage
{
    private readonly By _emailInput = By.Id("susbscribe_email");
    private readonly By _subscribeButton = By.Id("subscribe");
    private readonly By _successMessage = By.CssSelector("#success-subscribe .alert-success");

    public FooterComponent(IWebDriver driver) : base(driver) { }

    public void Subscribe(string email)
    {
        EnterText(_emailInput, email);
        Click(_subscribeButton);
    }

    public string GetSuccessMessage() => GetText(_successMessage);

    /// <summary>The browser's HTML5 validation message. Empty when the email is valid.</summary>
    public string GetEmailValidationMessage() =>
        WaitForVisible(_emailInput).GetDomProperty("validationMessage") ?? string.Empty;
}
