using System;
using System.Collections.Generic;

namespace DTO;

public partial class DangHoatDong
{
    public int IdTaiKhoan { get; set; }

    public short IdMay { get; set; }

    public virtual May IdMayNavigation { get; set; } = null!;

    public virtual TaiKhoan IdTaiKhoanNavigation { get; set; } = null!;

    public static bool operator ==(DangHoatDong? dhd1, DangHoatDong? dhd2)
    {
        if (dhd1 is null && dhd2 is null) return true;
        if (dhd1 is null ^ dhd2 is null) return false;
        if (dhd1!.IdTaiKhoan != dhd2!.IdTaiKhoan || dhd1.IdMay != dhd2.IdMay) return false;
        else return true;
    }
    public static bool operator !=(DangHoatDong? dhd1, DangHoatDong? dhd2)
    {
        return !(dhd1 == dhd2);
    }
    public override bool Equals(object? obj)
    {
        if (obj is DangHoatDong other) return this == other;
        else return false;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
