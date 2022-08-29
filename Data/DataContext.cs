using Microsoft.EntityFrameworkCore;

namespace dotnet__rpg.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options):base (options)
        {
            
        }

        //Pluralisar nombre de entidad
        public DbSet<Character> Characters{get;set;}
    }
}