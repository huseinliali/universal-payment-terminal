using Microsoft.AspNetCore.Mvc;
using UniversalPaymentTerminal.Application.Services;
using UniversalPaymentTerminal.Domain.Enums;

namespace UniversalPaymentTerminal.API.Controllers;

[ApiController]
[Route("api")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentsController(PaymentService service)
    {
        _service = service;
    }

    [HttpGet("payment-categories")]
    public IActionResult GetCategories() =>
        Ok(_service.GetCategories());

    [HttpGet("payment-methods")]
    public IActionResult GetMethods() =>
        Ok(_service.GetMethods());

    [HttpPost("service-payments/validate")]
    public async Task<IActionResult> Validate(
        [FromBody] ValidateRequest request)
    {
        var result = await _service.ValidateAsync(
            request.ProviderId,
            request.CustomerIdentifier);

        return Ok(result);
    }

    [HttpPost("payments")]
    public async Task<IActionResult> Pay(
    [FromBody] PayRequest request)
    {
        var result = await _service.PayAsync(
            request.ProviderId,
            request.PaymentMethod,
            request.Amount,
            request.Currency);

        return Ok(result);
    }
}

public record ValidateRequest(
    string ProviderId,
    string CustomerIdentifier);

public record PayRequest(
    string ProviderId,
    PaymentMethodType PaymentMethod,
    decimal Amount,
    string Currency);
