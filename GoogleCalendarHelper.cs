using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace randevuOtomasyonu.Google_Calendar.Helper
{
    public class GoogleCalendarHelper
    {   
        protected GoogleCalendarHelper()
        {

        }
        public static async Task<Event> CreateGoogleCalendar(google_calendar request)

        {
            string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
            string ApplicationName = "Google Calendar Api";
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "NewFolder", "dene.json"), FileMode.Open, FileAccess.Read))
         
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,

                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore  (credPath,true)).Result;
            }
            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,



            });

            Event eventCalendar = new Event()
            {

                Summary = request.Summary,
                Location = request.konum,
                
                Start= new EventDateTime
                {
                    DateTime= new DateTime(1/1/2024)

                },
                

                End= new EventDateTime
                {
                    DateTime = new DateTime(2/2/2024)
                }
                ,
                Description = request.Aciklama



            };
            var eventRequest = services.Events.Insert(eventCalendar, "primary");
            var requestCreate = await eventRequest.ExecuteAsync();


            return requestCreate;
        }
    }
}
