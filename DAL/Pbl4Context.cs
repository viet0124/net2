using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DTO;

namespace DAL;

public partial class Pbl4Context : DbContext
{

    public Pbl4Context()
    {
    }

    public Pbl4Context(DbContextOptions<Pbl4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DangHoatDong> TableDangHoatDong { get; set; }

    public virtual DbSet<GiaoDich> TableGiaoDich { get; set; }

    public virtual DbSet<KhoHang> TableKhoHang { get; set; }

    public virtual DbSet<LoaiMay> TableLoaiMay { get; set; }

    public virtual DbSet<MatHang> TableMatHang { get; set; }

    public virtual DbSet<May> TableMay { get; set; }

    public virtual DbSet<PhanLoaiHang> TablePhanLoaiHang { get; set; }

    public virtual DbSet<PhanMem> TablePhanMem { get; set; }

    public virtual DbSet<TaiKhoan> TableTaiKhoan { get; set; }

    public virtual DbSet<TinhTrangMay> TableTinhTrangMay { get; set; }

    public virtual DbSet<VaiTro> TableVaiTro { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseLazyLoadingProxies().UseSqlServer(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("Default")); // MultipleActiveResultSets=True;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DangHoatDong>(entity =>
        {
            entity.HasKey(e => new { e.IdTaiKhoan, e.IdMay }).HasName("PK__Dang_Hoat_Dong__ID_Tai_Khoan__ID_May");

            entity.ToTable("Dang_Hoat_Dong");

            entity.HasIndex(e => e.IdMay, "UK__Dang_Hoat_Dong__ID_May").IsUnique();

            entity.HasIndex(e => e.IdTaiKhoan, "UK__Dang_Hoat_Dong__ID_Tai_Khoan").IsUnique();

            entity.Property(e => e.IdTaiKhoan).HasColumnName("ID_Tai_Khoan");
            entity.Property(e => e.IdMay).HasColumnName("ID_May");

            entity.HasOne(d => d.IdMayNavigation).WithOne(p => p.DangHoatDong)
                .HasForeignKey<DangHoatDong>(d => d.IdMay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dang_Hoat_Dong__ID_May__May__ID");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithOne(p => p.DangHoatDong)
                .HasForeignKey<DangHoatDong>(d => d.IdTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Dang_Hoat_Dong__ID_Tai_Khoan__Tai_Khoan__ID");
        });

        modelBuilder.Entity<GiaoDich>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Giao_Dich__ID");

            entity.ToTable("Giao_Dich");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.IdTaiKhoan).HasColumnName("ID_Tai_Khoan");
            entity.Property(e => e.NoiDungGiaoDich)
                .HasColumnType("text")
                .HasColumnName("Noi_Dung_Giao_Dich");
            entity.Property(e => e.SoTien)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("So_Tien");
            entity.Property(e => e.ThoiGian)
                .HasPrecision(0)
                .HasColumnName("Thoi_Gian");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.GiaoDiches)
                .HasForeignKey(d => d.IdTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Giao_Dich__ID_Tai_Khoan__Tai_Khoan__ID");
        });

        modelBuilder.Entity<KhoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Kho_Hang__ID");

            entity.ToTable("Kho_Hang");

