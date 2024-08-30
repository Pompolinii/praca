using Microsoft.EntityFrameworkCore;
using praca.Entities;
using System.IO.Pipes;

namespace praca.repository
{
    public class CarRepository : ICarRepository
    {
        private readonly CarDbContext _carDbContext;

        public CarRepository(CarDbContext carDbContext)
        {
            _carDbContext=carDbContext;
        }
        public async Task<Car> PostCarAsync(string marka, string model, int year, bool isRented, string imagePath , string transmission, float enginesize , int price )
        {
            var car = new Car
            {
                Marka = marka,
                Model = model,
                Year = year,
                IsRented = isRented,
                ImagePath = imagePath,
                Transmission= transmission,
                Enginesize=enginesize,
                Price= price
            };

            await _carDbContext.Cars.AddAsync(car);
            await _carDbContext.SaveChangesAsync();
            return car;
        }



        public async Task<bool> DeleteCarAsync(int carId)
        {
            var carToDelete = await _carDbContext.Set<Car>().FindAsync(carId);

            if (carToDelete == null)
                return false; 

            _carDbContext.Set<Car>().Remove(carToDelete);
            await _carDbContext.SaveChangesAsync();

            return true; 
        }


        public async Task<bool> UpdateCarAsync(int id, string model, string marka, int year, string imagePath , string transmission, float enginesize, int price)
        {
            var car = await _carDbContext.Cars.FindAsync(id);
            if (car == null)
                return false;

            car.Marka = marka;
            car.Model = model;
            car.Year = year;
            car.Transmission = transmission;
            car.Enginesize = enginesize;
            car.Price = price;

            
            if (!string.IsNullOrEmpty(imagePath))
            {
                car.ImagePath = imagePath;
            }

            _carDbContext.Cars.Update(car);
            await _carDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carDbContext.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _carDbContext.Cars.FindAsync(id);
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(string marka, string model, int? minYear, int? maxYear, string transmission, float? minEngineSize, float? maxEngineSize, int? minPrice, int? maxPrice)
        {
            var query = _carDbContext.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(marka))
            {
                query = query.Where(c => c.Marka.ToLower().Contains(marka.ToLower()));
            }

            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));
            }

            if (!string.IsNullOrEmpty(transmission))
            {
                query = query.Where(c => c.Transmission.ToLower().Contains(transmission.ToLower()));
            }

            if (minEngineSize.HasValue)
            {
                query = query.Where(c => c.Enginesize >= minEngineSize.Value);
            }

            if (maxEngineSize.HasValue)
            {
                query = query.Where(c => c.Enginesize <= maxEngineSize.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(c => c.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= maxPrice.Value);
            }

            if (minYear.HasValue)
            {
                query = query.Where(c => c.Year >= minYear.Value);
            }

            if (maxYear.HasValue)
            {
                query = query.Where(c => c.Year <= maxYear.Value);
            }

            var cars = await query.ToListAsync();

            return (cars);


        }



    }

}
