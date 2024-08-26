
using praca.Entities;

namespace praca.repository
{
    public interface ICarRepository 
    {
        Task<Car> PostCarAsync(string marka, string model, int year, bool isRented, string imagePath, string transmission, float enginesize, int price);
        Task<bool> DeleteCarAsync(int carId);
        Task<bool> UpdateCarAsync(int id, string model, string marka, int Year, string ImagePath, string transmission, float enginesize, int price);
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);

        Task<IEnumerable<Car>> SearchCarsAsync(string marka, string model, int? minYear, int? maxYear, string transmission, float? minEngineSize, float? maxEngineSize, int? minPrice, int? maxPrice);
 
    }
}
