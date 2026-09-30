using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

/// <summary>"All Products" page with the search box and product grid.</summary>
public class ProductsPage : BasePage
{
    private readonly By _searchInput = By.Id("search_product");
    private readonly By _searchButton = By.Id("submit_search");
    private readonly By _sectionTitle = By.CssSelector(".features_items h2.title");
    private readonly By _productNames = By.CssSelector(".features_items .productinfo p");

    public ProductsPage(IWebDriver driver) : base(driver) { }

    public void Open() => OpenPath("/products", _searchInput);

    public void Search(string searchTerm)
    {
        EnterText(_searchInput, searchTerm);
        Click(_searchButton);
        // The search reloads /products with "?search=..." in the URL. Wait for it, so the next step
        // does not read the old "All Products" page.
        Wait.Until(d => d.Url.Contains("search=", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Title above the grid, e.g. "ALL PRODUCTS" or "SEARCHED PRODUCTS".</summary>
    public string GetSectionTitle() => GetText(_sectionTitle);

    public List<string> GetProductNames() => GetAllTexts(_productNames);

    public void AddToCart(string productName) =>
        Click(By.XPath(ProductCardXpath(productName) + "//a[contains(@class, 'add-to-cart')]"));

    public void OpenProductDetails(string productName) =>
        Click(By.XPath(ProductCardXpath(productName) + "//a[contains(@href, '/product_details/')]"));

    /// <summary>Dynamic locator: the product card whose name is exactly productName.</summary>
    private static string ProductCardXpath(string productName) =>
        "//div[contains(@class, 'product-image-wrapper')]" +
        $"[.//div[contains(@class, 'productinfo')]/p[normalize-space()='{productName}']]";
}
