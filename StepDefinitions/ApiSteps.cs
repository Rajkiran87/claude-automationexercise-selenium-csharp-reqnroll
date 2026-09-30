using System.Text.Json;
using AutomationExercise.Tests.Support;
using NUnit.Framework;
using Reqnroll;

namespace AutomationExercise.Tests.StepDefinitions;

/// <summary>
/// Steps for the @api scenarios. The site always answers with HTTP 200 and puts the
/// real status in the "responseCode" field of the JSON body, so that is what we check.
/// </summary>
[Binding]
public class ApiSteps
{
    private readonly SharedContext _context;
    private JsonElement _response;

    public ApiSteps(SharedContext context) => _context = context;

    [When("I send a {word} request to {string}")]
    public Task WhenISendARequestTo(string method, string path) =>
        Send(new HttpMethod(method), path);

    [When("I search the API for products matching {string}")]
    public Task WhenISearchTheApiForProductsMatching(string searchTerm) =>
        Send(HttpMethod.Post, "/api/searchProduct", new() { ["search_product"] = searchTerm });

    [When("I verify the login of the registered user through the API")]
    public Task WhenIVerifyTheLoginOfTheRegisteredUser() =>
        WhenIVerifyTheLoginFor(_context.User.Email, _context.User.Password);

    [When("I verify the login for email {string} and password {string}")]
    public Task WhenIVerifyTheLoginFor(string email, string password) =>
        Send(HttpMethod.Post, "/api/verifyLogin", new() { ["email"] = email, ["password"] = password });

    [When("I request the account details for the registered user's email")]
    public Task WhenIRequestTheAccountDetailsForTheRegisteredUser() =>
        WhenIRequestTheAccountDetailsFor(_context.User.Email);

    [When("I request the account details for email {string}")]
    public Task WhenIRequestTheAccountDetailsFor(string email) =>
        Send(HttpMethod.Get, "/api/getUserDetailByEmail?email=" + Uri.EscapeDataString(email));

    [Then("the response code should be {int}")]
    public void ThenTheResponseCodeShouldBe(int expected) =>
        Assert.That(_response.GetProperty("responseCode").GetInt32(), Is.EqualTo(expected),
            $"Response body: {_response}");

    [Then("the response message should be {string}")]
    public void ThenTheResponseMessageShouldBe(string expected) =>
        Assert.That(_response.GetProperty("message").GetString(), Is.EqualTo(expected));

    [Then("the response should contain a non-empty {string} list")]
    public void ThenTheResponseShouldContainANonEmptyList(string property) =>
        Assert.That(_response.GetProperty(property).GetArrayLength(), Is.GreaterThan(0));

    [Then("every product name in the response should contain {string}")]
    public void ThenEveryProductNameShouldContain(string text)
    {
        var names = _response.GetProperty("products").EnumerateArray()
            .Select(product => product.GetProperty("name").GetString() ?? string.Empty)
            .ToList();

        Assert.That(names, Is.Not.Empty, "The search returned no products");
        Assert.That(names, Has.All.Matches<string>(name => name.Contains(text, StringComparison.OrdinalIgnoreCase)));
    }

    [Then("the returned user's email should match the registered user")]
    public void ThenTheReturnedUsersEmailShouldMatch() =>
        Assert.That(_response.GetProperty("user").GetProperty("email").GetString(),
            Is.EqualTo(_context.User.Email));

    private async Task Send(HttpMethod method, string path, Dictionary<string, string>? form = null)
    {
        using var json = await ApiClient.RequestAsync(method, path, form);
        _response = json.RootElement.Clone();   // Clone keeps the data after the document is disposed
    }
}
