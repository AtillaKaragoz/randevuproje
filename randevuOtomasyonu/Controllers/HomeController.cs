using Microsoft.AspNetCore.Mvc;
using randevuOtomasyonu.Models;
using System.Diagnostics;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;


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


        [Route("/musteribilgi")]
        public IActionResult musteribilgi()
        {
            ViewBag.musteriler = _context.Musteris.ToList();
            return View();
            
        }

        //m�steriEkle

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

        //musteriD�zenle

        [HttpGet]
        public IActionResult musteriD�zenle(int id)
        {
            Musteri? musteriDuzen = _context.Musteris.Where(u => u.MusteriId == id).FirstOrDefault();
            if (musteriDuzen == null)
            {
                return NotFound();
            }
            return View(musteriDuzen);
        }

        [HttpPost]
        public IActionResult musteriD�zenle(int? id, Musteri musteri)
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

        //Uygulama D�zenle 

        [HttpGet]
        public IActionResult uygulamaD�zenle(int id)
        {
            Uygulamalar? uygD�zen = _context.Uygulamalars.Where(u => u.ServisId == id).FirstOrDefault();
            if (uygD�zen == null)
            {
                return NotFound();
            }
            return View(uygD�zen);
        }



        [HttpPost]
        public IActionResult uygulamaD�zenle(int? id, Uygulamalar uygulamalar)
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
            Uygulamalar? uygD�zen = _context.Uygulamalars.Where(u => u.ServisId == id).FirstOrDefault();
            return View(uygD�zen);
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
      

        [Route("/kullan�c�lar")]
        public IActionResult kullan�c�lar()
        {
            ViewBag.login = _context.Logins.ToList();
            return View();
            
        }


        //kullan�c�Ekle
        [HttpGet]
        public IActionResult kullan�c�Ekle(int id)
        {
            Login? loginEkle = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            return View(loginEkle);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult kullan�c�Ekle(Login login)
        {
            if (ModelState.IsValid)
            {
                _context.Logins.Add(login);
                _context.SaveChanges();

                return RedirectToAction(nameof(kullan�c�lar));


            }

            return View();
        }
        //

        //kullan�c�D�zenleme

        [HttpGet]
        public IActionResult kullan�c�D�zenle(int id)
        {
            Login? kullan�c�Duzen = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            if (kullan�c�Duzen == null)
            {
                return NotFound();
            }
            return View(kullan�c�Duzen);
        }



        [HttpPost]
        public IActionResult kullan�c�D�zenle(int? id, Login login)
        {           
            var kullan�c�Duzen = _context.Logins.ToList().Where(x => x.LoginId == id).FirstOrDefault();
            if (kullan�c�Duzen != null)
            {
                kullan�c�Duzen.Email = login.Email;
                kullan�c�Duzen.Sifre = login.Sifre;         
                _context.SaveChanges();
                return RedirectToAction(nameof(kullan�c�lar));

            }
            return View(kullan�c�lar);
        }

        //

        //kullan�c�Sil

        [HttpGet]
        public IActionResult kullan�c�Sil(int? id)
        {
            Login? kullan�c�Sil = _context.Logins.Where(u => u.LoginId == id).FirstOrDefault();
            if (kullan�c�Sil == null)
            {
                return NotFound();
            }
            return View(kullan�c�Sil);


        }

        [HttpPost]
        public IActionResult kullan�c�Sil(int id)
        {
            var kullan�c�Sil = _context.Logins.Where(x => x.LoginId == id).FirstOrDefault();
            if (kullan�c�Sil != null)
            {
                _context.Logins.Remove(kullan�c�Sil);
                _context.SaveChanges();
                return RedirectToAction(nameof(kullan�c�lar));
            }
            return View();
        }

        [Route("/ayarlar")]
        public IActionResult ayarlar()
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
