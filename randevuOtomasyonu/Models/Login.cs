using System;
using System.Collections.Generic;

namespace randevuOtomasyonu.Models;

public partial class Login
{
    public int LoginId { get; set; }

    public string? Email { get; set; }

    public string? Sifre { get; set; }
}
