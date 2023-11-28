using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class RoomService
    {
        public int RoomServiceId { get; set; }
        public int? RoomId { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Room? Room { get; set; }
    }
}
