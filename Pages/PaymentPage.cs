using AutomationExercise.Tests.Models;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

/// <summary>Payment form and the "Order Placed!" confirmation that follows it.</summary>
public class PaymentPage : BasePage
{
    private readonly By _nameOnCard = By.CssSelector("input[data-qa='name-on-card']");
    private readonly By _cardNumber = By.CssSelector("input[data-qa='card-number']");
    private readonly By _cvc = By.CssSelector("input[data-qa='cvc']");
    private readonly By _expiryMonth = By.CssSelector("input[data-qa='expiry-month']");
    private readonly By _expiryYear = By.CssSelector("input[data-qa='expiry-year']");
    private readonly By _payButton = By.CssSelector("button[data-qa='pay-button']");
    private readonly By _orderPlacedHeading = By.CssSelector("h2[data-qa='order-placed']");

    public PaymentPage(IWebDriver driver) : base(driver) { }

    public void PayWith(PaymentCard card)
    {
        EnterText(_nameOnCard, card.NameOnCard);
        EnterText(_cardNumber, card.CardNumber);
        EnterText(_cvc, card.Cvc);
        EnterText(_expiryMonth, card.ExpiryMonth);
        EnterText(_expiryYear, card.ExpiryYear);
        Click(_payButton);
    }

    public string GetOrderPlacedHeading() => GetText(_orderPlacedHeading);
}
