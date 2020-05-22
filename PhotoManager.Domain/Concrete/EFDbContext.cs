using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoManager.Domain.Entities;
using System.Data.Entity;

namespace PhotoManager.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }

    }
}
