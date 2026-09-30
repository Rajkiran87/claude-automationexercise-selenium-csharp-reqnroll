using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class SearchSteps
{
    private readonly SharedContext _context;

    public SearchSteps(SharedContext context) => _context = context;

    private ProductsPage ProductsPage => new(_context.Driver);

    [When("I search for {string}")]
    public void WhenISearchFor(string searchTerm) => ProductsPage.Search(searchTerm);

    [Then("the searched products section should be displayed")]
    public void ThenTheSearchedProductsSectionShouldBeDisplayed() =>
        Assert.That(ProductsPage.GetSectionTitle(), Is.EqualTo("Searched Products").IgnoreCase);

    [Then("every product in the results should contain {string}")]
    public void ThenEveryProductInTheResultsShouldContain(string expectedText)
    {
        var names = ProductsPage.GetProductNames();

        Assert.That(names, Is.Not.Empty, "The search returned no products");
        foreach (var name in names)
        {
            // Case-insensitive: "jeans" matches "Soft Stretch Jeans"
            Assert.That(name, Does.Contain(expectedText).IgnoreCase,
                $"Product '{name}' does not contain '{expectedText}'");
        }
    }

    [Then("no products should be displayed")]
    public void ThenNoProductsShouldBeDisplayed() =>
        Assert.That(ProductsPage.GetProductNames(), Is.Empty);
}
