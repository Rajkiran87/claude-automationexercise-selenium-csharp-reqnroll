namespace AutomationExercise.Tests.Models;

/// <summary>
/// All the details needed to register a user.
/// A "record" is a short way to declare a class that only holds data.
/// </summary>
public record User(
    string Name,
    string Email,
    string Password,
    string Title,        // "Mr" or "Mrs"
    string BirthDay,     // "1" to "31"
    string BirthMonth,   // "1" to "12"
    string BirthYear,    // e.g. "1995"
    string FirstName,
    string LastName,
    string Company,
    string Address1,
    string Address2,
    string Country,      // must match an option in the site's dropdown
    string State,
    string City,
    string Zipcode,
    string MobileNumber);
