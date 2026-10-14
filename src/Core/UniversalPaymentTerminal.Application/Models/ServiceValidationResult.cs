namespace UniversalPaymentTerminal.Application.Models;

public record ServiceValidationResult(
    bool IsValid,
    string CustomerName,
    decimal OutstandingAmount,
    string Currency);

public record PaymentResultDto(
    bool Success,
    decimal Amount,
    decimal CommissionAmount,
    decimal TotalAmount,
    string Currency);