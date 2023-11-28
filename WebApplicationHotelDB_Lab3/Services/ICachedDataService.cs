using WebApplicationHotelDB_Lab3.Models;
using System.Collections.Generic;

namespace WebApplicationHotel.Services
{
    public interface ICachedDataService
    {
        IEnumerable<Client> GetClients(int rowsNumber = 20);
        void AddClientsToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<Client> GetClientsFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<ClientService> GetClientServices(int rowsNumber = 20);
        void AddClientServicesToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<ClientService> GetClientServicesFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<Employee> GetEmployees(int rowsNumber = 20);
        void AddEmployeesToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<Employee> GetEmployeesFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<HotelService> GetHotelServices(int rowsNumber = 20);
        void AddHotelServicesToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<HotelService> GetHotelServicesFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<Room> GetRooms(int rowsNumber = 20);
        void AddRoomsToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<Room> GetRoomsFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<RoomPrice> GetRoomPrices(int rowsNumber = 20);
        void AddRoomPricesToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<RoomPrice> GetRoomPricesFromCache(string cacheKey, int rowsNumber = 20);

        IEnumerable<RoomService> GetRoomServices(int rowsNumber = 20);
        void AddRoomServicesToCache(string cacheKey, int rowsNumber = 20);
        IEnumerable<RoomService> GetRoomServicesFromCache(string cacheKey, int rowsNumber = 20);
        public IEnumerable<string> GetRoomTypes(IEnumerable<Room> rooms)
        {
            return rooms.Select(r => r.RoomType).Where(rt => !string.IsNullOrEmpty(rt)).Distinct();
        }

        public IEnumerable<int?> GetRoomCapacities(IEnumerable<Room> rooms)
        {
            return rooms.Select(r => r.RoomCapacity).Where(rc => rc.HasValue).Distinct();
        }
    }
}
