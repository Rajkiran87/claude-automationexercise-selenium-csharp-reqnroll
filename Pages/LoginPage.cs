using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

/// <summary>
/// "Signup / Login" page: Login form on the left, New User Signup on the right.
/// Locators use the site's data-qa attributes, which exist for testing and rarely change.
/// </summary>
public class LoginPage : BasePage
{
    // Login form
    private readonly By _loginEmail = By.CssSelector("input[data-qa='login-email']");
    private readonly By _loginPassword = By.CssSelector("input[data-qa='login-password']");
    private readonly By _loginButton = By.CssSelector("button[data-qa='login-button']");
    private readonly By _loginError = By.CssSelector("form[action='/login'] p");

    // Signup form
    private readonly By _signupName = By.CssSelector("input[data-qa='signup-name']");
    private readonly By _signupEmail = By.CssSelector("input[data-qa='signup-email']");
    private readonly By _signupButton = By.CssSelector("button[data-qa='signup-button']");
    private readonly By _signupError = By.CssSelector("form[action='/signup'] p");

    public LoginPage(IWebDriver driver) : base(driver) { }

    public void Open() => OpenPath("/login");

    public bool IsLoginFormDisplayed() => WaitForVisible(_loginEmail).Displayed;

    public void Login(string email, string password)
    {
        EnterText(_loginEmail, email);
        EnterText(_loginPassword, password);
        Click(_loginButton);
    }

    public string GetLoginError() => GetText(_loginError);

    public void StartSignup(string name, string email)
    {
        EnterText(_signupName, name);
        EnterText(_signupEmail, email);
        Click(_signupButton);
    }

    public string GetSignupError() => GetText(_signupError);
}
