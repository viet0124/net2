using System;
using System.Collections.Generic;

namespace DTO;

public partial class VaiTro
{
    public byte Id { get; set; }

    public string TenVaiTro { get; set; } = null!;

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
