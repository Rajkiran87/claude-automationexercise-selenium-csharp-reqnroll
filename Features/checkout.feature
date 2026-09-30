@checkout @e2e
Feature: Checkout
  As a logged-in customer
  I want to pay for the products in my cart
  So that my order is placed

  @smoke
  Scenario: Logged-in user places an order for one product
    Given a registered user exists
    And I am on the login page
    And I log in with the registered user's credentials
    And I am on the products page
    And I add the product "Blue Top" to the cart
    And I open the cart
    When I proceed to checkout
    And I place the order
    And I pay with the test card details
    Then I should see the order placed confirmation
