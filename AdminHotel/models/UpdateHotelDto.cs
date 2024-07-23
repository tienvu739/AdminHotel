using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminHotel.models
{
    internal class UpdateHotelDto
    {
        public string NameHotel { get; set; }
        public string AddressHotel { get; set; }
        public string DescribeHotel { get; set; }
        public string PolicyHotel { get; set; }
        public string TypeHotel { get; set; }
        public bool? StatusHotel { get; set; }
    }
}
