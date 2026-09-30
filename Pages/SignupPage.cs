using AutomationExercise.Tests.Models;
using OpenQA.Selenium;

namespace AutomationExercise.Tests.Pages;

/// <summary>"Enter Account Information" page shown after the first signup step.</summary>
public class SignupPage : BasePage
{
    private readonly By _titleMr = By.Id("id_gender1");
    private readonly By _titleMrs = By.Id("id_gender2");
    private readonly By _password = By.CssSelector("input[data-qa='password']");
    private readonly By _birthDay = By.CssSelector("select[data-qa='days']");
    private readonly By _birthMonth = By.CssSelector("select[data-qa='months']");
    private readonly By _birthYear = By.CssSelector("select[data-qa='years']");
    private readonly By _firstName = By.CssSelector("input[data-qa='first_name']");
    private readonly By _lastName = By.CssSelector("input[data-qa='last_name']");
    private readonly By _company = By.CssSelector("input[data-qa='company']");
    private readonly By _address1 = By.CssSelector("input[data-qa='address']");
    private readonly By _address2 = By.CssSelector("input[data-qa='address2']");
    private readonly By _country = By.CssSelector("select[data-qa='country']");
    private readonly By _state = By.CssSelector("input[data-qa='state']");
    private readonly By _city = By.CssSelector("input[data-qa='city']");
    private readonly By _zipcode = By.CssSelector("input[data-qa='zipcode']");
    private readonly By _mobileNumber = By.CssSelector("input[data-qa='mobile_number']");
    private readonly By _createAccountButton = By.CssSelector("button[data-qa='create-account']");

    public SignupPage(IWebDriver driver) : base(driver) { }

    public void FillAccountInformation(User user)
    {
        Click(user.Title == "Mrs" ? _titleMrs : _titleMr);
        EnterText(_password, user.Password);
        SelectByValue(_birthDay, user.BirthDay);
        SelectByValue(_birthMonth, user.BirthMonth);
        SelectByValue(_birthYear, user.BirthYear);
        EnterText(_firstName, user.FirstName);
        EnterText(_lastName, user.LastName);
        EnterText(_company, user.Company);
        EnterText(_address1, user.Address1);
        EnterText(_address2, user.Address2);
        SelectByText(_country, user.Country);
        EnterText(_state, user.State);
        EnterText(_city, user.City);
        EnterText(_zipcode, user.Zipcode);
        EnterText(_mobileNumber, user.MobileNumber);
    }

    public void CreateAccount() => Click(_createAccountButton);
}
