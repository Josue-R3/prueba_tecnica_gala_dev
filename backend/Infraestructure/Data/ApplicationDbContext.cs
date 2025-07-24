using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<Tienda> Tiendas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuraciones para Tienda
        modelBuilder.Entity<Tienda>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.HasIndex(e => e.Estado);
        });

        // Configuraciones para Empleado
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Apellido)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Correo)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Cargo)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FechaIngreso)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Correo).IsUnique();
            entity.HasIndex(e => e.TiendaId);
            entity.HasIndex(e => e.Estado);

            entity.HasOne(e => e.Tienda)
                .WithMany(t => t.Empleados)
                .HasForeignKey(e => e.TiendaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuraciones para Rol
        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Nombre).IsUnique();

            // Datos semilla
            entity.HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Manager" },
                new Rol { Id = 3, Nombre = "Empleado" }
            );
        });

        // Configuraciones para Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UsuarioName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Usuario");
            entity.Property(e => e.Contrasenia)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Estado)
                .IsRequired()
                .HasDefaultValue(true);
            entity.Property(e => e.FechaCreado)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            entity.HasIndex(e => e.UsuarioName).IsUnique();
            entity.HasIndex(e => e.RolId);
            entity.HasIndex(e => e.EmpleadoId);

            entity.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.Empleado)
                .WithMany(e => e.Usuarios)
                .HasForeignKey(u => u.EmpleadoId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
