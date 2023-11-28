using System;
using System.Collections.Generic;

namespace WebApplicationHotelDB_Lab3.Models
{
    public partial class Employee
    {
        public Employee()
        {
            RoomServices = new HashSet<RoomService>();
        }

        public int EmployeeId { get; set; }
        public string? EmployeeFullName { get; set; }
        public string? EmployeePosition { get; set; }

        public virtual ICollection<RoomService> RoomServices { get; set; }
    }
}
