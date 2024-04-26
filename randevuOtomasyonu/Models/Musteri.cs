using System;
using System.Collections.Generic;

namespace randevuOtomasyonu.Models;

public partial class Musteri
{
    public int MusteriId { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Marka { get; set; }

    public string? Model { get; set; }

    public int? Yil { get; set; }

    public string? MotorTipi { get; set; }
}
