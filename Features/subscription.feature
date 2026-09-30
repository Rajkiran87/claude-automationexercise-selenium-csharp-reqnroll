@subscription
Feature: Newsletter subscription
  As a visitor
  I want to subscribe to the newsletter from the footer
  So that I receive product updates

  Background:
    Given I am on the home page

  @smoke
  Scenario: Subscribe with a valid email
    When I subscribe with a unique email
    Then I should see the subscription success message

  @regression @negative
  Scenario Outline: Subscription is blocked for an invalid email
    When I subscribe with the email "<email>"
    Then the subscription email field should show a validation error

    Examples:
      | email          |
      | plainaddress   |
      | missing-at.com |
      | user@          |
