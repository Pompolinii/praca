using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praca.Entities;

[Route("api/[controller]")]
[ApiController]
public class RentalsController : ControllerBase
{
    private readonly CarDbContext _context;

    public RentalsController(CarDbContext context)
    {
        _context = context;
    }

    [HttpPost("rent")]
    public async Task<IActionResult> RentCar([FromBody] RentRequest request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound("Użytkownik nie istnieje.");
        }

        var activeRentals = await _context.Rentals
            .Where(r => r.UserId == request.UserId && !r.IsReturned)
            .CountAsync();

        if (activeRentals >= 2)
        {
            return BadRequest("Nie można wypożyczyć więcej niż 2 auta jednocześnie.");
        }

        var car = await _context.Cars.FindAsync(request.CarId);
        if (car == null)
        {
            return BadRequest("Auto nie istnieje.");
        }

        // Sprawdzenie, czy samochód jest dostępny w zadanym przedziale czasowym
        var isAvailable = !_context.Rentals
            .Any(r => r.CarId == request.CarId
                   && r.StartDate < request.EndDate
                   && r.EndDate > request.StartDate
                   && !r.IsReturned);  // Uwzględniamy tylko wynajmy, które nie zostały zwrócone

        if (!isAvailable)
        {
            return BadRequest("Samochód jest już wynajęty w zadanym przedziale czasowym.");
        }

        // Obliczanie liczby dni wynajmu (plus jeden, aby uwzględnić pełny dzień zakończenia)
        var numberOfDays = (request.EndDate - request.StartDate).Days;
        var totalCost = numberOfDays * car.Price;

        // Sprawdzanie salda użytkownika
        if (user.Balance < totalCost)
        {
            return BadRequest("Niewystarczające środki na koncie.");
        }

        if (request.StartDate > request.EndDate)
        {
            return BadRequest("Data rozpoczęcia nie może być późniejsza niż data zakończenia.");
        }


        if (request.StartDate == request.EndDate)
        {
            return BadRequest("Auto musi być wypożyczone na miniumum 1 dzień");
        }


        // Obciążenie salda użytkownika
        user.Balance -= totalCost;

        var rental = new Rental
        {
            CarId = car.Id,
            UserId = request.UserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsReturned = false
        };

        car.IsRented = true;  // Oznaczamy samochód jako wynajęty

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return Ok(new { TotalCost = totalCost });
    }

    [HttpPost("return")]
    public async Task<IActionResult> ReturnCar([FromBody] ReturnRequest request)
    {
        var rental = await _context.Rentals
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.CarId == request.CarId && r.UserId == request.UserId && !r.IsReturned);

        if (rental == null)
        {
            return BadRequest("Wypożyczenie nie zostało znalezione.");
        }

        rental.IsReturned = true;  // Oznaczamy wynajem jako zakończony
        rental.Car.IsRented = false;  // Oznaczamy samochód jako dostępny

        await _context.SaveChangesAsync();

        return Ok();
    }




    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserRentals(string userId)
    {
        var rentals = await _context.Rentals
            .Include(r => r.Car)
            .Where(r => r.UserId == userId && !r.IsReturned) 
            .ToListAsync();

        return Ok(rentals);
    }

    [HttpPost("topup")]
    public async Task<IActionResult> TopUp([FromBody] TopUpRequest request)
    {
        var user = await _context.Users.FindAsync(request.UserId);
        if (user == null)
        {
            return NotFound("Użytkownik nie istnieje.");
        }

        // Walidacja kodu Blik (np. długość i zawartość)
        if (string.IsNullOrWhiteSpace(request.BlikCode) || request.BlikCode.Length != 6)
        {
            return BadRequest("Niepoprawny kod Blik.");
        }

        user.Balance += request.Amount;

        await _context.SaveChangesAsync();

        return Ok(new { NewBalance = user.Balance });
    }


}


