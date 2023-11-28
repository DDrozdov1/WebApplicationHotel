using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class ClientService
    {
        public int ClientServiceId { get; set; }
        public int? ClientId { get; set; }
        public int? HotelServiceId { get; set; }
        public decimal? TotalCost { get; set; }

        public virtual Client? Client { get; set; }
        public virtual HotelService? HotelService { get; set; }
    }
}
