using UniversalPaymentTerminal.Application.Contracts;
using UniversalPaymentTerminal.Application.Models;
using UniversalPaymentTerminal.Domain.Enums;
using UniversalPaymentTerminal.Infrastructure.ExternalServices;

namespace UniversalPaymentTerminal.Infrastructure.Adapters;

public class UniversityPaymentAdapter : IServicePaymentProvider
{
    private readonly UniversityApi _api;

    public UniversityPaymentAdapter(UniversityApi api)
    {
        _api = api;
    }

    public PaymentCategory Category =>
        PaymentCategory.Education;

    public string ProviderId => "university-1";

    public async Task<ServiceValidationResult> ValidateAsync(
        string customerIdentifier)
    {
        var student = await _api.GetStudentAsync(
            customerIdentifier);

        if (student is null)
            return new(false, "", 0, "EUR");

        return new(
            true,
            student.Value.Name,
            student.Value.Debt,
            "EUR");
    }
}


public class BankPaymentAdapter : IServicePaymentProvider
{
    private readonly BankApi _bankApi;

    public BankPaymentAdapter(BankApi bankApi)
    {
        _bankApi = bankApi;
    }

    public PaymentCategory Category => PaymentCategory.Bank;

    public string ProviderId => "bank-1";

    public async Task<ServiceValidationResult> ValidateAsync(
        string customerIdentifier)
    {
        var loan = await _bankApi.GetLoanAsync(customerIdentifier);

        if (loan is null)
        {
            return new ServiceValidationResult(
                false, string.Empty, 0m, "EUR");
        }

        return new ServiceValidationResult(
            true,
            loan.Value.Name,
            loan.Value.Debt,
            "EUR");
    }
}

public class UtilityPaymentAdapter : IServicePaymentProvider
{
    private readonly UtilityApi _utilityApi;

    public UtilityPaymentAdapter(UtilityApi utilityApi)
    {
        _utilityApi = utilityApi;
    }

    public PaymentCategory Category => PaymentCategory.Utility;

    public string ProviderId => "utility-1";

    public async Task<ServiceValidationResult> ValidateAsync(
        string customerIdentifier)
    {
        var account = await _utilityApi.GetUtilityAsync(
            customerIdentifier);

        if (account is null)
        {
            return new ServiceValidationResult(
                false, string.Empty, 0m, "EUR");
        }

        return new ServiceValidationResult(
            true,
            account.Value.Name,
            account.Value.Debt,
            "EUR");
    }
}

public class FinePaymentAdapter : IServicePaymentProvider
{
    private readonly FineApi _fineApi;

    public FinePaymentAdapter(FineApi fineApi)
    {
        _fineApi = fineApi;
    }

    public PaymentCategory Category => PaymentCategory.Fine;

    public string ProviderId => "fine-1";

    public async Task<ServiceValidationResult> ValidateAsync(
        string customerIdentifier)
    {
        var fine = await _fineApi.GetFineAsync(customerIdentifier);

        if (fine is null)
        {
            return new ServiceValidationResult(
                false, string.Empty, 0m, "EUR");
        }

        return new ServiceValidationResult(
            true,
            fine.Value.Name,
            fine.Value.Debt,
            "EUR");
    }
}