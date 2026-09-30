using System.Text.Json;
using AutomationExercise.Tests.Models;

namespace AutomationExercise.Tests.Support;

/// <summary>
/// Calls the Automation Exercise public API (https://automationexercise.com/api_list).
///
/// Why use the API in UI tests?
///  - Creating a user through the API is much faster than filling the signup form.
///  - Deleting the user afterwards keeps the site clean.
///
/// Note: this API always returns HTTP 200. The real result is "responseCode" inside the JSON body.
/// </summary>
public static class ApiClient
{
    // One HttpClient for the whole run is the recommended .NET practice
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(30) };

    public static async Task CreateAccountAsync(User user)
    {
        var form = new Dictionary<string, string>
        {
            ["name"] = user.Name,
            ["email"] = user.Email,
            ["password"] = user.Password,
            ["title"] = user.Title,
            ["birth_date"] = user.BirthDay,
            ["birth_month"] = user.BirthMonth,
            ["birth_year"] = user.BirthYear,
            ["firstname"] = user.FirstName,
            ["lastname"] = user.LastName,
            ["company"] = user.Company,
            ["address1"] = user.Address1,
            ["address2"] = user.Address2,
            ["country"] = user.Country,
            ["zipcode"] = user.Zipcode,
            ["state"] = user.State,
            ["city"] = user.City,
            ["mobile_number"] = user.MobileNumber
        };

        var body = await SendAsync(HttpMethod.Post, "/api/createAccount", form);
        if (ReadResponseCode(body) != 201)
        {
            throw new InvalidOperationException($"Could not create test user via API. Response: {body}");
        }
    }

    /// <summary>Returns true when the API confirms the account was deleted.</summary>
    public static async Task<bool> DeleteAccountAsync(string email, string password)
    {
        var form = new Dictionary<string, string>
        {
            ["email"] = email,
            ["password"] = password
        };

        var body = await SendAsync(HttpMethod.Delete, "/api/deleteAccount", form);
        return ReadResponseCode(body) == 200;
    }

    /// <summary>Sends any request and parses the JSON body. Used by the @api scenarios.</summary>
    public static async Task<JsonDocument> RequestAsync(HttpMethod method, string path, Dictionary<string, string>? form = null)
    {
        using var request = new HttpRequestMessage(method, ConfigReader.BaseUrl + path);
        if (form is not null) request.Content = new FormUrlEncodedContent(form);

        using var response = await Http.SendAsync(request);
        return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
    }

    private static async Task<string> SendAsync(HttpMethod method, string path, Dictionary<string, string> form)
    {
        using var request = new HttpRequestMessage(method, ConfigReader.BaseUrl + path)
        {
            Content = new FormUrlEncodedContent(form)
        };
        using var response = await Http.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }

    private static int ReadResponseCode(string body)
    {
        try
        {
            using var json = JsonDocument.Parse(body);
            return json.RootElement.GetProperty("responseCode").GetInt32();
        }
        catch (Exception)
        {
            return -1; // body was not the expected JSON
        }
    }
}
