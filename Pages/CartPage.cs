using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

public class CartPage : BasePage
{
    private readonly By _emptyCartMessage = By.Id("empty_cart");
    private readonly By _proceedToCheckoutButton = By.CssSelector("a.check_out");

    public CartPage(IWebDriver driver) : base(driver) { }

    public void Open() => OpenPath("/view_cart");

    /// <summary>True when the product's row appears within the normal timeout.</summary>
    public bool ContainsProduct(string productName)
    {
        try
        {
            WaitForVisible(By.XPath(RowXpath(productName)));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    /// <summary>Reads the number in the Quantity column for this product.</summary>
    public int GetQuantity(string productName) =>
        int.Parse(GetText(By.XPath(RowXpath(productName) + "//td[contains(@class, 'cart_quantity')]/button")));

    /// <summary>Clicks the X button and waits until the row disappears.</summary>
    public void RemoveProduct(string productName)
    {
        Click(By.XPath(RowXpath(productName) + "//a[contains(@class, 'cart_quantity_delete')]"));
        WaitForInvisible(By.XPath(RowXpath(productName)));
    }

    public bool IsEmptyMessageDisplayed() => WaitForVisible(_emptyCartMessage).Displayed;

    public void ProceedToCheckout() => Click(_proceedToCheckoutButton);

    /// <summary>Dynamic locator: the cart row whose product name is exactly productName.</summary>
    private static string RowXpath(string productName) =>
        "//table[@id='cart_info_table']//tbody/tr" +
        $"[.//td[contains(@class, 'cart_description')]//a[normalize-space()='{productName}']]";
}
