using CrudforMedicshop.Domain.Entities;
using CrudforMedicshop.Domain.Models;
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
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
