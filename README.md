# Automation Exercise – Selenium + C# + Reqnroll (BDD)

UI test automation framework for the practice e-commerce site **[automationexercise.com](https://automationexercise.com)**, written in **C# / .NET 8** with **Selenium WebDriver 4**, **Reqnroll** (the open-source successor of SpecFlow) and **NUnit**.

It is one of four portfolio projects that automate **the same Gherkin UI scenarios** with different tool stacks (this one also adds API tests):

| Project | Stack |
|---|---|
| selenium-java-cucumber | Java · Selenium · Cucumber · JUnit 5 |
| **selenium-csharp-reqnroll** (this repo) | C# · Selenium · Reqnroll · NUnit |
| playwright-java-cucumber | Java · Playwright · Cucumber · JUnit 5 |
| playwright-typescript-bdd | TypeScript · Playwright Test · playwright-bdd |

---

## What is tested

| Feature | Scenarios | Tags |
|---|---|---|
| Login | valid login, wrong password, unregistered email (2 data rows), logout | `@login` |
| Registration | register a new user, duplicate email is rejected | `@registration` |
| Product search | matching results, no results | `@search` |
| Shopping cart | add one product, add several (data table), set quantity on details page, remove product | `@cart` |
| Checkout (end-to-end) | logged-in user pays and sees *Order Placed!* | `@checkout @e2e` |
| Newsletter | valid subscription, invalid emails blocked (3 data rows) | `@subscription` |
| Public API (no browser) | products and brands lists, unsupported methods (3 data rows), search, verify login, account details | `@api` |

Each scenario also has `@smoke` or `@regression`. Negative tests carry `@negative`.

---

## Tech stack

| Tool | Why |
|---|---|
| .NET 8 (C# 12) | Language and runtime |
| Selenium 4 | Browser automation. **Selenium Manager** downloads the driver automatically |
| Reqnroll | Runs Gherkin `.feature` files as NUnit tests (BDD) |
| NUnit 4 | Test runner and `Assert.That` assertions |
| GitHub Actions | Runs the suite on every push and nightly, in Chrome, Firefox and Edge |

---

## Project structure

```
selenium-csharp-reqnroll
├── AutomationExercise.Tests.csproj      # NuGet packages and versions
├── appsettings.json                     # base URL, browser, headless, timeout
├── reqnroll.json                        # Reqnroll settings
├── Features/*.feature                   # the BDD scenarios
├── StepDefinitions/                     # Gherkin step  ->  C# method
│   ├── ApiSteps.cs
│   ├── NavigationSteps.cs
│   ├── LoginSteps.cs
│   ├── RegistrationSteps.cs
│   ├── SearchSteps.cs
│   ├── CartSteps.cs
│   ├── CheckoutSteps.cs
│   └── SubscriptionSteps.cs
├── Hooks/ScenarioHooks.cs               # open browser / screenshot + close + cleanup
├── Pages/                               # Page Object Model
│   ├── BasePage.cs                      # waits + common actions
│   ├── Components/                      # Header, Footer, CartModal (shared page parts)
│   └── LoginPage.cs, SignupPage.cs, ProductsPage.cs, CartPage.cs, PaymentPage.cs ...
├── Drivers/DriverFactory.cs             # creates Chrome / Firefox / Edge
├── Support/
│   ├── SharedContext.cs                 # data shared inside one scenario (driver, user)
│   ├── ConfigReader.cs                  # reads appsettings.json (+ environment overrides)
│   ├── ApiClient.cs                     # calls the site's API (test users + @api scenarios)
│   ├── ParallelConfig.cs                # runs feature files in parallel
│   └── TestDataFactory.cs               # unique emails, test user, test card
├── Models/                              # User, PaymentCard records
└── .github/workflows/tests.yml          # CI pipeline
```

### How a test flows

```
login.feature  ──►  LoginSteps.cs  ──►  LoginPage.cs  ──►  Selenium  ──►  Browser
  (what)             (glue)              (how)
```

---

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download) or newer (`dotnet --version`)
* Google Chrome (or Firefox / Edge)
* Visual Studio 2022 or JetBrains Rider with the **Reqnroll** extension (for Gherkin highlighting and "Go to step")

## How to run

```bash
# all scenarios
dotnet test

# only smoke tests (Reqnroll turns tags into NUnit categories)
dotnet test --filter "Category=smoke"

# everything except the end-to-end checkout
dotnet test --filter "Category!=e2e"

# only the API scenarios (no browser, takes seconds)
dotnet test --filter "Category=api"

# fewer parallel workers (default is 4) if the site is slow
dotnet test -- NUnit.NumberOfTestWorkers=2

# with an HTML report in TestResults/
dotnet test --logger "html;LogFileName=test-report.html"
```

Change the browser or run headless with environment variables:

```bash
# macOS / Linux
BROWSER=firefox HEADLESS=true dotnet test
```

```powershell
# Windows PowerShell
$env:BROWSER="firefox"; $env:HEADLESS="true"; dotnet test
```

## Reports and screenshots

* `TestResults/test-report.html` – when run with `--logger html`
* Screenshots of failed scenarios: `bin/Debug/net8.0/Screenshots/` (also attached to the NUnit result)

---

## Design decisions

| Decision | Reason |
|---|---|
| Page Object Model + components | Locators live in one place; header/footer/cart pop-up are reused |
| Explicit waits only (`BasePage`) | No `Thread.Sleep`, fewer flaky failures |
| `data-qa` locators first | Added by the site for testing, rarely change |
| Context injection (`SharedContext`) | Step classes share state without static fields |
| Test users created and deleted through the **API** | Fast, independent scenarios with no leftover data |
| Unique email per run | Tests can run repeatedly and in any order |
| Ads blocked at DNS level (Chrome/Edge) | Google ads sometimes cover buttons on this site |
| `[Given]` + `[When]` on shared steps | In Reqnroll, `And` after `Given` is a *Given* step, so reusable steps accept both |
| Feature files run in parallel | Safe because each scenario has its own browser and data; the suite finishes much faster |
| `@api` scenarios skip the browser | API checks run in seconds and once in CI, not once per browser |
| CI browser matrix + nightly run | Catches browser-specific bugs and changes on the live site |

## Known limitations / ideas for next steps

* Automation Exercise is a shared public demo site. It can be slow, and ads can still appear in Firefox. Re-run before logging a defect.
* Add Allure (`Allure.Reqnroll`) or ExtentReports for richer dashboards.

---

**Author:** Rajkiran G R · [LinkedIn](https://www.linkedin.com/in/grkrajkiran)
