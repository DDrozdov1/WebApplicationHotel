using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebApplicationHotelDB_Lab3.Models;

namespace WebApplicationHotel.Data
{
    public partial class HotelDBContext : DbContext
    {
        public HotelDBContext()
        {
        }

        public HotelDBContext(DbContextOptions<HotelDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<ClientService> ClientServices { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<HotelService> HotelServices { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomPrice> RoomPrices { get; set; } = null!;
        public virtual DbSet<RoomService> RoomServices { get; set; } = null!;
        public virtual DbSet<RoomsWithPrice> RoomsWithPrices { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            IConfigurationRoot config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");
            _ = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).ValueGeneratedNever();

                entity.Property(e => e.CheckInDate).HasColumnType("date");

                entity.Property(e => e.CheckOutDate).HasColumnType("date");

                entity.Property(e => e.ClientFullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClientPassportDetails)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__Clients__RoomId__18EBB532");
            });

            modelBuilder.Entity<ClientService>(entity =>
            {
                entity.Property(e => e.ClientServiceId).ValueGeneratedNever();

                entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK__ClientSer__Clien__14270015");

                entity.HasOne(d => d.HotelService)
                    .WithMany(p => p.ClientServices)
                    .HasForeignKey(d => d.HotelServiceId)
                    .HasConstraintName("FK__ClientSer__Hotel__151B244E");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.EmployeeFullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeePosition)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HotelService>(entity =>
            {
                entity.Property(e => e.HotelServiceid).ValueGeneratedNever();

                entity.Property(e => e.HotelServiceCost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.HotelServiceDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HotelServiceName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).ValueGeneratedNever();

                entity.Property(e => e.RoomDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoomType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RoomPrice>(entity =>
            {
                entity.Property(e => e.RoomPriceId).ValueGeneratedNever();

                entity.Property(e => e.RoomCost).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomPrices)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomPrice__RoomI__17F790F9");
            });

            modelBuilder.Entity<RoomService>(entity =>
            {
                entity.ToTable("RoomService");

                entity.Property(e => e.RoomServiceId).ValueGeneratedNever();

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.RoomServices)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__RoomServi__Emplo__17036CC0");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomServices)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__RoomServi__RoomI__160F4887");
            });

            modelBuilder.Entity<RoomsWithPrice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("RoomsWithPrices");

                entity.Property(e => e.RoomCost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.RoomDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoomType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
