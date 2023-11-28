using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class RoomsWithPrice
    {
        public int RoomId { get; set; }
        public string? RoomType { get; set; }
        public int? RoomCapacity { get; set; }
        public string? RoomDescription { get; set; }
        public decimal? RoomCost { get; set; }
    }
}
