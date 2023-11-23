using CrudforMedicshop.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudforMedicshop.infrastructure.Dbcontext
{
    public class ApplicationDbcontext1 :DbContext
    {
        public ApplicationDbcontext1(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Medicine> Medicines1 { get; set; }
        public DbSet<ApplicationUser1> UserforRefresh { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
