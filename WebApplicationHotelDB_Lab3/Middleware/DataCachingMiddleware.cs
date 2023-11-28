using Microsoft.Extensions.Caching.Memory;
using WebApplicationHotel.Data;
using WebApplicationHotelDB_Lab3.Models;
using WebApplicationHotel.Services;

namespace WebApplicationHotel.Middleware
{
    public class DataCachingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICachedDataService _cachedDataService;

        public DataCachingMiddleware(RequestDelegate next, ICachedDataService cachedDataService)
        {
            _next = next;
            _cachedDataService = cachedDataService;
        }

        public async Task Invoke(HttpContext context)
        {
            string clientsCacheKey = "ClientsCacheKey";
            string clientServicesCacheKey = "ClientServicesCacheKey";
            string employeesCacheKey = "EmployeesCacheKey";
            string hotelServicesCacheKey = "HotelServicesCacheKey";
            string roomsCacheKey = "RoomsCacheKey";
            string roomPricesCacheKey = "RoomPricesCacheKey";
            string roomServicesCacheKey = "RoomServicesCacheKey";

            if (!_cachedDataService.GetClientsFromCache(clientsCacheKey).Any())
            {
                IEnumerable<Client> clients = _cachedDataService.GetClients();
                _cachedDataService.AddClientsToCache(clientsCacheKey);
            }

            if (!_cachedDataService.GetClientServicesFromCache(clientServicesCacheKey).Any())
            {
                IEnumerable<ClientService> clientServices = _cachedDataService.GetClientServices();
                _cachedDataService.AddClientServicesToCache(clientServicesCacheKey);
            }

            if (!_cachedDataService.GetEmployeesFromCache(employeesCacheKey).Any())
            {
                IEnumerable<Employee> employees = _cachedDataService.GetEmployees();
                _cachedDataService.AddEmployeesToCache(employeesCacheKey);
            }

            if (!_cachedDataService.GetHotelServicesFromCache(hotelServicesCacheKey).Any())
            {
                IEnumerable<HotelService> hotelServices = _cachedDataService.GetHotelServices();
                _cachedDataService.AddHotelServicesToCache(hotelServicesCacheKey);
            }

            if (!_cachedDataService.GetRoomsFromCache(roomsCacheKey).Any())
            {
                IEnumerable<Room> rooms = _cachedDataService.GetRooms();
                _cachedDataService.AddRoomsToCache(roomsCacheKey);
            }

            if (!_cachedDataService.GetRoomPricesFromCache(roomPricesCacheKey).Any())
            {
                IEnumerable<RoomPrice> roomPrices = _cachedDataService.GetRoomPrices();
                _cachedDataService.AddRoomPricesToCache(roomPricesCacheKey);
            }

            if (!_cachedDataService.GetRoomServicesFromCache(roomServicesCacheKey).Any())
            {
                IEnumerable<RoomService> roomServices = _cachedDataService.GetRoomServices();
                _cachedDataService.AddRoomServicesToCache(roomServicesCacheKey);
            }

            await _next(context);
        }
    }
}
