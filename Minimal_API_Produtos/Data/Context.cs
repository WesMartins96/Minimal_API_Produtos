using Microsoft.EntityFrameworkCore;
using Minimal_API_Produtos.Models;

namespace Minimal_API_Produtos.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
    }
}
