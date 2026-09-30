@registration
Feature: New user registration
  As a new visitor
  I want to create an account
  So that I can place orders

  Background:
    Given I am on the login page

  @smoke
  Scenario: Register a new user with valid details
    When I sign up with a new name and a unique email
    And I fill in the account information form
    Then I should see the account created message
    When I continue to the home page
    Then I should see that I am logged in

  @regression @negative
  Scenario: Signup is rejected for an email that is already registered
    Given a registered user exists
    When I sign up with the registered user's email
    Then I should see the signup error "Email Address already exist!"
