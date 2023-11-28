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
            // ��������� ����������� ��� ������� � �� � �������������� EF
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HotelDBContext>(options => options.UseSqlServer(connection));

            // ���������� �����������
            services.AddMemoryCache();

            // ���������� ��������� ������
            services.AddDistributedMemoryCache();
            services.AddSession();

            // ��������� ����������� CachedMaterialsService
            services.AddScoped<ICachedDataService, CachedDataService>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            // ��������� ��������� ������
            app.UseSession();


            app.Map("/rooms", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ ������ �� ���� ������
                    IEnumerable<Room> cachedRooms;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRooms = cachedDataService.GetRoomsFromCache("CachedRooms");
                    }

                    // ��������� HTML-������ � ����������� � ��������
                    string htmlString = "<HTML><HEAD><TITLE>������ ������</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ ������</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>��� �������</TH>" +
                        "<TH>�����������</TH>" +
                        "<TH>��������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/clients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ �������� �� ���� ������
                    IEnumerable<Client> cachedClients;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedClients = cachedDataService.GetClientsFromCache("CachedClients");
                    }

                    // ��������� HTML-������ � ����������� � ��������
                    string htmlString = "<HTML><HEAD><TITLE>������ ��������</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ ��������</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>������ ���</TH>" +
                        "<TH>���������� ������</TH>" +
                        "<TH>���� ������</TH>" +
                        "<TH>���� ������</TH>" +
                        "<TH>������������� �������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            // ����� ���������� � �������
            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ������������ ������ ��� ������ 
                    string htmlString = "<HTML><HEAD><TITLE>����������</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>����������:</H1>"
                    + "<BR> ������: " + context.Request.Host
                    + "<BR> ����: " + context.Request.PathBase
                    + "<BR> ��������: " + context.Request.Protocol
                    + "<BR><A href='/'>�������</A></BODY></HTML>";
                    // ����� ������
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/client-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ ���������� ����� �� ���� ������
                    IEnumerable<ClientService> cachedClientServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedClientServices = cachedDataService.GetClientServicesFromCache("CachedClientServices");
                    }

                    // ��������� HTML-������ � ����������� � ���������� �������
                    string htmlString = "<HTML><HEAD><TITLE>������ ���������� �����</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ ���������� �����</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>������������� �������</TH>" +
                        "<TH>������������� ������ �����</TH>" +
                        "<TH>����� ���������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/employees", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ ����������� �� ���� ������
                    IEnumerable<Employee> cachedEmployees;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedEmployees = cachedDataService.GetEmployeesFromCache("CachedEmployees");
                    }

                    // ��������� HTML-������ � ����������� � �����������
                    string htmlString = "<HTML><HEAD><TITLE>������ �����������</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ �����������</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>������ ���</TH>" +
                        "<TH>���������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/hotel-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ ����������� ����� �� ���� ������
                    IEnumerable<HotelService> cachedHotelServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedHotelServices = cachedDataService.GetHotelServicesFromCache("CachedHotelServices");
                    }

                    // ��������� HTML-������ � ����������� � ����������� �������
                    string htmlString = "<HTML><HEAD><TITLE>������ ����������� �����</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ ����������� �����</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>�������� ������</TH>" +
                        "<TH>�������� ������</TH>" +
                        "<TH>��������� ������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/room-prices", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ ��� �� ������ �� ���� ������
                    IEnumerable<RoomPrice> cachedRoomPrices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRoomPrices = cachedDataService.GetRoomPricesFromCache("CachedRoomPrices");
                    }

                    // ��������� HTML-������ � ����������� � ����� �� ������
                    string htmlString = "<HTML><HEAD><TITLE>������ ��� �� ������</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ ��� �� ������</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>������������� ������</TH>" +
                        "<TH>��������� ������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
                    await context.Response.WriteAsync(htmlString);
                });
            });

            app.Map("/room-services", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    // ��������� ��������� ���������� �� �������� ��������� �������
                    var serviceProvider = context.RequestServices;

                    // ��������� ������������ �������� ������� �� ���� ������
                    IEnumerable<RoomService> cachedRoomServices;
                    using (var scope = serviceProvider.CreateScope())
                    {
                        var cachedDataService = scope.ServiceProvider.GetRequiredService<ICachedDataService>();
                        cachedRoomServices = cachedDataService.GetRoomServicesFromCache("CachedRoomServices");
                    }

                    // ��������� HTML-������ � ����������� � �������� �������
                    string htmlString = "<HTML><HEAD><TITLE>������ �������� �������</TITLE></HEAD>" +
                        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<BODY><H1>������ �������� �������</H1>" +
                        "<TABLE BORDER=1>" +
                        "<TR>" +
                        "<TH>�������������</TH>" +
                        "<TH>������������� ������</TH>" +
                        "<TH>������������� ����������</TH>" +
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
                        "<BR><A href='/'>�������</A></BR>" +
                        "</BODY></HTML>";

                    // ��������� ��������� Content-Type ��� �������� ���� �����������
                    context.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

                    // ����� HTML-������ � �������� �����
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

                    string htmlString = "<html><head><title>������</title></head>" +
                        "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                        "<body>" +
                        "<form method='post' action='/searchform1'>" +
                            "<label>��� ������:</label>" +
                            "<select name='roomType'>" +
                            "<option value='all'>���</option>";

                    foreach (var roomType in roomTypes)
                    {
                        htmlString += $"<option value='{roomType}' {(roomType == room.RoomType ? "selected" : "")}>{roomType}</option>";
                    }

                    htmlString += "</select><br><br>" +
                        "<label>�����������:</label>" +
                        "<select name='roomCapacity'>" +
                        "<option value='0'>�����</option>";

                    foreach (var roomCapacity in roomCapacities)
                    {
                        htmlString += $"<option value='{roomCapacity}' {(roomCapacity == room.RoomCapacity ? "selected" : "")}>{roomCapacity}</option>";
                    }

                    htmlString += "</select><br><br>" +
                        "<input type='submit' value='�����'>" +
                        "</form>";

                    htmlString += "<h1>������ �������</h1>" +
                        "<table border='1'>" +
                        "<tr>" +
                            "<th>�����</th>" +
                            "<th>��� ������</th>" +
                            "<th>�����������</th>" +
                        "</tr>";

                    foreach (var rm in rooms)
                    {
                        htmlString += "<tr>" +
                            $"<td>{rm.RoomId}</td>" +
                            $"<td>{rm.RoomType}</td>" +
                            $"<td>{rm.RoomCapacity}</td>" +
                        "</tr>";
                    }

                    htmlString += "</table><br><a href='/'>�������</a></br></body></html>";
                    await context.Response.WriteAsync(htmlString);
                });
            });
            app.Run((context) =>
            {
                string htmlString = "<html><head><title>��������� ��������</title></head>" +
                    "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<body>" +
                    "<h1>�������</h1>" +
                    "<ul>" +
                    "<li><a href='/searchroom'>����� ������</a></li>" +
                    "<li><a href='/room-services'>������ �������</a></li>" +
                    "<li><a href='/room-prices'>���� �� �������</a></li>" +
                    "<li><a href='/hotel-services'>������ �����</a></li>" +
                    "<li><a href='/employees'>����������</a></li>" +
                    "<li><a href='/client-services'>������ ��� ��������</a></li>" +
                    "<li><a href='/info'>���������� � �������</a></li>" +
                    "<li><a href='/clients'>�������</a></li>" +
                    "<li><a href='/rooms'>�������</a></li>" +
                    "</ul>" +
                    "</body></html>";

                return context.Response.WriteAsync(htmlString);
            });
            app.Run();
        }
    }
    
}