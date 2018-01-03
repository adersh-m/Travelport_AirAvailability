using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirAvailabilityTest.Models
{
    public class SearchModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}