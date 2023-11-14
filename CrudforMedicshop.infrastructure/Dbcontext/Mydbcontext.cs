using CrudforMedicshop.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Dbcontext
{
    public class Mydbcontext : DbContext
    {
       
        public Mydbcontext()
        {

        }
        public Mydbcontext(DbContextOptions<Mydbcontext> options) : base(options)
        {
        }
        public DbSet<Medicine> Medicines { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=::1;Port=5432;Database=Hello;user id=postgres;password=123456");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
