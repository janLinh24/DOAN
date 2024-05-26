using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebBanSua.Models
{
    public partial class CuaHangBanSuaContext : DbContext
    {
        public CuaHangBanSuaContext()
        {
        }

        public CuaHangBanSuaContext(DbContextOptions<CuaHangBanSuaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<DanhMucSp> DanhMucSps { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<RoleAccount> RoleAccounts { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        public virtual DbSet<TinTuc> TinTucs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-N695PFT\\SQLEXPRESS;Database=CuaHangBanSua;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.TaiKhoan)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__RoleID__35BCFE0A");
            });

            modelBuilder.Entity<ChiTietDonHang>(entity =>
            {
                entity.HasKey(e => e.MaCtdh)
                    .HasName("PK__ChiTietD__1E4E40F0C7EDC280");

                entity.ToTable("ChiTietDonHang");

                entity.Property(e => e.MaCtdh).HasColumnName("MaCTDH");

                entity.Property(e => e.MaDh).HasColumnName("MaDH");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.HasOne(d => d.MaDhNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaDh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDon__MaDH__36B12243");

                entity.HasOne(d => d.MaSpNavigation)
                    .WithMany(p => p.ChiTietDonHangs)
                    .HasForeignKey(d => d.MaSp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ChiTietDon__MaSP__37A5467C");
            });

            modelBuilder.Entity<DanhMucSp>(entity =>
            {
                entity.HasKey(e => e.MaDm)
                    .HasName("PK__DanhMucS__2725866EE82E8DAD");

                entity.ToTable("DanhMucSP");

                entity.Property(e => e.MaDm).HasColumnName("MaDM");

                entity.Property(e => e.AnhDm).HasColumnName("AnhDM");

                entity.Property(e => e.MoTaDm)
                    .HasMaxLength(500)
                    .HasColumnName("MoTaDM");

                entity.Property(e => e.TenDm)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TenDM");
            });

            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.HasKey(e => e.MaDh)
                    .HasName("PK__DonHang__2725866101A47066");

                entity.ToTable("DonHang");

                entity.Property(e => e.MaDh).HasColumnName("MaDH");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.NgayTao).HasColumnType("date");

                entity.Property(e => e.NgayThanhToan).HasColumnType("date");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.DonHangs)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DonHang__MaKH__38996AB5");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KhachHan__2725CF1EED6DB3BA");

                entity.ToTable("KhachHang");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.AvatarKh).HasColumnName("AvatarKH");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Diachi).HasMaxLength(500);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.GioiTinh).HasMaxLength(10);

                entity.Property(e => e.Ngaysinh).HasColumnType("date");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(26)
                    .IsUnicode(false);

                entity.Property(e => e.TenKh)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TenKH");
            });

            //modelBuilder.Entity<QuanLyShipper>(entity =>
            //{
            //    entity.HasKey(e => e.MaShipper)
            //        .HasName("PK__QuanLySh__5C944AF62FC3612E");

            //    entity.ToTable("QuanLyShipper");

            //    entity.Property(e => e.MaDh).HasColumnName("MaDH");

            //    entity.Property(e => e.NgayLayHang).HasColumnType("date");

            //    entity.Property(e => e.TenCongty)
            //        .IsRequired()
            //        .HasMaxLength(100);

            //    entity.Property(e => e.TenShipper)
            //        .IsRequired()
            //        .HasMaxLength(100);

            //    entity.HasOne(d => d.MaDhNavigation)
            //        .WithMany(p => p.QuanLyShippers)
            //        .HasForeignKey(d => d.MaDh)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__QuanLyShip__MaDH__398D8EEE");
            //});

            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__RoleAcco__8AFACE3A7AB4B914");

                entity.ToTable("RoleAccount");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Mota)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SanPham>(entity =>
            {
                entity.HasKey(e => e.MaSp)
                    .HasName("PK__SanPham__2725081CBF3C0121");

                entity.ToTable("SanPham");

                entity.Property(e => e.MaSp).HasColumnName("MaSP");

                entity.Property(e => e.AnhSp)
                    .IsRequired()
                    .HasColumnName("AnhSP");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.GiaSp).HasColumnName("GiaSP");

                entity.Property(e => e.MaDm).HasColumnName("MaDM");

                entity.Property(e => e.MotaSp)
                    .IsRequired()
                    .HasColumnName("MotaSP");

                entity.Property(e => e.NgaySua).HasColumnType("date");

                entity.Property(e => e.TenSp)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TenSP");

                entity.Property(e => e.VideoSp).HasColumnName("VideoSP");

                entity.HasOne(d => d.MaDmNavigation)
                    .WithMany(p => p.SanPhams)
                    .HasForeignKey(d => d.MaDm)
                    .HasConstraintName("FK__SanPham__MaDM__3A81B327");
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.HasKey(e => e.MaTt)
                    .HasName("PK__TinTuc__27250079A6FB35AA");

                entity.ToTable("TinTuc");

                entity.Property(e => e.MaTt).HasColumnName("MaTT");

                entity.Property(e => e.AnhTt)
                    .IsRequired()
                    .HasColumnName("AnhTT");

                entity.Property(e => e.CreateDate).HasColumnType("date");

                entity.Property(e => e.Motadai).IsRequired();

                entity.Property(e => e.Motangan)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Tacgia)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TenTt)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("TenTT");
            });

            //modelBuilder.Entity<TrangThaiDh>(entity =>
            //{
            //    entity.HasKey(e => e.MaTtdh)
            //        .HasName("PK__TrangTha__4484B855C84735FE");

            //    entity.ToTable("TrangThaiDH");

            //    entity.Property(e => e.MaTtdh).HasColumnName("MaTTDH");

            //    entity.Property(e => e.MaDh).HasColumnName("MaDH");

            //    entity.Property(e => e.TrangThai)
            //        .IsRequired()
            //        .HasMaxLength(200);

            //    entity.HasOne(d => d.MaDhNavigation)
            //        .WithMany(p => p.TrangThaiDhs)
            //        .HasForeignKey(d => d.MaDh)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__TrangThaiD__MaDH__3B75D760");
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
