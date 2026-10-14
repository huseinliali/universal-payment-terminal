namespace UniversalPaymentTerminal.Application.Models;

public record ServiceValidationResult(
    bool IsValid,
    string CustomerName,
    decimal OutstandingAmount,
    string Currency);