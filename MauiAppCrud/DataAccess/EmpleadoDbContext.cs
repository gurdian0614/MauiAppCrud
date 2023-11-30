using MauiAppCrud.Models;
using MauiAppCrud.Utilties;
using Microsoft.EntityFrameworkCore;

namespace MauiAppCrud.DataAccess
{
    public class EmpleadoDbContext : DbContext
    {
        public DbSet<Empleado> EmpleadoSet { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexion = $"FileName={Conexion.ObtenerRuta("empleado.db")}";
            optionsBuilder.UseSqlite(conexion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
