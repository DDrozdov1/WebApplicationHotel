using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class HotelService
    {
        public HotelService()
        {
            ClientServices = new HashSet<ClientService>();
        }

        public int HotelServiceid { get; set; }
        public string? HotelServiceName { get; set; }
        public string? HotelServiceDescription { get; set; }
        public decimal? HotelServiceCost { get; set; }

        public virtual ICollection<ClientService> ClientServices { get; set; }
    }
}
