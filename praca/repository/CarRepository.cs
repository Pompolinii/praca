using Microsoft.EntityFrameworkCore;
using praca.Entities;

namespace praca.repository
{
    public class CarRepository : ICarRepository
    {
        private readonly CarDbContext _carDbContext;

        public CarRepository(CarDbContext carDbContext)
        {
            _carDbContext=carDbContext;
        }
        public async Task<Car> PostCarAsync(string marka, string model, int year, bool isRented, string imagePath)
        {
            var car = new Car
            {
                Marka = marka,
                Model = model,
                Year = year,
                IsRented = isRented,
                ImagePath = imagePath // Zapisujemy ścieżkę do obrazu
            };

            await _carDbContext.Cars.AddAsync(car);
            await _carDbContext.SaveChangesAsync();
            return car;
        }



        public async Task<bool> DeleteCarAsync(int carId)
        {
            var carToDelete = await _carDbContext.Set<Car>().FindAsync(carId);

            if (carToDelete == null)
                return false; // Samochód nie został znaleziony, więc nie można go usunąć

            _carDbContext.Set<Car>().Remove(carToDelete);
            await _carDbContext.SaveChangesAsync();

            return true; // Usunięcie samochodu zakończone sukcesem
        }


        public async Task<bool> UpdateCarAsync(int id, string model, string marka, int year, string imagePath)
        {
            var car = await _carDbContext.Cars.FindAsync(id);
            if (car == null)
                return false;

            car.Marka = marka;
            car.Model = model;
            car.Year = year;

            // Aktualizujemy ścieżkę do obrazu tylko wtedy, gdy jest przekazana
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

        public async Task<IEnumerable<Car>> SearchCarsAsync(string marka, string model, int? minYear, int? maxYear)
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

            if (minYear.HasValue)
            {
                query = query.Where(c => c.Year >= minYear.Value);
            }

            if (maxYear.HasValue)
            {
                query = query.Where(c => c.Year <= maxYear.Value);
            }

            return await query.ToListAsync();
        }


    }

}
