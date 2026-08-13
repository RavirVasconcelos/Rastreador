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
        public DbSet<SonoRastreador> SonoRastreadors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- RELACIONAMENTOS ---

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

            // Rastreador -> Posição Atual (1:1)
            modelBuilder.Entity<RastreadorModulo>()
                .HasOne(r => r.PosicaoAtual)
                .WithOne(p => p.RastreadorModulo)
                .HasForeignKey<PosicaoVeiculo>(p => p.RastreadorModuloId);

            // Rastreador -> Sono (1:1)
            modelBuilder.Entity<RastreadorModulo>()
                .HasOne(r => r.SonoRastreador)
                .WithOne(s => s.Rastreador)
                .HasForeignKey<SonoRastreador>(s => s.RastreadorId);

            modelBuilder.Entity<SonoRastreador>()
                .HasKey(s => s.SonoId);
        }
    }
}