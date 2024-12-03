using System;
using System.Collections.Generic;

namespace DTO;

public partial class LoaiMay
{
    public byte Id { get; set; }

    public string TenLoaiMay { get; set; } = null!;

    public virtual ICollection<May> Mays { get; set; } = new List<May>();
}
