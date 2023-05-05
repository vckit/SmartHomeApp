using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmartHomeApp.Models;

public partial class SmartHomeContext : DbContext
{
    public SmartHomeContext()
    {
    }

    public SmartHomeContext(DbContextOptions<SmartHomeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<DeviceDatum> DeviceData { get; set; }

    public virtual DbSet<DeviceModel> DeviceModels { get; set; }

    public virtual DbSet<DeviceStatus> DeviceStatuses { get; set; }

    public virtual DbSet<EventType> EventTypes { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<SecurityEvent> SecurityEvents { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDevicePermission> UserDevicePermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=ROOT\\ROOT;Database=dbSmartHome;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasName("PK__Actions__FFE3F4B90CF0E6ED");

            entity.Property(e => e.ActionId).HasColumnName("ActionID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ActionType).WithMany(p => p.Actions)
                .HasForeignKey(d => d.ActionTypeId)
                .HasConstraintName("FK__Actions__ActionT__4CA06362");

            entity.HasOne(d => d.Device).WithMany(p => p.Actions)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Actions__DeviceI__4AB81AF0");

            entity.HasOne(d => d.User).WithMany(p => p.Actions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Actions__UserID__4BAC3F29");
        });

        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.HasKey(e => e.ActionTypeId).HasName("PK__ActionTy__62FE4C04A484633C");

            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.ActionTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK__Devices__49E123316E3ED6D2");

            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.DeviceName).HasMaxLength(100);
            entity.Property(e => e.InstallationDate).HasColumnType("datetime");
            entity.Property(e => e.LastMaintenance).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.SubscriptionPeriod).HasDefaultValueSql("((0))");
            entity.Property(e => e.SubscriptionPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Model).WithMany(p => p.Devices)
                .HasForeignKey(d => d.ModelId)
                .HasConstraintName("FK__Devices__ModelID__412EB0B6");

            entity.HasOne(d => d.Status).WithMany(p => p.Devices)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Devices__StatusI__4222D4EF");
        });

        modelBuilder.Entity<DeviceDatum>(entity =>
        {
            entity.HasKey(e => e.DataId).HasName("PK__DeviceDa__9D05305DF9A4ED81");

            entity.Property(e => e.DataId).HasColumnName("DataID");
            entity.Property(e => e.DateRecorded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");

            entity.HasOne(d => d.Device).WithMany(p => p.DeviceData)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DeviceDat__Devic__44FF419A");
        });

        modelBuilder.Entity<DeviceModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK__DeviceMo__E8D7A1CCB398A257");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ModelName).HasMaxLength(50);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.DeviceModels)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK__DeviceMod__Manuf__3E52440B");
        });

        modelBuilder.Entity<DeviceStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__DeviceSt__C8EE2043D4B50BCB");

            entity.ToTable("DeviceStatus");

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.HasKey(e => e.EventTypeId).HasName("PK__EventTyp__A9216B1FC04F37EA");

            entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");
            entity.Property(e => e.EventTypeName).HasMaxLength(50);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__357E5CA1F6D759E7");

            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(50);
        });

        modelBuilder.Entity<SecurityEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Security__7944C8702EB2F70D");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.EventDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventTypeId).HasColumnName("EventTypeID");

            entity.HasOne(d => d.Device).WithMany(p => p.SecurityEvents)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__SecurityE__Devic__52593CB8");

            entity.HasOne(d => d.EventType).WithMany(p => p.SecurityEvents)
                .HasForeignKey(d => d.EventTypeId)
                .HasConstraintName("FK__SecurityE__Event__534D60F1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC4CAD64A6");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserDevicePermission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__UserDevi__EFA6FB0F3A92B92F");

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.DeviceId).HasColumnName("DeviceID");
            entity.Property(e => e.PermissionType).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Device).WithMany(p => p.UserDevicePermissions)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__UserDevic__Devic__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.UserDevicePermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserDevic__UserI__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
