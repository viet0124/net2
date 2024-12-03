using System;
using System.Collections.Generic;

namespace DTO;

public partial class GiaoDich
{
    public int Id { get; set; }

    public int IdTaiKhoan { get; set; }

    public DateTime ThoiGian { get; set; }

    private decimal _SoTien;
    public decimal SoTien { get => _SoTien; set => _SoTien = Math.Round(value, 2); }

    public string NoiDungGiaoDich { get; set; } = null!;

    public virtual TaiKhoan IdTaiKhoanNavigation { get; set; } = null!;
}
