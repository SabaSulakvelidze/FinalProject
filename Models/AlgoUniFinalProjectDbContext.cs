using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Models;

public partial class AlgoUniFinalProjectDbContext : DbContext
{
    public AlgoUniFinalProjectDbContext()
    {
    }

    public AlgoUniFinalProjectDbContext(DbContextOptions<AlgoUniFinalProjectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionsForUser> PermissionsForUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AlgoUniFinalProjectDb;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07F8B8C5C4");

            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermissionsForUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PermissionId }).HasName("PK__Permissi__F972A3FEE22E910C");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Permission).WithMany(p => p.PermissionsForUsers)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__Permi__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.PermissionsForUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__UserI__4F7CD00D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07A1107945");

            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
