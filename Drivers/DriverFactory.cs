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
    /// The site shows Google ads that can cover buttons or replace the page.
    /// For Chrome/Edge we point the ad domains at 127.0.0.1 so they never load.
    /// </summary>
    private const string BlockAds =
        "--host-resolver-rules=" +
        "MAP *.doubleclick.net 127.0.0.1, " +
        "MAP *.googlesyndication.com 127.0.0.1, " +
        "MAP *.googleadservices.com 127.0.0.1, " +
        "MAP *.adtrafficquality.google 127.0.0.1, " +
        "MAP adservice.google.com 127.0.0.1, " +
        "MAP fundingchoicesmessages.google.com 127.0.0.1";

    /// <summary>
    /// Firefox has no wildcard rule like Chrome, so we list the ad hosts the site uses.
    /// The "network.dns.localDomains" setting makes Firefox resolve them to localhost.
    /// </summary>
    private static readonly string[] FirefoxAdHosts =
    {
        "pagead2.googlesyndication.com",
        "tpc.googlesyndication.com",
        "googleads.g.doubleclick.net",
        "securepubads.g.doubleclick.net",
        "static.doubleclick.net",
        "partner.googleadservices.com",
        "www.googletagservices.com",
        "adservice.google.com",
        "fundingchoicesmessages.google.com",
        "ep1.adtrafficquality.google",
        "ep2.adtrafficquality.google"
    };

    private static readonly string[] CommonChromiumArgs =
    {
        "--window-size=1920,1080",
        "--disable-notifications",
        BlockAds
    };

    /// <summary>
    /// Extra Chromium flags needed on Linux CI runners (GitHub Actions sets CI=true).
    /// Ubuntu 24.04 runners block the browser sandbox, which makes Edge crash on start with
    /// "session not created: Chrome instance exited". The runner is a throwaway machine,
    /// so turning the sandbox off there is safe. Local runs keep the sandbox on.
    /// </summary>
    private static readonly string[] CiChromiumArgs =
    {
        "--no-sandbox",
        "--disable-dev-shm-usage"
    };

    private static bool RunningInCi =>
        string.Equals(Environment.GetEnvironmentVariable("CI"), "true", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// How long Selenium waits for the browser to answer a command, including starting up.
    /// The default is 60 seconds; a busy CI machine starting several browsers at once can need more.
    /// </summary>
    private static readonly TimeSpan CommandTimeout = TimeSpan.FromSeconds(120);

    /// <summary>
    /// Maximum time for one page load. It must be shorter than CommandTimeout, so a slow page
    /// ends with a normal timeout (handled in BasePage.OpenPath) instead of hanging the browser.
    /// </summary>
    private static readonly TimeSpan PageLoadTimeout = TimeSpan.FromSeconds(60);

    public static IWebDriver Create()
    {
        var headless = ConfigReader.Headless;

        IWebDriver driver = ConfigReader.Browser switch
        {
            "chrome" => new ChromeDriver(ChromeDriverService.CreateDefaultService(), BuildChromeOptions(headless), CommandTimeout),
            "edge" => new EdgeDriver(EdgeDriverService.CreateDefaultService(), BuildEdgeOptions(headless), CommandTimeout),
            "firefox" => new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), BuildFirefoxOptions(headless), CommandTimeout),
            var other => throw new ArgumentException($"Unsupported browser '{other}'. Use chrome, firefox or edge.")
        };

        driver.Manage().Timeouts().PageLoad = PageLoadTimeout;

        if (!headless)
        {
            driver.Manage().Window.Maximize();
        }

        return driver;
    }

    // PageLoadStrategy.Eager: continue as soon as the HTML is ready instead of waiting for every
    // ad, font and tracking script. Page objects already wait for the elements they need.
    private static ChromeOptions BuildChromeOptions(bool headless)
    {
        var options = new ChromeOptions { PageLoadStrategy = PageLoadStrategy.Eager };
        options.AddArguments(CommonChromiumArgs);
        if (RunningInCi) options.AddArguments(CiChromiumArgs);
        if (headless) options.AddArgument("--headless=new");
        return options;
    }

    private static EdgeOptions BuildEdgeOptions(bool headless)
    {
        var options = new EdgeOptions { PageLoadStrategy = PageLoadStrategy.Eager };
        options.AddArguments(CommonChromiumArgs);
        if (RunningInCi) options.AddArguments(CiChromiumArgs);
        if (headless) options.AddArgument("--headless=new");
        return options;
    }

    private static FirefoxOptions BuildFirefoxOptions(bool headless)
    {
        var options = new FirefoxOptions { PageLoadStrategy = PageLoadStrategy.Eager };
        options.AddArguments("--width=1920", "--height=1080");
        options.SetPreference("network.dns.localDomains", string.Join(",", FirefoxAdHosts));
        if (headless) options.AddArgument("-headless");
        return options;
    }
}
