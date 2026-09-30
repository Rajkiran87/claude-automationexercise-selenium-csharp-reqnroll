using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

public class ProductDetailsPage : BasePage
{
    private readonly By _productName = By.CssSelector(".product-information h2");
    private readonly By _quantityInput = By.Id("quantity");
    private readonly By _addToCartButton = By.CssSelector(".product-information button.cart");

    public ProductDetailsPage(IWebDriver driver) : base(driver) { }

    public string GetProductName() => GetText(_productName);

    public void AddToCart(int quantity)
    {
        EnterText(_quantityInput, quantity.ToString());
        Click(_addToCartButton);
    }
}
