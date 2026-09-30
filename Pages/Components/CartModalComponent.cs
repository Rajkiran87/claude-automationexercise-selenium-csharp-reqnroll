using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages.Components;

/// <summary>The "Added!" pop-up shown after clicking "Add to cart".</summary>
public class CartModalComponent : BasePage
{
    private readonly By _modal = By.Id("cartModal");
    private readonly By _continueShoppingButton = By.CssSelector("#cartModal button.close-modal");

    public CartModalComponent(IWebDriver driver) : base(driver) { }

    /// <summary>Waits for the pop-up, closes it and waits until it is gone.</summary>
    public void ContinueShopping()
    {
        WaitForVisible(_modal);
        Click(_continueShoppingButton);
        WaitForInvisible(_modal);
    }
}
