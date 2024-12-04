using Microsoft.EntityFrameworkCore;
using Productos.Models;

namespace Productos
{
    public class ProductosDBContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Producto> Productos { get; set; }

        public ProductosDBContext(DbContextOptions<ProductosDBContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Solo configurar si no se pasa `options` en el constructor
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=Productos.db"); // Cambiar según el tipo de base de datos
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad "Categoria"
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.id); // Clave primaria
                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.description).HasMaxLength(250);

                // Relación con Producto (1:N)
                entity.HasMany(c => c.Productos)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.categoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de la entidad "Empleado"
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Id); // Clave primaria
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);
            });

            // Configuración de la entidad "Producto"
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.id); // Clave primaria
                entity.Property(e => e.id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.description).HasMaxLength(250);
                entity.Property(e => e.price).IsRequired().HasColumnType("decimal(10, 2)");

                // Relación con Categoria (N:1)
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Productos)
                      .HasForeignKey(p => p.categoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Generar datos iniciales
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Datos iniciales para Categorías
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { id = 1, name = "Electrónica", description = "Dispositivos electrónicos" },
                new Categoria { id = 2, name = "Ropa", description = "Prendas de vestir" }
            );

            // Datos iniciales para Empleados
            modelBuilder.Entity<Empleado>().HasData(
                new Empleado { Id = 1, Nombre = "Juan", Apellido = "Perez", Correo = "juan@example.com", Password = "123456" },
                new Empleado { Id = 2, Nombre = "Ana", Apellido = "Lopez", Correo = "ana@example.com", Password = "abcdef" }
            );

            // Datos iniciales para Productos
            modelBuilder.Entity<Producto>().HasData(
                new Producto { id = 1, name = "Smartphone", description = "Teléfono inteligente de última generación", price = "0", categoryId = 1 },
                new Producto { id = 2, name = "Camisa", description = "Camisa de algodón para hombre", price = "0", categoryId = 2 }
            );
        }
    }
}
