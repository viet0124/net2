using System;
using System.Collections.Generic;

namespace DTO;

public partial class MatHang
{
    public byte Id { get; set; }

    public string TenMatHang { get; set; } = null!;

    public int Gia { get; set; }

    public string MoTa { get; set; } = null!;

    public string? UrlHinhAnh { get; set; }

    public byte IdPhanLoaiHang { get; set; }

    public virtual PhanLoaiHang IdPhanLoaiHangNavigation { get; set; } = null!;
}
