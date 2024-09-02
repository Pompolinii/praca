using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using praca.Entities;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System.Globalization;

namespace praca.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private PayPalHttpClient _client;

        public PaymentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            var environment = new SandboxEnvironment("AQoPgLY96X4tWiWhZZH8ckpCuV86C1MHOsEGfbzScwspGVJO4tr0nLO_uz9rPx1TwLc8xlyibkllPOgs", "EOjKDbLPMEvOjwq4NMhWRaBVt2l7-GRtRiBJ338VcfBkS2NSHBGNCbi3FORlB6RlvXRwEk51iKbJiPpL");
            _client = new PayPalHttpClient(environment);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto orderRequest)
        {
            if (orderRequest.Amount <= 0)
            {
                return BadRequest("Kwota musi być większa niż zero.");
            }


            var order = new OrdersCreateRequest();
            order.Prefer("return=representation");
            order.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = "PLN",
                            Value = orderRequest.Amount.ToString("F2", CultureInfo.InvariantCulture)
                        }
                    }
                }
            });

            var response = await _client.Execute(order);
            var result = response.Result<Order>();

            return Ok(new { OrderId = result.Id });
        }


        [HttpPost("capture-order")]
        public async Task<IActionResult> CaptureOrder([FromBody] CaptureOrderRequest captureOrderRequest)
        {
            var request = new OrdersCaptureRequest(captureOrderRequest.OrderId);
            request.RequestBody(new OrderActionRequest());

            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            if (result.Status == "COMPLETED")
            {
                var user = await _userManager.FindByIdAsync(captureOrderRequest.UserId);
                user.Balance += captureOrderRequest.Amount;
                await _userManager.UpdateAsync(user);

                return Ok(new { Balance = user.Balance });
            }

            return BadRequest("Payment failed.");
        }


        public class OrderRequestDto
        {
            public decimal Amount { get; set; }
        }

        public class CaptureOrderRequest
        {
            public string OrderId { get; set; }
            public string UserId { get; set; }
            public decimal Amount { get; set; }
        }





    }
}