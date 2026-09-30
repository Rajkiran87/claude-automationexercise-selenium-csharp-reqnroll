using AutomationExercise.Tests.Support;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AutomationExercise.Tests.Drivers;

/// <summary>
/// Creates the browser named in appsettings.json.
/// Selenium Manager (built into Selenium 4) downloads the right driver automatically.
/// </summary>
public static class DriverFactory
{
    /// <summary>
    /// The site shows Google ads that can cover buttons. For Chrome/Edge we point the ad
    /// domains at 127.0.0.1 so they never load.
    /// </summary>
    private const string BlockAds =
        "--host-resolver-rules=" +
        "MAP *.doubleclick.net 127.0.0.1, " +
        "MAP *.googlesyndication.com 127.0.0.1, " +
        "MAP *.googleadservices.com 127.0.0.1, " +
        "MAP adservice.google.com 127.0.0.1, " +
        "MAP fundingchoicesmessages.google.com 127.0.0.1";

    private static readonly string[] CommonChromiumArgs =
    {
        "--window-size=1920,1080",
        "--disable-notifications",
        BlockAds
    };

    public static IWebDriver Create()
    {
        var headless = ConfigReader.Headless;

        IWebDriver driver = ConfigReader.Browser switch
        {
            "chrome" => new ChromeDriver(BuildChromeOptions(headless)),
            "edge" => new EdgeDriver(BuildEdgeOptions(headless)),
            "firefox" => new FirefoxDriver(BuildFirefoxOptions(headless)),
            var other => throw new ArgumentException($"Unsupported browser '{other}'. Use chrome, firefox or edge.")
        };

        if (!headless)
        {
            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    private static ChromeOptions BuildChromeOptions(bool headless)
    {
        var options = new ChromeOptions();
        options.AddArguments(CommonChromiumArgs);
        if (headless) options.AddArgument("--headless=new");
        return options;
    }

    private static EdgeOptions BuildEdgeOptions(bool headless)
    {
        var options = new EdgeOptions();
        options.AddArguments(CommonChromiumArgs);
        if (headless) options.AddArgument("--headless=new");
        return options;
    }

    private static FirefoxOptions BuildFirefoxOptions(bool headless)
    {
        var options = new FirefoxOptions();
        options.AddArguments("--width=1920", "--height=1080");
        if (headless) options.AddArgument("-headless");
        return options;
    }
}
