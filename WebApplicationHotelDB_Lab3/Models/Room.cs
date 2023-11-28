using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class Room
    {
        public Room()
        {
            Clients = new HashSet<Client>();
            RoomPrices = new HashSet<RoomPrice>();
            RoomServices = new HashSet<RoomService>();
        }

        public int RoomId { get; set; }
        public string? RoomType { get; set; }
        public int? RoomCapacity { get; set; }
        public string? RoomDescription { get; set; }

        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<RoomPrice> RoomPrices { get; set; }
        public virtual ICollection<RoomService> RoomServices { get; set; }
    }
}
