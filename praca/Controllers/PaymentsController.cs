using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using praca.Entities;

namespace praca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUpBalance([FromBody] TopUpRequest request)
        {
            // Logika do integracji z bramką płatności (np. Przelewy24 API)
            var result = await ProcessPayment(request.BlikCode, request.Amount);

            if (result.Success)
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                user.Balance += request.Amount;
                await _userManager.UpdateAsync(user);

                return Ok(new { Balance = user.Balance });
            }

            return BadRequest("Payment failed.");
        }

        private async Task<PaymentResult> ProcessPayment(string blikCode, decimal amount)
        {
            // Integracja z API bramki płatności
            // Zwróć PaymentResult z wynikiem płatności
            // Dla przykładu, zwracam sukces, ale w rzeczywistej implementacji tutaj byłoby API do bramki płatności
            return new PaymentResult { Success = true };
        }
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
    }

}
