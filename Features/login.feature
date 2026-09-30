@login
Feature: User login
  As a registered customer of Automation Exercise
  I want to log in and log out of my account
  So that I can shop securely

  Background:
    Given I am on the login page

  @smoke
  Scenario: Login with valid credentials
    Given a registered user exists
    When I log in with the registered user's credentials
    Then I should see that I am logged in

  @regression @negative
  Scenario: Login with a correct email but a wrong password
    Given a registered user exists
    When I log in with the registered user's email and a wrong password
    Then I should see the login error "Your email or password is incorrect!"

  @regression @negative
  Scenario Outline: Login with an email that is not registered
    When I log in with email "<email>" and password "<password>"
    Then I should see the login error "Your email or password is incorrect!"

    Examples:
      | email                          | password   |
      | not.registered.user@example.com | Test@12345 |
      | NOT.REGISTERED@EXAMPLE.COM      | wrongpass  |

  @regression
  Scenario: Logged-in user can log out
    Given a registered user exists
    And I log in with the registered user's credentials
    When I log out
    Then I should be on the login page
