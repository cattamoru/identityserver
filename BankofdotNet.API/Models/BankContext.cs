using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankofdotNet.API.Models
{
    public class BankContext : DbContext
    {
        //override di dbcontext
        public BankContext(DbContextOptions<BankContext> options) :base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
