using Microsoft.EntityFrameworkCore;
using teste_WebMotors.Models;

namespace teste_WebMotors.Models.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<teste_WebMotors.Models.AnunciosDTO> Anuncios { get; set; }
    }
}
