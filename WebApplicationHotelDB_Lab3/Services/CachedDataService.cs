using WebApplicationHotelDB_Lab3.Models;
using Microsoft.Extensions.Caching.Memory;
using WebApplicationHotel.Data;

namespace WebApplicationHotel.Services
{
    public class CachedDataService : ICachedDataService
    {
        private readonly HotelDBContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public CachedDataService(HotelDBContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public IEnumerable<Client> GetClients(int rowsNumber = 20)
        {
            return _dbContext.Clients.Take(rowsNumber).ToList();
        }

        public void AddClientsToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Client> clients = _dbContext.Clients.Take(rowsNumber).ToList();
            if (clients != null)
            {
                _memoryCache.Set(cacheKey, clients, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<Client> GetClientsFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Client> clients;
            if (!_memoryCache.TryGetValue(cacheKey, out clients))
            {
                clients = _dbContext.Clients.Take(rowsNumber).ToList();
                if (clients != null)
                {
                    _memoryCache.Set(cacheKey, clients, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return clients;
        }

        public IEnumerable<ClientService> GetClientServices(int rowsNumber = 20)
        {
            return _dbContext.ClientServices.Take(rowsNumber).ToList();
        }

        public void AddClientServicesToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<ClientService> clientServices = _dbContext.ClientServices.Take(rowsNumber).ToList();
            if (clientServices != null)
            {
                _memoryCache.Set(cacheKey, clientServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<ClientService> GetClientServicesFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<ClientService> clientServices;
            if (!_memoryCache.TryGetValue(cacheKey, out clientServices))
            {
                clientServices = _dbContext.ClientServices.Take(rowsNumber).ToList();
                if (clientServices != null)
                {
                    _memoryCache.Set(cacheKey, clientServices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return clientServices;
        }
        public IEnumerable<Employee> GetEmployees(int rowsNumber = 20)
        {
            return _dbContext.Employees.Take(rowsNumber).ToList();
        }

        public void AddEmployeesToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> employees = _dbContext.Employees.Take(rowsNumber).ToList();
            if (employees != null)
            {
                _memoryCache.Set(cacheKey, employees, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<Employee> GetEmployeesFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Employee> employees;
            if (!_memoryCache.TryGetValue(cacheKey, out employees))
            {
                employees = _dbContext.Employees.Take(rowsNumber).ToList();
                if (employees != null)
                {
                    _memoryCache.Set(cacheKey, employees, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return employees;
        }

        public IEnumerable<HotelService> GetHotelServices(int rowsNumber = 20)
        {
            return _dbContext.HotelServices.Take(rowsNumber).ToList();
        }

        public void AddHotelServicesToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<HotelService> hotelServices = _dbContext.HotelServices.Take(rowsNumber).ToList();
            if (hotelServices != null)
            {
                _memoryCache.Set(cacheKey, hotelServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<HotelService> GetHotelServicesFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<HotelService> hotelServices;
            if (!_memoryCache.TryGetValue(cacheKey, out hotelServices))
            {
                hotelServices = _dbContext.HotelServices.Take(rowsNumber).ToList();
                if (hotelServices != null)
                {
                    _memoryCache.Set(cacheKey, hotelServices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return hotelServices;
        }

        public IEnumerable<Room> GetRooms(int rowsNumber = 20)
        {
            return _dbContext.Rooms.Take(rowsNumber).ToList();
        }

        public void AddRoomsToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Room> rooms = _dbContext.Rooms.Take(rowsNumber).ToList();
            if (rooms != null)
            {
                _memoryCache.Set(cacheKey, rooms, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<Room> GetRoomsFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<Room> rooms;
            if (!_memoryCache.TryGetValue(cacheKey, out rooms))
            {
                rooms = _dbContext.Rooms.Take(rowsNumber).ToList();
                if (rooms != null)
                {
                    _memoryCache.Set(cacheKey, rooms, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return rooms;
        }
        public IEnumerable<RoomPrice> GetRoomPrices(int rowsNumber = 20)
        {
            return _dbContext.RoomPrices.Take(rowsNumber).ToList();
        }

        public void AddRoomPricesToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<RoomPrice> roomPrices = _dbContext.RoomPrices.Take(rowsNumber).ToList();
            if (roomPrices != null)
            {
                _memoryCache.Set(cacheKey, roomPrices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<RoomPrice> GetRoomPricesFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<RoomPrice> roomPrices;
            if (!_memoryCache.TryGetValue(cacheKey, out roomPrices))
            {
                roomPrices = _dbContext.RoomPrices.Take(rowsNumber).ToList();
                if (roomPrices != null)
                {
                    _memoryCache.Set(cacheKey, roomPrices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return roomPrices;
        }
        public IEnumerable<RoomService> GetRoomServices(int rowsNumber = 20)
        {
            return _dbContext.RoomServices.Take(rowsNumber).ToList();
        }

        public void AddRoomServicesToCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<RoomService> roomServices = _dbContext.RoomServices.Take(rowsNumber).ToList();
            if (roomServices != null)
            {
                _memoryCache.Set(cacheKey, roomServices, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
        }

        public IEnumerable<RoomService> GetRoomServicesFromCache(string cacheKey, int rowsNumber = 20)
        {
            IEnumerable<RoomService> roomServices;
            if (!_memoryCache.TryGetValue(cacheKey, out roomServices))
            {
                roomServices = _dbContext.RoomServices.Take(rowsNumber).ToList();
                if (roomServices != null)
                {
                    _memoryCache.Set(cacheKey, roomServices, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }
            return roomServices;
        }

    }
}
