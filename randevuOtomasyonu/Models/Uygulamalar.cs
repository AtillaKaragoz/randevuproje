using System;
using System.Collections.Generic;

namespace randevuOtomasyonu.Models;

public partial class Uygulamalar
{
    public int ServisId { get; set; }

    public string? ServisTuru { get; set; }

    public string? ServisAciklama { get; set; }

    public string? ServisUcreti { get; set; }
}
