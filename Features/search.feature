@search
Feature: Product search
  As a shopper
  I want to search the product catalogue
  So that I can quickly find what I want to buy

  Background:
    Given I am on the products page

  @smoke
  Scenario: Search returns only matching products
    When I search for "Jeans"
    Then the searched products section should be displayed
    And every product in the results should contain "jeans"

  @regression @negative
  Scenario: Search for a product that does not exist
    When I search for "qa-no-such-product-12345"
    Then the searched products section should be displayed
    And no products should be displayed
