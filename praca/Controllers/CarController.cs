using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> PostCar([FromForm] CarCreateDto carCreateDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string? imagePath = null;

            if (carCreateDto.ImagePath != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder); 
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + carCreateDto.ImagePath.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await carCreateDto.ImagePath.CopyToAsync(fileStream);
                }

                
                imagePath = $"/images/{uniqueFileName}";
            }

            
            var result = await _carRepository.PostCarAsync(
                carCreateDto.Marka,
                carCreateDto.Model,
                carCreateDto.Year,
                carCreateDto.IsRented,
                imagePath,
                carCreateDto.Transmission,
                carCreateDto.Enginesize,
                carCreateDto.Price
            );

            return Ok("Samochód zosta³ pomyœlnie dodany");
        }




        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarAsync(int id)
        {
            var carDeleted = await _carRepository.DeleteCarAsync(id);

            if (!carDeleted)
                return NotFound(); 

            return Ok(new { Message = "Samochód zosta³ pomyœlnie usuniêty." });
        }
        [Authorize(Policy = "AdminOnly")]
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

            var carUpdated = await _carRepository.UpdateCarAsync(id, request.Model, request.Marka, request.Year, imagePath , request.Transmission, request.Enginesize, request.Price);

            if (!carUpdated)
                return NotFound(); 

            return Ok(new { Message = "Samochód zosta³ pomyœlnie zaktualizowany." });
        }


        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            var cars = await _carRepository.GetAllCarsAsync();
            return Ok(cars);
        }


       
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
        public async Task<ActionResult<IEnumerable<Car>>> SearchCars([FromQuery] string? marka, [FromQuery] string? model, [FromQuery] int? minYear, [FromQuery] int? maxYear, [FromQuery] string? transmission, [FromQuery] float? minEngineSize, [FromQuery] int? minPrice , [FromQuery] float? maxEngineSize , [FromQuery] int? maxPrice)
        {
            var cars = await _carRepository.SearchCarsAsync(marka, model, minYear, maxYear, transmission, minEngineSize, maxEngineSize, minPrice, maxPrice);
            if (cars == null || !cars.Any())
            {
                return Ok(new List<Car>());
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
