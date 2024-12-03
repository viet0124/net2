using System;
using System.Collections.Generic;

namespace DTO;

public partial class PhanLoaiHang
{
    public byte Id { get; set; }

    public string TenLoai { get; set; } = null!;

    public virtual ICollection<MatHang> MatHangs { get; set; } = new List<MatHang>();
}
