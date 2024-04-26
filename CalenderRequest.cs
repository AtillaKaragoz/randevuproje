using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using randevuOtomasyonu.Google_Calendar;
using randevuOtomasyonu.Google_Calendar.Helper;
using System.Globalization;

namespace randevuOtomasyonu.Controllers
{
    [Route("~/api/randevu")]
    [ApiController]
    public class CalenderRequest : ControllerBase
    {
        [HttpPost]
        public async Task <IActionResult> CreateGoogleCalendar([FromBody] google_calendar request)
        {
            return Ok(await GoogleCalendarHelper.CreateGoogleCalendar(request));


        }

    }
}
