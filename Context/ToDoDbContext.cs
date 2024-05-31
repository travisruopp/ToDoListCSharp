using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Context
{
    public class ToDoDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public ToDoDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            if(connectionString is not null)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<ToDoList.Models.ToDoModel> ToDoModel { get; set; } = default!;
    }
}
