using System;
using System.Collections.Generic;

namespace DTO;

public partial class PhanMem
{
    public byte Id { get; set; }

    public string? TenPhanMem { get; set; }

    public string DuongDan { get; set; } = null!;

    public long DungLuong { get; set; }

    public string? MoTa { get; set; }
}
