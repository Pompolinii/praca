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
        var activeRentals = await _context.Rentals
        .Where(r => r.UserId == request.UserId && !r.IsReturned)
        .CountAsync();

        if (activeRentals >= 2)
        {
            return BadRequest("Nie można wypożyczyć więcej niż 2 auta jednocześnie.");
        }

        var car = await _context.Cars.FindAsync(request.CarId);
        if (car == null || car.IsRented)
        {
            return BadRequest("Auto jest już wypożyczone lub nie istnieje.");
        }

        car.IsRented = true;

        var rental = new Rental
        {
            CarId = car.Id,
            UserId = request.UserId, 
            RentedAt = DateTime.Now
        };

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return Ok();
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

        rental.IsReturned = true;
        rental.Car.IsRented = false;
        rental.ReturnedAt = DateTime.Now;

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
}


