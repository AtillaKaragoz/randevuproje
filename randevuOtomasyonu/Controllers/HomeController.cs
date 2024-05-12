using Microsoft.AspNetCore.Mvc;
using randevuOtomasyonu.Models;
using System.Diagnostics;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;


namespace randevuOtomasyonu.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RandevuprojeContext _context;

        public HomeController(ILogger<HomeController> logger, RandevuprojeContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OauthRedirect()
        {
            var credentialsFile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\credentials.json";

            JObject credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

            var client_id = credentials["client_id"];

            var redirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                            "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                            "access_type=offline& " +
                            "include_granted_scopes=true&" +
                            "response_type=code&" +
                            "state=basarili&" +
                            "redirect_uri=https://localhost:7036/oauth/callback&" +
                            "client_id=" + client_id;
          

            return Redirect(redirectUrl);
         }














        public void Callback(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                this.GetTokens(code);


            }



        }
        public IActionResult GetTokens(string code)
        {
            var tokenfile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\tokens.json";
            var credentialsFile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));



            //RestClient restClient = new RestClient("https://oauth2.googleapis.com/token");
            RestRequest request = new RestRequest();
            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("code", code);
            request.AddQueryParameter("grant_type", "authorization_code");
            request.AddQueryParameter("redirect_uri", "https://localhost:7036/oauth/callback");



            var client = new RestClient("https://oauth2.googleapis.com/token");
            var baseUrl = client.Options.BaseUrl;
            var response = client.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                System.IO.File.WriteAllText(tokenfile, response.Content);
                return RedirectToAction("randevuEkle", "Home");
            }




            return View("Error");

        }
        public IActionResult RefreshToken()
        {
            var tokenfile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\tokens.json";
            var credentialsFile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenfile));

            RestRequest request = new RestRequest();
            RestClient restClient = new RestClient();
            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("grant_type", "refresh_token");
            if (tokens.ContainsKey("refresh_token"))
            {
                request.AddQueryParameter("refresh_token", tokens["refresh_token"].ToString());

                var client = new RestClient("https://oauth2.googleapis.com/token");
                var baseUrl = client.Options.BaseUrl;
                var response = client.Post(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    JObject newTokens = JObject.Parse(response.Content);
                    newTokens["refresh_token"] = tokens["refresh_token"].ToString();
                    System.IO.File.WriteAllText(tokenfile, newTokens.ToString());
                    return RedirectToAction("randevuEkle", "Home", new { status = "basarili" });

                }

            }
            else
            {
                return View("Error");
            }




            return View("Error");
        }

        public IActionResult RevokeToken()
        {
            var tokenfile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenfile));



            RestRequest request = new RestRequest();
            request.AddQueryParameter("token", tokens["access_token"].ToString());

            var client = new RestClient("https://oauth2.googleapis.com/revoke");
            var baseUrl = client.Options.BaseUrl;
            var response = client.Post(request);


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return RedirectToAction("randevuEkle", "Home", new { status = "basarili" });

            }

            return View("Error");
        }












        [HttpGet]
        public IActionResult randevuEkle(int id)
        {

            ViewBag.Servisler = _context.Uygulamalars.ToList();

            var model2List = _context.Musteris.Where(x => x.MusteriId == id).FirstOrDefault();

            var viewModel = new MyViewModel
            {
                Model1List = _context.Uygulamalars.ToList(),
                Model2List = model2List,

            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult CreateEvent(Event calendarEvent)
        {
 

            var tokenFile = "C:\\Users\\oarda\\OneDrive\\Masaüstü\\OTOMASYON PROJE\\randevuproje\\randevuOtomasyonu\\Files\\tokens.json";
            var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));


            RestRequest request = new RestRequest();
            
            calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fff");
            calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.End.DateTime).ToString("yyyy-MM-dd'T'HH:mm:ss.fff");

            var model = JsonConvert.SerializeObject(calendarEvent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            request.AddQueryParameter("key", "AIzaSyB_18dmHsgBJSQiHhwnWbY-royBHcu7ae4");
            request.AddHeader("Authorization", "Bearer " + tokens["access_token"]);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", model, ParameterType.RequestBody);



            var client = new RestClient("https://www.googleapis.com/calendar/v3/calendars/primary/events");
            var baseUrl = client.Options.BaseUrl;
            var response = client.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home", new { status = "success" });
            }



            return View("Error");
            
        }





        












        [HttpGet]
        public IActionResult Search(string searchValue)
        {

            ViewBag.Search = searchValue;
            var musteriler = _context.Musteris.Where(x => x.Ad.Contains(searchValue) || x.Soyad.Contains(searchValue) || x.Telefon.Contains(searchValue) || x.Email.Contains(searchValue)).ToList();
          
            return View(musteriler);
           
        }
        //CreateEvent modeline randevuEkle modelini entegre edilecek
        //randevuEkle
       
        [HttpPost]
        public IActionResult randevuEkle(int? id)
        {
            //burada select list bilgileri ekledikten sonra database post iþlemi yapýlacak
            return View();
        }




        //


        [Route("/musteribilgi")]
        public IActionResult musteribilgi()
        {
            var musteriler = _context.Musteris.Take(6).ToList();
            return View(musteriler);
            
        }

        //müsteriEkle

        [HttpGet]
        public IActionResult musteriEkle(int id)
        {
            
            Musteri? musteriEkle = _context.Musteris.Where(x => x.MusteriId == id).FirstOrDefault();
            return View(musteriEkle);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult musteriEkle(Musteri musteri)
        {
            if (ModelState.IsValid)
            {

                _context.Musteris.Add(musteri);
                _context.SaveChanges();

                return RedirectToAction(nameof(musteribilgi));


            }

            return View();
        }
        //

        //musteriDüzenle

        [HttpGet]
        public IActionResult musteriDüzenle(int id)
        {
            Musteri? musteriDuzen = _context.Musteris.Where(u => u.MusteriId == id).FirstOrDefault();
            if (musteriDuzen == null)
            {
                return NotFound();
            }
            return View(musteriDuzen);
        }

        [HttpPost]
        public IActionResult musteriDüzenle(int? id, Musteri musteri)
        {
            var duzen = _context.Musteris.ToList().Where(x => x.MusteriId == id).FirstOrDefault();
            if (duzen != null)
            {
                duzen.Ad = musteri.Ad;
                duzen.Soyad = musteri.Soyad;
                duzen.Telefon = musteri.Telefon;
                duzen.Email = musteri.Email;
                duzen.Marka = musteri.Marka;
                duzen.Model = musteri.Model;
                duzen.Yil = musteri.Yil;
                duzen.MotorTipi = musteri.MotorTipi;

                _context.SaveChanges();
                return RedirectToAction(nameof(musteribilgi));

            }
            return View(musteri);
        }
        //

        [Route("/uygulamalar")]
        public IActionResult uygulamalar()
        {
            ViewBag.uygulamalar = _context.Uygulamalars.ToList();
            return View();
        }
        //

        //musteriSil

        [HttpGet]
        public IActionResult musteriSil(int? id)
        {
            Musteri? musteriSil = _context.Musteris.Where(u => u.MusteriId == id).FirstOrDefault();
            if (musteriSil == null)
            {
                return NotFound();
            }
            return View(musteriSil);


        }

        [HttpPost]
        public IActionResult musteriSil(int id)
        {
            var musteriSil = _context.Musteris.Where(x => x.MusteriId == id).FirstOrDefault();
            if (musteriSil != null)
            {
                _context.Musteris.Remove(musteriSil);
                _context.SaveChanges();
                return RedirectToAction(nameof(musteribilgi));
            }
            return View();
        }
        //

        //Uygulama Sil
        [HttpGet]
        public IActionResult uygulamaSil(int? id)
        {
            Uygulamalar? uyg = _context.Uygulamalars.Where(u => u.ServisId == id).FirstOrDefault();
            if (uyg == null)
            {
                return NotFound();
            }
            return View(uyg);

          
        }
   
      
        [HttpPost]
        public IActionResult uygulamaSil(int id)
        {
            var silme = _context.Uygulamalars.Where(x => x.ServisId == id).FirstOrDefault();
            if (silme!=null)
            {
                _context.Uygulamalars.Remove(silme);
                _context.SaveChanges();
                return RedirectToAction(nameof(uygulamalar));
            }
            return View();
        }
        //

        //Uygulama Düzenle 

        [HttpGet]
        public IActionResult uygulamaDüzenle(int id)
        {
            Uygulamalar? uygDüzen = _context.Uygulamalars.Where(u => u.ServisId == id).FirstOrDefault();
            if (uygDüzen == null)
            {
                return NotFound();
            }
            return View(uygDüzen);
        }



        [HttpPost]
        public IActionResult uygulamaDüzenle(int? id, Uygulamalar uygulamalar)
        {
            //var duzen = _context.Uygulamalars.Where(x => x.ServisId == id).FirstOrDefault();
            var duzen = _context.Uygulamalars.ToList().Where(x => x.ServisId == id).FirstOrDefault();
            if (duzen != null)
            {
                duzen.ServisTuru = uygulamalar.ServisTuru;
                duzen.ServisAciklama = uygulamalar.ServisAciklama;
                duzen.ServisUcreti = uygulamalar.ServisUcreti;

                _context.SaveChanges();
                return RedirectToAction(nameof(uygulamalar));

            }
            return View(uygulamalar);
        }
        //

        //Uygulama Ekle

        [HttpGet]
        public IActionResult uygulamaEkle(int id)
        {
            Uygulamalar? uygDüzen = _context.Uygulamalars.Where(u => u.ServisId == id).FirstOrDefault();
            return View(uygDüzen);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult uygulamaEkle(Uygulamalar uygulama,string ServisUcreti)
        {
            if (ModelState.IsValid)
            {

                    //uygulama.ServisUcreti = ServisUcreti;
                    _context.Uygulamalars.Add(uygulama);
                    _context.SaveChanges();
                
                    return RedirectToAction(nameof(uygulamalar)); 
                
               
            }
            
            return View();
        }
        //
      

        [Route("/kullanýcýlar")]
        public IActionResult kullanýcýlar()
        {
            ViewBag.login = _context.Logins.ToList();
            return View();
            
        }


        //kullanýcýEkle
        [HttpGet]
        public IActionResult kullanýcýEkle(int id)
        {
            Login? loginEkle = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            return View(loginEkle);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult kullanýcýEkle(Login login)
        {
            if (ModelState.IsValid)
            {
                _context.Logins.Add(login);
                _context.SaveChanges();

                return RedirectToAction(nameof(kullanýcýlar));


            }

            return View();
        }
        //

        //kullanýcýDüzenleme

        [HttpGet]
        public IActionResult kullanýcýDüzenle(int id)
        {
            Login? kullanýcýDuzen = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            if (kullanýcýDuzen == null)
            {
                return NotFound();
            }
            return View(kullanýcýDuzen);
        }



        [HttpPost]
        public IActionResult kullanýcýDüzenle(int? id, Login login)
        {           
            var kullanýcýDuzen = _context.Logins.ToList().Where(x => x.LoginId == id).FirstOrDefault();
            if (kullanýcýDuzen != null)
            {
                kullanýcýDuzen.Email = login.Email;
                kullanýcýDuzen.Sifre = login.Sifre;         
                _context.SaveChanges();
                return RedirectToAction(nameof(kullanýcýlar));

            }
            return View(kullanýcýlar);
        }

        //

        //kullanýcýSil

        [HttpGet]
        public IActionResult kullanýcýSil(int? id)
        {
            Login? kullanýcýSil = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            if (kullanýcýSil == null)
            {
                return NotFound();
            }
            return View(kullanýcýSil);


        }

        [HttpPost]
        public IActionResult kullanýcýSil(int id)
        {
            var kullanýcýSil = _context.Logins.Where(x => x.LoginId == id).FirstOrDefault();
            if (kullanýcýSil != null)
            {
                _context.Logins.Remove(kullanýcýSil);
                _context.SaveChanges();
                return RedirectToAction(nameof(kullanýcýlar));
            }
            return View();
        }

        [Route("/ayarlar")]
        public IActionResult ayarlar()
        {
            return View();
        }


        public IActionResult _dahafazla()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
}
