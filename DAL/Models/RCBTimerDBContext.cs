using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RCBTimerDBContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Workday> Workday { get; set; }
        public DbSet<Break> Break { get; set; }
    }
}
