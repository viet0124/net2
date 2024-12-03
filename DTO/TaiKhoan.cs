using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO;

public partial class TaiKhoan: INotifyPropertyChanged
{
    public static decimal MoneyPerSecond { get; set; } = 5000m / 3600m;

    public int Id { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public byte IdVaiTro { get; set; }

    private decimal _SoDu;
    public decimal SoDu
    {
        get => _SoDu;
        set
        {
            decimal rounded = Math.Round(value, 2);
            if (_SoDu != rounded)
            {
                _SoDu = rounded;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoDu)));
                PropertyChanged?.Invoke(this, new(nameof(ThoiGianConLai_Rounded)));
            }
        }
    }

    public string ThoiGianConLai_Rounded
    {
        get
        {
            TimeSpan _ThoiGianConLai = TimeSpan.FromSeconds(Convert.ToDouble(SoDu / MoneyPerSecond));
            if (_ThoiGianConLai.Seconds > 0)
            {
                _ThoiGianConLai += TimeSpan.FromMinutes(1);
            }
            return _ThoiGianConLai.Hours.ToString("00") + ":" + _ThoiGianConLai.Minutes.ToString("00");
        }
    }

    public virtual DangHoatDong? DangHoatDong { get; set; }

    public virtual ICollection<GiaoDich> GiaoDiches { get; set; } = new List<GiaoDich>();

    public virtual VaiTro IdVaiTroNavigation { get; set; } = null!;

    public void Update(TaiKhoan newTK)
    {
        this.MatKhau = newTK.MatKhau;
        this.SoDu = newTK.SoDu;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
