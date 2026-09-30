using AutomationExercise.Tests.Pages;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class RegistrationSteps
{
    private readonly SharedContext _context;

    public RegistrationSteps(SharedContext context) => _context = context;

    [When("I sign up with a new name and a unique email")]
    public void WhenISignUpWithANewNameAndAUniqueEmail()
    {
        var newUser = TestDataFactory.NewUser();
        // Saved now, so the AfterScenario hook deletes the account even if a later step fails
        _context.User = newUser;
        new LoginPage(_context.Driver).StartSignup(newUser.Name, newUser.Email);
    }

    [When("I sign up with the registered user's email")]
    public void WhenISignUpWithTheRegisteredUsersEmail() =>
        new LoginPage(_context.Driver).StartSignup(_context.User.Name, _context.User.Email);

    [When("I fill in the account information form")]
    public void WhenIFillInTheAccountInformationForm()
    {
        var signupPage = new SignupPage(_context.Driver);
        signupPage.FillAccountInformation(_context.User);
        signupPage.CreateAccount();
    }

    [Then("I should see the account created message")]
    public void ThenIShouldSeeTheAccountCreatedMessage() =>
        // The heading is shown in capitals with CSS, so ignore case
        Assert.That(new AccountCreatedPage(_context.Driver).GetHeading(),
            Is.EqualTo("Account Created!").IgnoreCase);

    [When("I continue to the home page")]
    public void WhenIContinueToTheHomePage() => new AccountCreatedPage(_context.Driver).ClickContinue();

    [Then("I should see the signup error {string}")]
    public void ThenIShouldSeeTheSignupError(string expectedError) =>
        Assert.That(new LoginPage(_context.Driver).GetSignupError(), Is.EqualTo(expectedError));
}
