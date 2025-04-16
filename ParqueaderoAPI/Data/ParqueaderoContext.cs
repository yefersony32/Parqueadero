using Microsoft.EntityFrameworkCore;
using ParqueaderoAPI.Models;

namespace ParqueaderoAPI.Data
{
    public class ParqueaderoContext : DbContext
    {
        public ParqueaderoContext(DbContextOptions<ParqueaderoContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Espacio> Espacios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehiculo>()
               .HasOne(v => v.Cliente)
               .WithMany()
               .HasForeignKey(v => v.ClienteID)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Vehiculo)
                .WithMany()
                .HasForeignKey(r => r.VehiculoID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Espacio)
                .WithMany()
                .HasForeignKey(r => r.EspacioID)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Monto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");
        }
    }


}
