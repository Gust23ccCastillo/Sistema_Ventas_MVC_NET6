using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Entities;

namespace SistemaVentas.Dal
{
    public partial class DataBase_Application_Sales_MVCContext : DbContext
    {
        public DataBase_Application_Sales_MVCContext()
        {
        }

        public DataBase_Application_Sales_MVCContext(DbContextOptions<DataBase_Application_Sales_MVCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Business> Businesses { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ConfigurationApplication> ConfigurationApplications { get; set; } = null!;
        public virtual DbSet<CorrelativeNumber> CorrelativeNumbers { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Rol> Rols { get; set; } = null!;
        public virtual DbSet<RolMenu> RolMenus { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<SaleDetail> SaleDetails { get; set; } = null!;
        public virtual DbSet<TypeOfDocumentSale> TypeOfDocumentSales { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.IdBusiness)
                    .HasName("PK__Business__9C92AEE52C1FC218");

                entity.ToTable("Business");

                entity.Property(e => e.IdBusiness).ValueGeneratedNever();

                entity.Property(e => e.AddressBusiness)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Coin)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LogoName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NameBusiness)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PercentageTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UrlLogo)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__Category__A3C02A100F3337A6");

                entity.ToTable("Category");

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ConfigurationApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ConfigurationApplication");

                entity.Property(e => e.Property)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResourceConfig)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Resource_Config");

                entity.Property(e => e.ValueConfig)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("Value_Config");
            });

            modelBuilder.Entity<CorrelativeNumber>(entity =>
            {
                entity.HasKey(e => e.IdCorrelativeNumber)
                    .HasName("PK__Correlat__FFD1136D71CB9EB3");

                entity.ToTable("CorrelativeNumber");

                entity.Property(e => e.DateOfUpdate).HasColumnType("datetime");

                entity.Property(e => e.Gestion)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.IdMenu)
                    .HasName("PK__Menu__4D7EA8E1EBB2190C");

                entity.ToTable("Menu");

                entity.Property(e => e.ActionPage)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Controller)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Icon)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdMenuFatherNavigation)
                    .WithMany(p => p.InverseIdMenuFatherNavigation)
                    .HasForeignKey(d => d.IdMenuFather)
                    .HasConstraintName("FK__Menu__IdMenuFath__36B12243");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.IdProduct)
                    .HasName("PK__Products__2E8946D44E36592B");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mark)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NameImage)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UrlImage)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__Products__IdCate__48CFD27E");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__Rol__2A49584C0D314B9A");

                entity.ToTable("Rol");

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<RolMenu>(entity =>
            {
                entity.HasKey(e => e.IdRolMenu)
                    .HasName("PK__RolMenu__79F10105DA76B072");

                entity.ToTable("RolMenu");

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.IdMenuNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdMenu)
                    .HasConstraintName("FK__RolMenu__IdMenu__3E52440B");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RolMenus)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__RolMenu__IdRol__3D5E1FD2");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.HasKey(e => e.IdSale)
                    .HasName("PK__Sales__A04F9B37800F9CAD");

                entity.Property(e => e.ClientDocument)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ClientName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SalesNumber)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TotalTax).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdTypeOfDocumentSaleNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.IdTypeOfDocumentSale)
                    .HasConstraintName("FK__Sales__IdTypeOfD__5165187F");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__Sales__IdUser__52593CB8");
            });

            modelBuilder.Entity<SaleDetail>(entity =>
            {
                entity.HasKey(e => e.IdSaleDetail)
                    .HasName("PK__SaleDeta__A98F222AA43B892C");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductMark)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.IdSaleNavigation)
                    .WithMany(p => p.SaleDetails)
                    .HasForeignKey(d => d.IdSale)
                    .HasConstraintName("FK__SaleDetai__IdSal__5629CD9C");
            });

            modelBuilder.Entity<TypeOfDocumentSale>(entity =>
            {
                entity.HasKey(e => e.IdTypeOfDocumentSale)
                    .HasName("PK__TypeOfDo__45E7DE5775CAE349");

                entity.ToTable("TypeOfDocumentSale");

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PK__Users__B7C92638571F4380");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NamePhoto)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegistrationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UrlPhoto)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRol)
                    .HasConstraintName("FK__Users__IdRol__4222D4EF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
