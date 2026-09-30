using AutomationExercise.Tests.Models;

namespace AutomationExercise.Tests.Support;

/// <summary>
/// Builds test data. Every user gets a unique email so runs never clash.
/// </summary>
public static class TestDataFactory
{
    public const string DefaultPassword = "Test@12345";

    /// <summary>Example: autotest_3f9a1c2b@example.com</summary>
    public static string UniqueEmail() => $"autotest_{Guid.NewGuid().ToString("N")[..8]}@example.com";

    public static User NewUser() => new(
        Name: "QA Tester",
        Email: UniqueEmail(),
        Password: DefaultPassword,
        Title: "Mr",
        BirthDay: "15",
        BirthMonth: "6",
        BirthYear: "1995",
        FirstName: "Qa",
        LastName: "Tester",
        Company: "Portfolio Testing Ltd",
        Address1: "221B Test Street",
        Address2: "Near Automation Park",
        Country: "India",
        State: "Karnataka",
        City: "Bengaluru",
        Zipcode: "560001",
        MobileNumber: "9876543210");

    public static PaymentCard TestCard() => new("QA Tester", "4111111111111111", "123", "12", "2030");
}
