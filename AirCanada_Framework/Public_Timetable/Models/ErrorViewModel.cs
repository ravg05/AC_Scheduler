using System;

namespace Public_Timetable.Models
{

    public class Flight
    {
        public string flightno { get; set; }
        public string PeriodOperation { get; set; }
        public string DaysOfOperation { get; set; }
        public string DepartureTime { get; set; }
        public string OriginStation { get; set; }
        public string DestinationStation { get; set; }
        public string Aircraft { get; set; }

    }





    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
