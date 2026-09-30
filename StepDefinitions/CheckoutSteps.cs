using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class CheckoutSteps
{
    private readonly SharedContext _context;

    public CheckoutSteps(SharedContext context) => _context = context;

    [When("I proceed to checkout")]
    public void WhenIProceedToCheckout() => new CartPage(_context.Driver).ProceedToCheckout();

    [When("I place the order")]
    public void WhenIPlaceTheOrder() => new CheckoutPage(_context.Driver).PlaceOrder();

    [When("I pay with the test card details")]
    public void WhenIPayWithTheTestCardDetails() =>
        new PaymentPage(_context.Driver).PayWith(TestDataFactory.TestCard());

    [Then("I should see the order placed confirmation")]
    public void ThenIShouldSeeTheOrderPlacedConfirmation() =>
        Assert.That(new PaymentPage(_context.Driver).GetOrderPlacedHeading(),
            Is.EqualTo("Order Placed!").IgnoreCase);
}
