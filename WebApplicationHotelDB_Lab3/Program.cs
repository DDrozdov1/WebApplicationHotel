using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplicationHotel.Data;
using WebApplicationHotel.Services;
using WebApplicationHotelDB_Lab3.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;


namespace WebApplicationHotelDB_Lab3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            // внедрение зависимости для доступа к БД с использованием EF
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HotelDBContext>(options => options.UseSqlServer(connection));

            // добавление кэширования
            services.AddMemoryCache();

            // добавление поддержки сессии
            services.AddDistributedMemoryCache();
            services.AddSession();

            // внедрение зависимости CachedMaterialsService
            services.AddScoped<ICachedDataService, CachedDataService>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            // добавляем поддержку сессий
            app.UseSession();


            app.Map("/rooms", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных комнат из базы данных
                    IEnumerable<Room> cachedRooms;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRooms = cachedDataService.GetRoomsFromCache("CachedRooms");
                    }

                    // Генерация HTML-строки с информацией о комнатах
                    string htmlString = "<HTML><HEAD><TITLE>Список комнат</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список комнат</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Тип комнаты</TH>" +
                        "<TH>Вместимость</TH>" +
                        "<TH>Описание</TH>" +
                        "</TR>";

                    foreach (var room in cachedRooms)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{room.RoomId}</TD>" +
                            $"<TD>{room.RoomType}</TD>" +
                            $"<TD>{room.RoomCapacity}</TD>" +
                            $"<TD>{room.RoomDescription}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/clients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных клиентов из базы данных
                    IEnumerable<Client> cachedClients;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedClients = cachedDataService.GetClientsFromCache("CachedClients");
                    }

                    // Генерация HTML-строки с информацией о клиентах
                    string htmlString = "<HTML><HEAD><TITLE>Список клиентов</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список клиентов</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Полное имя</TH>" +
                        "<TH>Паспортные данные</TH>" +
                        "<TH>Дата заезда</TH>" +
                        "<TH>Дата выезда</TH>" +
                        "<TH>Идентификатор комнаты</TH>" +
                        "</TR>";

                    foreach (var client in cachedClients)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{client.ClientId}</TD>" +
                            $"<TD>{client.ClientFullName}</TD>" +
                            $"<TD>{client.ClientPassportDetails}</TD>" +
                            $"<TD>{client.CheckInDate}</TD>" +
                            $"<TD>{client.CheckOutDate}</TD>" +
                            $"<TD>{client.RoomId}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            // Вывод информации о клиенте
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Формирование строки для вывода 
                    string htmlString = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>"
                    + "<BR> Сервер: " + context.Request.Host
                    + "<BR> Путь: " + context.Request.PathBase
                    + "<BR> Протокол: " + context.Request.Protocol
                    + "<BR><A href='/'>Главная</A></BODY></HTML>";
                    // Вывод данных
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/client-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных клиентских услуг из базы данных
                    IEnumerable<ClientService> cachedClientServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedClientServices = cachedDataService.GetClientServicesFromCache("CachedClientServices");
                    }

                    // Генерация HTML-строки с информацией о клиентских услугах
                    string htmlString = "<HTML><HEAD><TITLE>Список клиентских услуг</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список клиентских услуг</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Идентификатор клиента</TH>" +
                        "<TH>Идентификатор услуги отеля</TH>" +
                        "<TH>Общая стоимость</TH>" +
                        "</TR>";

                    foreach (var clientService in cachedClientServices)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{clientService.ClientServiceId}</TD>" +
                            $"<TD>{clientService.ClientId}</TD>" +
                            $"<TD>{clientService.HotelServiceId}</TD>" +
                            $"<TD>{clientService.TotalCost}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/employees", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных сотрудников из базы данных
                    IEnumerable<Employee> cachedEmployees;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedEmployees = cachedDataService.GetEmployeesFromCache("CachedEmployees");
                    }

                    // Генерация HTML-строки с информацией о сотрудниках
                    string htmlString = "<HTML><HEAD><TITLE>Список сотрудников</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список сотрудников</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Полное имя</TH>" +
                        "<TH>Должность</TH>" +
                        "</TR>";

                    foreach (var employee in cachedEmployees)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{employee.EmployeeId}</TD>" +
                            $"<TD>{employee.EmployeeFullName}</TD>" +
                            $"<TD>{employee.EmployeePosition}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/hotel-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных гостиничных услуг из базы данных
                    IEnumerable<HotelService> cachedHotelServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedHotelServices = cachedDataService.GetHotelServicesFromCache("CachedHotelServices");
                    }

                    // Генерация HTML-строки с информацией о гостиничных услугах
                    string htmlString = "<HTML><HEAD><TITLE>Список гостиничных услуг</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список гостиничных услуг</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Название услуги</TH>" +
                        "<TH>Описание услуги</TH>" +
                        "<TH>Стоимость услуги</TH>" +
                        "</TR>";

                    foreach (var hotelService in cachedHotelServices)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{hotelService.HotelServiceid}</TD>" +
                            $"<TD>{hotelService.HotelServiceName}</TD>" +
                            $"<TD>{hotelService.HotelServiceDescription}</TD>" +
                            $"<TD>{hotelService.HotelServiceCost}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/room-prices", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных цен на номера из базы данных
                    IEnumerable<RoomPrice> cachedRoomPrices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRoomPrices = cachedDataService.GetRoomPricesFromCache("CachedRoomPrices");
                    }

                    // Генерация HTML-строки с информацией о ценах на номера
                    string htmlString = "<HTML><HEAD><TITLE>Список цен на номера</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список цен на номера</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Идентификатор номера</TH>" +
                        "<TH>Стоимость номера</TH>" +
                        "</TR>";

                    foreach (var roomPrice in cachedRoomPrices)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{roomPrice.RoomPriceId}</TD>" +
                            $"<TD>{roomPrice.RoomId}</TD>" +
                            $"<TD>{roomPrice.RoomCost}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/room-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // Получение облачного провайдера из текущего контекста запроса
                    var serviceProvider = context.RequestServices;

                    // Получение кэшированных сервисов номеров из базы данных
                    IEnumerable<RoomService> cachedRoomServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRoomServices = cachedDataService.GetRoomServicesFromCache("CachedRoomServices");
                    }

                    // Генерация HTML-строки с информацией о сервисах номеров
                    string htmlString = "<HTML><HEAD><TITLE>Список сервисов номеров</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>Список сервисов номеров</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>Идентификатор</TH>" +
                        "<TH>Идентификатор номера</TH>" +
                        "<TH>Идентификатор сотрудника</TH>" +
                        "</TR>";

                    foreach (var roomService in cachedRoomServices)
                    {
                        htmlString += "<TR>" +
                            $"<TD>{roomService.RoomServiceId}</TD>" +
                            $"<TD>{roomService.RoomId}</TD>" +
                            $"<TD>{roomService.EmployeeId}</TD>" +
                            "</TR>";
                    }

                    htmlString += "</TABLE>" +
                        "<BR><A href='/'>Главная</A></BR>" +
                        "</BODY></HTML>";

                    // Установка заголовка Content-Type для указания типа содержимого
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // Вывод HTML-строки в выходной поток
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/searchroom", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var room = new Room();
                    ICachedDataService cachedRooms = context.RequestServices.GetService<ICachedDataService>();
                    IEnumerable<Room> rooms = cachedRooms.GetRoomsFromCache("rooms20");
                    IEnumerable<string> roomTypes = cachedRooms.GetRoomTypes(rooms);
                    IEnumerable<int?> roomCapacities = cachedRooms.GetRoomCapacities(rooms);

                    if (context.Request.Method == "POST")
                    {
                        room.RoomType = context.Request.Form["roomType"];
                        room.RoomCapacity = int.Parse(context.Request.Form["roomCapacity"]);

                        context.Response.Cookies.Append("room", JsonConvert.SerializeObject(room));

                        if (room.RoomType != "all" && room.RoomCapacity != 0)
                        {
                            rooms = rooms.Where(r => r.RoomType == room.RoomType && r.RoomCapacity >= room.RoomCapacity);
                        }
                        else if (room.RoomType != "all")
                        {
                            rooms = rooms.Where(r => r.RoomType == room.RoomType);
                        }
                        else if (room.RoomCapacity != 0)
                        {
                            rooms = rooms.Where(r => r.RoomCapacity >= room.RoomCapacity);
                        }
                    }
                    else if (context.Request.Cookies.ContainsKey("room"))
                    {
                        room = JsonConvert.DeserializeObject<Room>(context.Request.Cookies["room"]);
                    }

                    string htmlString = "<html><head><title>Номера</title></head>" +
                        "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<body>" +
                        "<form method='post' action='/searchform1'>" +
                            "<label>Тип номера:</label>" +
                            "<select name='roomType'>" +
                            "<option value='all'>Все</option>";

                    foreach (var roomType in roomTypes)
                    {
                        htmlString += $"<option value='{roomType}' {(roomType == room.RoomType ? "selected" : "")}>{roomType}</option>";
                    }

                    htmlString += "</select><br><br>" +
                        "<label>Вместимость:</label>" +
                        "<select name='roomCapacity'>" +
                        "<option value='0'>Любая</option>";

                    foreach (var roomCapacity in roomCapacities)
                    {
                        htmlString += $"<option value='{roomCapacity}' {(roomCapacity == room.RoomCapacity ? "selected" : "")}>{roomCapacity}</option>";
                    }

                    htmlString += "</select><br><br>" +
                        "<input type='submit' value='Поиск'>" +
                        "</form>";

                    htmlString += "<h1>Список номеров</h1>" +
                        "<table border='1'>" +
                        "<tr>" +
                            "<th>Номер</th>" +
                            "<th>Тип номера</th>" +
                            "<th>Вместимость</th>" +
                        "</tr>";

                    foreach (var rm in rooms)
                    {
                        htmlString += "<tr>" +
                            $"<td>{rm.RoomId}</td>" +
                            $"<td>{rm.RoomType}</td>" +
                            $"<td>{rm.RoomCapacity}</td>" +
                        "</tr>";
                    }

                    htmlString += "</table><br><a href='/'>Главная</a></br></body></html>";
                    await context.Response.WriteAsync(htmlString);
                });
            });
            app.Run((context) =>
            {
                string htmlString = "<html><head><title>Стартовая страница</title></head>" +
                    "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<body>" +
                    "<h1>Главная</h1>" +
                    "<ul>" +
                    "<li><a href='/searchroom'>Поиск комнат</a></li>" +
                    "<li><a href='/room-services'>Услуги комнаты</a></li>" +
                    "<li><a href='/room-prices'>Цены на комнаты</a></li>" +
                    "<li><a href='/hotel-services'>Услуги отеля</a></li>" +
                    "<li><a href='/employees'>Сотрудники</a></li>" +
                    "<li><a href='/client-services'>Услуги для клиентов</a></li>" +
                    "<li><a href='/info'>Информация о клиенте</a></li>" +
                    "<li><a href='/clients'>Клиенты</a></li>" +
                    "<li><a href='/rooms'>Комнаты</a></li>" +
                    "</ul>" +
                    "</body></html>";

                return context.Response.WriteAsync(htmlString);
            });
            app.Run();
        }
    }
    
}