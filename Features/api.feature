@api
Feature: Public API
  As a client of the Automation Exercise API
  I want correct data and clear errors
  So that I can rely on the service without the website

  # These scenarios call the API directly. No browser is opened (see ScenarioHooks).

  @smoke
  Scenario: Get the list of all products
    When I send a GET request to "/api/productsList"
    Then the response code should be 200
    And the response should contain a non-empty "products" list

  @smoke
  Scenario: Get the list of all brands
    When I send a GET request to "/api/brandsList"
    Then the response code should be 200
    And the response should contain a non-empty "brands" list

  @regression @negative
  Scenario Outline: Unsupported request methods are rejected
    When I send a <method> request to "<endpoint>"
    Then the response code should be 405
    And the response message should be "This request method is not supported."

    Examples:
      | method | endpoint          |
      | POST   | /api/productsList |
      | PUT    | /api/brandsList   |
      | DELETE | /api/verifyLogin  |

  @regression
  Scenario: Search products by name
    When I search the API for products matching "jean"
    Then the response code should be 200
    And every product name in the response should contain "jean"

  @regression @negative
  Scenario: Search without the search_product parameter
    When I send a POST request to "/api/searchProduct"
    Then the response code should be 400
    And the response message should be "Bad request, search_product parameter is missing in POST request."

  @smoke
  Scenario: Verify login with valid credentials
    Given a registered user exists
    When I verify the login of the registered user through the API
    Then the response code should be 200
    And the response message should be "User exists!"

  @regression @negative
  Scenario: Verify login with an unknown user
    When I verify the login for email "not.registered.user@example.com" and password "Test@12345"
    Then the response code should be 404
    And the response message should be "User not found!"

  @regression
  Scenario: Get account details by email
    Given a registered user exists
    When I request the account details for the registered user's email
    Then the response code should be 200
    And the returned user's email should match the registered user

  @regression @negative
  Scenario: Get account details for an email that is not registered
    When I request the account details for email "not.registered.user@example.com"
    Then the response code should be 404
    And the response message should be "Account not found with this email, try another email!"
