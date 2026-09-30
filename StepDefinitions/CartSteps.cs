using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Pages.Components;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class CartSteps
{
    private readonly SharedContext _context;

    public CartSteps(SharedContext context) => _context = context;

    private CartPage CartPage => new(_context.Driver);

    [Given("I add the product {string} to the cart")]
    [When("I add the product {string} to the cart")]
    public void WhenIAddTheProductToTheCart(string productName)
    {
        new ProductsPage(_context.Driver).AddToCart(productName);
        new CartModalComponent(_context.Driver).ContinueShopping();
    }

    /// <summary>
    /// The feature file passes a table with a "product" header:
    ///   | product    |
    ///   | Blue Top   |
    ///   | Men Tshirt |
    /// </summary>
    [When("I add these products to the cart:")]
    public void WhenIAddTheseProductsToTheCart(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            WhenIAddTheProductToTheCart(row["product"]);
        }
    }

    [When("I open the product details for {string}")]
    public void WhenIOpenTheProductDetailsFor(string productName)
    {
        new ProductsPage(_context.Driver).OpenProductDetails(productName);
        Assert.That(new ProductDetailsPage(_context.Driver).GetProductName(), Is.EqualTo(productName));
    }

    [When("I add it to the cart with quantity {int}")]
    public void WhenIAddItToTheCartWithQuantity(int quantity)
    {
        new ProductDetailsPage(_context.Driver).AddToCart(quantity);
        new CartModalComponent(_context.Driver).ContinueShopping();
    }

    [Given("I open the cart")]
    [When("I open the cart")]
    public void WhenIOpenTheCart() => new HeaderComponent(_context.Driver).OpenCart();

    [When("I remove {string} from the cart")]
    public void WhenIRemoveFromTheCart(string productName) => CartPage.RemoveProduct(productName);

    [Then("the cart should contain {string} with quantity {int}")]
    public void ThenTheCartShouldContainWithQuantity(string productName, int expectedQuantity)
    {
        var cartPage = CartPage;
        Assert.That(cartPage.ContainsProduct(productName), Is.True, $"{productName} is not in the cart");
        Assert.That(cartPage.GetQuantity(productName), Is.EqualTo(expectedQuantity),
            $"Wrong quantity for {productName}");
    }

    [Then("the cart should be empty")]
    public void ThenTheCartShouldBeEmpty() =>
        Assert.That(CartPage.IsEmptyMessageDisplayed(), Is.True, "The 'Cart is empty!' message is not shown");
}
