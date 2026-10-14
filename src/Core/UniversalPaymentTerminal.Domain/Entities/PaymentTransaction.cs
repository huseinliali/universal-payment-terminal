using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.Domain.Entities;

public class PaymentTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public PaymentCategory Category { get; set; }

    public PaymentMethodType PaymentMethod { get; set; }

    public string CustomerIdentifier { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal Fee { get; set; }

    public decimal TotalAmount => Amount + Fee;

    public string Currency { get; set; } = "EUR";

    public string Status { get; set; } = "Pending";
}