            entity.HasIndex(e => e.TenHang, "UK__Kho_Hang__Ten_Hang").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SoLuong).HasColumnName("So_Luong");
            entity.Property(e => e.TenHang)
                .HasMaxLength(30)
                .HasColumnName("Ten_Hang");
        });

        modelBuilder.Entity<LoaiMay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Loai_May__ID");

            entity.ToTable("Loai_May");

            entity.HasIndex(e => e.TenLoaiMay, "UK__Loai_May__Ten_Loai_May").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenLoaiMay)
                .HasMaxLength(20)
                .HasColumnName("Ten_Loai_May");
        });

        modelBuilder.Entity<MatHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Mat_Hang__ID");

            entity.ToTable("Mat_Hang");

            entity.HasIndex(e => e.TenMatHang, "UK__Mat_Hang__Ten_Mat_Hang").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdPhanLoaiHang).HasColumnName("ID_Phan_Loai_Hang");
            entity.Property(e => e.MoTa)
                .HasColumnType("text")
                .HasColumnName("Mo_Ta");
            entity.Property(e => e.TenMatHang)
                .HasMaxLength(30)
                .HasColumnName("Ten_Mat_Hang");
            entity.Property(e => e.UrlHinhAnh)
                .HasColumnType("text")
                .HasColumnName("URL_Hinh_Anh");

            entity.HasOne(d => d.IdPhanLoaiHangNavigation).WithMany(p => p.MatHangs)
                .HasForeignKey(d => d.IdPhanLoaiHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mat_Hang__ID_Phan_Loai_Hang__Phan_Loai_Hang__ID");
        });

        modelBuilder.Entity<May>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__May__ID");

            entity.ToTable("May");

            entity.HasIndex(e => e.DiaChiIpv4, "UK__May__Dia_Chi_IPv4").IsUnique();

            entity.HasIndex(e => e.DiaChiMac, "UK__May__Dia_Chi_MAC").IsUnique();

            entity.HasIndex(e => e.TenMay, "UK__May__Ten_May").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DiaChiIpv4)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Dia_Chi_IPv4");
            entity.Property(e => e.DiaChiMac)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("Dia_Chi_MAC");
            entity.Property(e => e.IdLoaiMay).HasColumnName("ID_Loai_May");
            entity.Property(e => e.IdTinhTrang).HasColumnName("ID_Tinh_Trang");
            entity.Property(e => e.TenMay)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ten_May");

            entity.HasOne(d => d.IdLoaiMayNavigation).WithMany(p => p.Mays)
                .HasForeignKey(d => d.IdLoaiMay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__May__ID_Loai_May__Loai_May__ID");

            entity.HasOne(d => d.IdTinhTrangNavigation).WithMany(p => p.Mays)
                .HasForeignKey(d => d.IdTinhTrang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__May__ID_Tinh_Trang__Tinh_Trang_May__ID");
        });

        modelBuilder.Entity<PhanLoaiHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phan_Loai_Hang__ID");

            entity.ToTable("Phan_Loai_Hang");

            entity.HasIndex(e => e.TenLoai, "UK__Kho_Hang__Ten_Loai").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(30)
                .HasColumnName("Ten_Loai");
        });

        modelBuilder.Entity<PhanMem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phan_Mem__ID");

            entity.ToTable("Phan_Mem");

            entity.HasIndex(e => e.TenPhanMem, "UK__Phan_Mem__Ten_Phan_Mem").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DungLuong).HasColumnName("Dung_Luong");
            entity.Property(e => e.DuongDan)
                .HasColumnType("text")
                .HasColumnName("Duong_Dan");
            entity.Property(e => e.MoTa)
                .HasColumnType("text")
                .HasColumnName("Mo_Ta");
            entity.Property(e => e.TenPhanMem)
                .HasMaxLength(255)
                .HasColumnName("Ten_Phan_Mem");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tai_Khoan__ID");

            entity.ToTable("Tai_Khoan");

            entity.HasIndex(e => e.TenDangNhap, "UK__Tai_Khoan__Ten_Dang_Nhap").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.HoTen)
                .HasMaxLength(30)
                .HasColumnName("Ho_Ten");
            entity.Property(e => e.IdVaiTro).HasColumnName("ID_Vai_Tro");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Mat_Khau");
            entity.Property(e => e.SoDu)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("So_Du");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ten_Dang_Nhap");

            entity.HasOne(d => d.IdVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.IdVaiTro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tai_Khoan__ID_Vai_Tro__Vai_Tro__ID");
        });

        modelBuilder.Entity<TinhTrangMay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tinh_Trang_May__ID");

            entity.ToTable("Tinh_Trang_May");

            entity.HasIndex(e => e.TenTinhTrang, "UK__Tinh_Trang_May__Ten_Tinh_Trang").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenTinhTrang)
                .HasMaxLength(20)
                .HasColumnName("Ten_Tinh_Trang");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vai_Tro__ID");

            entity.ToTable("Vai_Tro");

            entity.HasIndex(e => e.TenVaiTro, "UK__Vai_Tro__Ten_Vai_Tro").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenVaiTro)
                .HasMaxLength(20)
                .HasColumnName("Ten_Vai_Tro");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
