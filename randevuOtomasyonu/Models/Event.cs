using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;

namespace randevuOtomasyonu.Models;

public partial class Event


{
    public Event()
    {
        this.Start = new EventDateTime()
        {
            TimeZone = "Europe/Istanbul"
        };
        this.End = new EventDateTime()
        {
            TimeZone = "Europe/Istanbul"
        };
    }
    public string Summary { get; set; }
    
    public string Description { get; set; }
    
    public EventDateTime Start { get; set; }
    public EventDateTime End { get; set; }

    public class EventDateTime()
    {
        public string DateTime { get; set; }
        public string TimeZone { get; set; }
    }

}
