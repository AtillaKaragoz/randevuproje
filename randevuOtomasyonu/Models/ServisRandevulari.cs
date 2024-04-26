using System;
using System.Collections.Generic;

namespace randevuOtomasyonu.Models;

public partial class ServisRandevulari
{
    public int RandevuId { get; set; }

    public int? MusteriId { get; set; }

    public int? ModelId { get; set; }

    public int? ServisId { get; set; }

    public DateTime RandevuTarihi { get; set; }

    public string ServisTuru { get; set; } = null!;

    public string? Aciklama { get; set; }

    public virtual Uygulamalar? Servis { get; set; }
}
