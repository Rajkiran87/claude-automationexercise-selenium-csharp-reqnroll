@cart
Feature: Shopping cart
  As a shopper
  I want to add and remove products in my cart
  So that I can control what I buy

  Background:
    Given I am on the products page

  @smoke
  Scenario: Add a single product to the cart
    When I add the product "Blue Top" to the cart
    And I open the cart
    Then the cart should contain "Blue Top" with quantity 1

  @regression
  Scenario: Add several products to the cart
    When I add these products to the cart:
      | product    |
      | Blue Top   |
      | Men Tshirt |
    And I open the cart
    Then the cart should contain "Blue Top" with quantity 1
    And the cart should contain "Men Tshirt" with quantity 1

  @regression
  Scenario: Add a product with a chosen quantity from the product details page
    When I open the product details for "Blue Top"
    And I add it to the cart with quantity 4
    And I open the cart
    Then the cart should contain "Blue Top" with quantity 4

  @regression
  Scenario: Remove the only product from the cart
    When I add the product "Blue Top" to the cart
    And I open the cart
    And I remove "Blue Top" from the cart
    Then the cart should be empty
