using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO;

public partial class May : INotifyPropertyChanged
{
    public short Id { get; set; }

    public string TenMay { get; set; } = null!;

    public string DiaChiIpv4 { get; set; } = null!;

    public string DiaChiMac { get; set; } = null!;

    public byte IdLoaiMay { get; set; }

    public byte IdTinhTrang { get; set; }
    //private byte _IdTinhTrang;
    //public byte IdTinhTrang
    //{
    //    get => _IdTinhTrang;
    //    set
    //    {
    //        if (_IdTinhTrang != value)
    //        {
    //            _IdTinhTrang = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdTinhTrang)));
    //        }
    //    }
    //}

    //public virtual DangHoatDong? DangHoatDong { get; set; }
    private DangHoatDong? _DangHoatDong;
    public virtual DangHoatDong? DangHoatDong
    {
        get => _DangHoatDong;
        set
        {
            if (_DangHoatDong != value)
            {
                _DangHoatDong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DangHoatDong)));
            }
        }
    }

    public virtual LoaiMay IdLoaiMayNavigation { get; set; } = null!;

    //public virtual TinhTrangMay IdTinhTrangNavigation { get; set; } = null!;
    private TinhTrangMay _IdTinhTrangNavigation = null!;
    public virtual TinhTrangMay IdTinhTrangNavigation
    {
        get => _IdTinhTrangNavigation;
        set
        {
            if (_IdTinhTrangNavigation.Id != value.Id)
            {
                _IdTinhTrangNavigation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdTinhTrangNavigation)));
            }
        }
    }

    public void Update(May newMay)
    {
        this.DangHoatDong = newMay.DangHoatDong;
        this.IdTinhTrang = newMay.IdTinhTrang;
        this.IdTinhTrangNavigation = newMay.IdTinhTrangNavigation;
    }

    //private short _Id;
    //private string _TenMay = null!;
    //private string _DiaChiIpv4 = null!;
    //private string _DiaChiMac = null!;
    //private byte _IdTinhTrang;
    //private byte _IdLoaiMay;
    //private DangHoatDong? _DangHoatDong;
    //private LoaiMay _IdLoaiMayNavigation = null!;
    //private TinhTrangMay _IdTinhTrangNavigation = null!;

    //public short Id
    //{
    //    get => _Id; set
    //    {
    //        if (_Id != value)
    //        {
    //            _Id = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
    //        }
    //    }
    //}

    //public string TenMay
    //{
    //    get => _TenMay; set
    //    {
    //        if (_TenMay != value)
    //        {
    //            _TenMay = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TenMay)));
    //        }
    //    }
    //}

    //public string DiaChiIpv4
    //{
    //    get => _DiaChiIpv4; set
    //    {
    //        if (_DiaChiIpv4 != value)
    //        {
    //            _DiaChiIpv4 = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DiaChiIpv4)));
    //        }
    //    }
    //}

    //public string DiaChiMac
    //{
    //    get => _DiaChiMac; set
    //    {
    //        if (_DiaChiMac != value)
    //        {
    //            _DiaChiMac = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DiaChiMac)));
    //        }
    //    }
    //}
    //public byte IdTinhTrang
    //{
    //    get => _IdTinhTrang; set
    //    {
    //        if (_IdTinhTrang != value)
    //        {
    //            _IdTinhTrang = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdTinhTrang)));
    //        }
    //    }
    //}

    //public byte IdLoaiMay
    //{
    //    get => _IdLoaiMay; set
    //    {
    //        if (_IdLoaiMay != value)
    //        {
    //            _IdLoaiMay = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdLoaiMay)));
    //        }
    //    }
    //}

    //public virtual DangHoatDong? DangHoatDong {
    //    get => _DangHoatDong; set
    //    {
    //        if (_DangHoatDong != value)
    //        {
    //            _DangHoatDong = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DangHoatDong)));
    //        }
    //    }
    //}

    //public virtual LoaiMay IdLoaiMayNavigation {
    //    get => _IdLoaiMayNavigation; set
    //    {
    //        if (_IdLoaiMayNavigation != value)
    //        {
    //            _IdLoaiMayNavigation = value;
    //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IdLoaiMay)));
    //        }
    //    }
    //} 

    public event PropertyChangedEventHandler? PropertyChanged;
}
