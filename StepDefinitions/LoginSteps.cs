using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Pages.Components;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly SharedContext _context;

    public LoginSteps(SharedContext context) => _context = context;

    private LoginPage LoginPage => new(_context.Driver);
    private HeaderComponent Header => new(_context.Driver);

    [Given("a registered user exists")]
    public async Task GivenARegisteredUserExists()
    {
        var user = TestDataFactory.NewUser();
        await ApiClient.CreateAccountAsync(user);   // fast set-up through the API instead of the UI
        _context.User = user;                       // remembered for later steps, deleted after the scenario
    }

    // In Reqnroll, "And" after "Given" counts as a Given step.
    // This step is used both ways in the features, so it has both attributes.
    [Given("I log in with the registered user's credentials")]
    [When("I log in with the registered user's credentials")]
    public void WhenILogInWithTheRegisteredUsersCredentials()
    {
        LoginPage.Login(_context.User.Email, _context.User.Password);

        // Wait for the login to finish before the next step. Otherwise the site's redirect after
        // login can arrive late and replace the next page we open (e.g. /products -> home page).
        Header.GetLoggedInUsername();
    }

    [When("I log in with the registered user's email and a wrong password")]
    public void WhenILogInWithTheRegisteredUsersEmailAndAWrongPassword() =>
        LoginPage.Login(_context.User.Email, "Wrong@Password1");

    [When("I log in with email {string} and password {string}")]
    public void WhenILogInWithEmailAndPassword(string email, string password) =>
        LoginPage.Login(email, password);

    [When("I log out")]
    public void WhenILogOut() => Header.Logout();

    [Then("I should see that I am logged in")]
    public void ThenIShouldSeeThatIAmLoggedIn() =>
        Assert.That(Header.GetLoggedInUsername(), Is.EqualTo(_context.User.Name),
            "The header should show the logged-in user's name");

    [Then("I should see the login error {string}")]
    public void ThenIShouldSeeTheLoginError(string expectedError) =>
        Assert.That(LoginPage.GetLoginError(), Is.EqualTo(expectedError));

    [Then("I should be on the login page")]
    public void ThenIShouldBeOnTheLoginPage()
    {
        var loginPage = LoginPage;
        Assert.That(loginPage.IsLoginFormDisplayed(), Is.True, "The login form should be visible");
        Assert.That(loginPage.CurrentUrl, Does.EndWith("/login"));
    }
}
