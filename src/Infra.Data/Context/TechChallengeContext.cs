using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Infra.Data.Context
{
    [ExcludeFromCodeCoverage]
    public sealed class TechChallengeContext : DbContext
    {         
        public TechChallengeContext(DbContextOptions<TechChallengeContext> options)
            : base(options)
        {
        }
     
        public DbSet<Pedido> Pedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());         
        }
    }
}
