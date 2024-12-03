using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;

namespace DTO
{
    public delegate void HetThoiGianEventHandler(object sender, EventArgs e);
    public class TaiKhoanClient : INotifyPropertyChanged
    {
        public decimal MoneyPerSecond { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Id { get; init; }
        public string TenDangNhap { get; init; } = null!;
        public string MatKhau { get; set; } = null!;
        private decimal _SoDu;
        public decimal SoDu
        {
            get => _SoDu;
            private set
            {
                _SoDu = Math.Round(value, 2);
                PropertyChanged?.Invoke(this, new(nameof(SoDu_Rounded)));
            }
        }
        public string SoDu_Rounded
        {
            get
            {
                TimeSpan Tgian = TimeSpan.Parse(ThoiGianConLai_Rounded+":00");
                return ((int)((decimal)Tgian.TotalSeconds * MoneyPerSecond)).ToString("N0");
            }
        }
        private TimeSpan _ThoiGianDaSuDung;
        public string ThoiGianDaSuDung_Rounded
        {
            get
            {
                return _ThoiGianDaSuDung.Hours.ToString("00") + ":" + _ThoiGianDaSuDung.Minutes.ToString("00");
            }
        }
        public TimeSpan ThoiGianDaSuDung
        {
            get => _ThoiGianDaSuDung;
            private set
            {
                _ThoiGianDaSuDung = value;
                PropertyChanged?.Invoke(this, new(nameof(ThoiGianDaSuDung_Rounded)));
            }
        }

        private TimeSpan _ThoiGianConLai;
        public string ThoiGianConLai_Rounded
        {
            get
            {
                if (_ThoiGianConLai.Seconds > 0)
                {
                    TimeSpan temp = _ThoiGianConLai + TimeSpan.FromMinutes(1);
                    return temp.Hours.ToString("00") + ":" + temp.Minutes.ToString("00");
                }
                return _ThoiGianConLai.Hours.ToString("00") + ":" + (_ThoiGianConLai.Minutes).ToString("00");
            }
        }
        public TimeSpan ThoiGianConLai
        {
            get => _ThoiGianConLai; private set
            {
                _ThoiGianConLai = value;
                if (_ThoiGianConLai <= TimeSpan.Zero) 
                {
                    _SoDu = decimal.Zero;
                    HetThoiGian?.Invoke(this, new EventArgs());
                }
                
                else PropertyChanged?.Invoke(this, new(nameof(ThoiGianConLai_Rounded)));
            }
        }

        public TaiKhoanClient(int id, string tenDangNhap, string matKhau, decimal soDu, decimal MoneyPerSecond)
        {
            this.MoneyPerSecond = MoneyPerSecond;
            Id = id;
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            Set_SoDu(soDu);
            ThoiGianDaSuDung = TimeSpan.Zero;
        }
        public void GiamThoiGian1Giay()
        {
            ThoiGianDaSuDung += TimeSpan.FromSeconds(1);
            ThoiGianConLai -= TimeSpan.FromSeconds(1);
            SoDu -= Math.Round(MoneyPerSecond, 2);
        }

        public void Set_SoDu(decimal newSoTien)
        {
            SoDu = newSoTien;
            ThoiGianConLai = TimeSpan.FromSeconds(Convert.ToDouble(Math.Round(newSoTien, 2) / MoneyPerSecond));
        }
        public event HetThoiGianEventHandler? HetThoiGian;
    }
}
