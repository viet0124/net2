using System;
using System.Collections.Generic;

namespace DTO;

public partial class KhoHang
{
    public byte Id { get; set; }

    public string TenHang { get; set; } = null!;

    public int SoLuong { get; set; }
}
