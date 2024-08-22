
using praca.Entities;

namespace praca.repository
{
    public interface ICarRepository 
    {
        Task<Car> PostCarAsync(string model, string marka, int Year , bool IsRented , string imagePath);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> UpdateCarAsync(int id, string model, string marka, int Year, string ImagePath);
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);

        Task<IEnumerable<Car>> SearchCarsAsync(string marka, string model, int? minYear, int? maxYear);
 
    }
}
