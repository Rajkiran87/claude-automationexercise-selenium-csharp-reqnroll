using AutomationExercise.Tests.Pages.Components;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

[Binding]
public class SubscriptionSteps
{
    private readonly SharedContext _context;

    public SubscriptionSteps(SharedContext context) => _context = context;

    private FooterComponent Footer => new(_context.Driver);

    [When("I subscribe with a unique email")]
    public void WhenISubscribeWithAUniqueEmail() => Footer.Subscribe(TestDataFactory.UniqueEmail());

    [When("I subscribe with the email {string}")]
    public void WhenISubscribeWithTheEmail(string email) => Footer.Subscribe(email);

    [Then("I should see the subscription success message")]
    public void ThenIShouldSeeTheSubscriptionSuccessMessage() =>
        Assert.That(Footer.GetSuccessMessage(), Is.EqualTo("You have been successfully subscribed!"));

    [Then("the subscription email field should show a validation error")]
    public void ThenTheSubscriptionEmailFieldShouldShowAValidationError() =>
        Assert.That(Footer.GetEmailValidationMessage(), Is.Not.Empty,
            "The browser should block an invalid email with a validation message");
}
