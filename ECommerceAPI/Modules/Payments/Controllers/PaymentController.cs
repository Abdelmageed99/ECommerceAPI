using ECommerceAPI.Modules.Payments.CustomModels;
using ECommerceAPI.Modules.Payments.DTOs;
using ECommerceAPI.Modules.Payments.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Modules.Payments.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                var transactionId = await _paymentService.ProcessPaymentAsync(request.Amount, request.OrderId.ToString(), userId);
                return Ok(new { TransactionId = transactionId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //[Authorize("Admin")]        
        [HttpPost("Verify")]
        public async Task<IActionResult> HandleWebhook([FromBody] BraintreeWebhookRequest request)
        {
            var isVerified = await _paymentService.VerifyPaymentAsync(request.TransactionId);
            if (!isVerified)
            {
                return BadRequest("Invalid transaction");
            }

            return Ok(new { Message = "Payment Verified" });
        }

        //[Authorize("Admin")]
        [HttpGet]
        public async Task<ActionResult<PaymentPagedResult>> GetPayments([FromQuery] PaymentPagedRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var payments = await _paymentService.GetPaymentsAsync(request);
            return Ok(payments);

        }






    }
}
