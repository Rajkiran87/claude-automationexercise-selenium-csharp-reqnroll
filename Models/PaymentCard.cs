namespace AutomationExercise.Tests.Models;

/// <summary>Dummy card details. The practice site never charges anything.</summary>
public record PaymentCard(
    string NameOnCard,
    string CardNumber,
    string Cvc,
    string ExpiryMonth,
    string ExpiryYear);
