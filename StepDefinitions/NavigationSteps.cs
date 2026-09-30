using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Support;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

/// <summary>Steps that simply open a page (used by most Backgrounds).</summary>
[Binding]
public class NavigationSteps
{
    private readonly SharedContext _context;

    public NavigationSteps(SharedContext context) => _context = context;

    [Given("I am on the home page")]
    public void GivenIAmOnTheHomePage() => new HomePage(_context.Driver).Open();

    [Given("I am on the login page")]
    public void GivenIAmOnTheLoginPage() => new LoginPage(_context.Driver).Open();

    [Given("I am on the products page")]
    public void GivenIAmOnTheProductsPage() => new ProductsPage(_context.Driver).Open();
}
