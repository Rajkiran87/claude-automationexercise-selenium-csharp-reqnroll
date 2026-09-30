using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

/// <summary>Checkout page with address review and the "Place Order" button.</summary>
public class CheckoutPage : BasePage
{
    private readonly By _placeOrderButton = By.CssSelector("a[href='/payment']");

    public CheckoutPage(IWebDriver driver) : base(driver) { }

    public void PlaceOrder() => Click(_placeOrderButton);
}
