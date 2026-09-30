using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages.Components;

/// <summary>The navigation bar at the top of every page.</summary>
public class HeaderComponent : BasePage
{
    private readonly By _cartLink = By.CssSelector(".shop-menu a[href='/view_cart']");
    private readonly By _logoutLink = By.CssSelector(".shop-menu a[href='/logout']");
    // Renders as: "Logged in as <b>QA Tester</b>"
    private readonly By _loggedInUsername = By.XPath("//a[contains(., 'Logged in as')]/b");

    public HeaderComponent(IWebDriver driver) : base(driver) { }

    public void OpenCart() => Click(_cartLink);

    public void Logout() => Click(_logoutLink);

    public string GetLoggedInUsername() => GetText(_loggedInUsername);
}
