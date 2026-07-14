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
        public DbSet<Central> Centrals { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<PosicaoVeiculo> PosicaoVeiculos { get; set; }
        public DbSet<RastreadorModulo> Rastreadors { get; set; }
    }
}
