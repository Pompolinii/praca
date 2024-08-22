using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using praca.Entities;
using praca.repository;
using System.Security.Claims;

namespace praca.Controllers
{

    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {

        private readonly ICarRepository _carRepository;
        public CarController(ICarRepository carRepository)
        {

            _carRepository = carRepository;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> PostCar([FromForm] CarCreateDto carCreateDto) // Zmieniamy na [FromForm]
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? imagePath = null;

            if (carCreateDto.ImagePath != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder); // Upewnij siê, ¿e katalog jest tworzony
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + carCreateDto.ImagePath.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Zapis pliku
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await carCreateDto.ImagePath.CopyToAsync(fileStream);
                }

                // Ustal œcie¿kê do obrazu
                imagePath = $"/images/{uniqueFileName}";
            }

            // Zaktualizuj wywo³anie repository, aby u¿ywaæ poprawnej œcie¿ki
            var result = await _carRepository.PostCarAsync(
                carCreateDto.Marka,
                carCreateDto.Model,
                carCreateDto.Year,
                carCreateDto.IsRented,
                imagePath // Przekazujemy œcie¿kê
            );

            return Ok("Samochód zosta³ pomyœlnie dodany");
        }




        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarAsync(int id)
        {
            var carDeleted = await _carRepository.DeleteCarAsync(id);

            if (!carDeleted)
                return NotFound(); // Samochód nie zosta³ znaleziony, wiêc zwracamy NotFound

            return Ok(new { Message = "Samochód zosta³ pomyœlnie usuniêty." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCarAsync(int id, [FromForm] CarUpdateDto request)
        {
            string imagePath = null;

            if (request.ImagePath != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.ImagePath.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImagePath.CopyToAsync(fileStream);
                }
                imagePath = $"/images/{uniqueFileName}";
            }

            var carUpdated = await _carRepository.UpdateCarAsync(id, request.Model, request.Marka, request.Year, imagePath);

            if (!carUpdated)
                return NotFound(); // Samochód nie zosta³ znaleziony, wiêc zwracamy NotFound

            return Ok(new { Message = "Samochód zosta³ pomyœlnie zaktualizowany." });
        }


        // GET: api/cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return Ok(cars);
        }


        // GET: api/cars/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _carRepository.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpGet("search-cars")]
        public async Task<ActionResult<IEnumerable<Car>>> SearchCars([FromQuery] string? marka, [FromQuery] string? model, [FromQuery] int? minYear, [FromQuery] int? maxYear)
        {
            var cars = await _carRepository.SearchCarsAsync(marka, model, minYear, maxYear);
            if (cars == null || !cars.Any())
            {
                return NotFound("Nie ma aut o takim kryterium.");
            }
            return Ok(cars);
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuthorization()
        {
            return Ok("Jesteœ zalogowany!");
        }

        [Authorize]
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return Ok(roles);
        }



    }




}
