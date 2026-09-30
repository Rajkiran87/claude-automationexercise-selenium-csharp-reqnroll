using AutomationExercise.Tests.Drivers;
using AutomationExercise.Tests.Support;
using OpenQA.Selenium;
using Reqnroll;

namespace AutomationExercise.Tests.Hooks;

/// <summary>Code that runs automatically before and after EVERY scenario.</summary>
[Binding]
public sealed class ScenarioHooks
{
    private readonly SharedContext _context;
    private readonly ScenarioContext _scenarioContext;
    private readonly IReqnrollOutputHelper _output;

    public ScenarioHooks(SharedContext context, ScenarioContext scenarioContext, IReqnrollOutputHelper output)
    {
        _context = context;
        _scenarioContext = scenarioContext;
        _output = output;
    }

    [BeforeScenario]
    public void StartBrowser()
    {
        // API scenarios talk to the server directly and do not need a browser
        if (_scenarioContext.ScenarioInfo.CombinedTags.Contains("api")) return;

        _context.Driver = DriverFactory.Create();
    }

    [AfterScenario]
    public async Task TearDown()
    {
        try
        {
            if (_scenarioContext.TestError is not null && _context.HasDriver)
            {
                SaveScreenshot();
            }
        }
        finally
        {
            if (_context.HasDriver)
            {
                _context.Driver.Quit();
            }

            await DeleteTestUserAsync();
        }
    }

    private void SaveScreenshot()
    {
        var folder = Path.Combine(AppContext.BaseDirectory, "Screenshots");
        Directory.CreateDirectory(folder);

        // Turn "Login with valid credentials" into a safe file name
        var safeName = string.Concat(_scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
        var path = Path.Combine(folder, $"{safeName}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.png");

        ((ITakesScreenshot)_context.Driver).GetScreenshot().SaveAsFile(path);

        _output.AddAttachment(path);                              // shows in Reqnroll output
        global::NUnit.Framework.TestContext.AddTestAttachment(path);      // shows in the NUnit / HTML report
    }

    /// <summary>Removes any account this scenario created, so the site does not fill up with test users.</summary>
    private async Task DeleteTestUserAsync()
    {
        if (!_context.UserNeedsCleanup) return;

        var user = _context.User;
        try
        {
            if (!await ApiClient.DeleteAccountAsync(user.Email, user.Password))
            {
                Console.WriteLine($"Warning: the API did not delete test user {user.Email}");
            }
        }
        catch (Exception e)
        {
            // Cleanup problems should not hide the real test result
            Console.WriteLine($"Warning: could not delete test user {user.Email}: {e.Message}");
        }
    }
}
