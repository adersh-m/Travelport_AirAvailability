using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirAvailabilityTest.Models
{
    public class AirAvailabilityResponseMain
    {
        public List<AirAvailabilityResponse> AirAvailabilityResponse { get; set; }
    }
    public class AirAvailabilityResponse
    {
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
        public List<AirAvailabilityResult> AirAvailabilityResult { get; set; }
    }
    public class AirAvailabilityResult
    {
        public string From { get; set; }
        public string To { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string Carrier { get; set; }

    }
}