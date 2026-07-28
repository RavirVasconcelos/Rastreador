using Microsoft.EntityFrameworkCore;
using Rastreador.Models;

namespace Rastreador.Context
{
    public class RastreadorContext : DbContext
    {
        public RastreadorContext(DbContextOptions<RastreadorContext> options) : base(options)
        {
            
        }

        public DbSet<Ancoragem> Ancoragems { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<PosicaoVeiculo> PosicaoVeiculos { get; set; }
        public DbSet<RastreadorModulo> Rastreadors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cliente -> Veículos (1:N)
            modelBuilder.Entity<Cliente>()
                .HasMany(c => c.Veiculos)
                .WithOne(v => v.Cliente)
                .HasForeignKey(v => v.ClienteId);

            // Veículo -> Rastreador (1:1)
            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Rastreador)
                .WithOne(r => r.Veiculo)
                .HasForeignKey<RastreadorModulo>(r => r.VeiculoId);

            // Veículo -> Ancoragem (1:1)
            modelBuilder.Entity<Veiculo>()
                .HasOne(v => v.Ancoragem)
                .WithOne(a => a.Veiculo)
                .HasForeignKey<Ancoragem>(a => a.VeiculoId);

            modelBuilder.Entity<RastreadorModulo>()
                .HasKey(r => r.RastreadorId);

            // Rastreador -> Posições (1:N)
            modelBuilder.Entity<RastreadorModulo>()
                .HasMany(r => r.Posicoes)
                .WithOne(p => p.RastreadorModulo)
                .HasForeignKey(p => p.RastreadorModuloId);
        }
    }
}
