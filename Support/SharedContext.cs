using AutomationExercise.Tests.Models;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Support;

/// <summary>
/// Data shared by hooks and step classes during ONE scenario (browser + test user).
///
/// Reqnroll creates a new SharedContext for every scenario and passes the same object to every
/// class that asks for it in its constructor ("context injection").
/// (Named SharedContext so it does not clash with NUnit's own TestContext class.)
/// </summary>
public class SharedContext
{
    private IWebDriver? _driver;
    private User? _user;

    public IWebDriver Driver
    {
        get => _driver ?? throw new InvalidOperationException("The browser has not been started. Check Hooks.");
        set => _driver = value;
    }

    public bool HasDriver => _driver is not null;

    /// <summary>Setting the user also marks it for deletion after the scenario.</summary>
    public User User
    {
        get => _user ?? throw new InvalidOperationException(
            "No test user yet. Add the step 'Given a registered user exists' first.");
        set
        {
            _user = value;
            UserNeedsCleanup = true;
        }
    }

    public bool UserNeedsCleanup { get; private set; }
}
