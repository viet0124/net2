using System.ComponentModel;

namespace DTO;

public partial class TinhTrangMay: INotifyPropertyChanged
{
    public byte Id { get; set; }

    //public string TenTinhTrang { get; set; } = null!;
    private string _TenTinhTrang = null!;
    public string TenTinhTrang
    {
        get => _TenTinhTrang;
        set
        {
            if (!_TenTinhTrang.Equals(value))
            {
                _TenTinhTrang = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TenTinhTrang)));
            }
        }
    }

    public virtual ICollection<May> Mays { get; set; } = new List<May>();

    public event PropertyChangedEventHandler? PropertyChanged;
}
