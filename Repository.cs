using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EFCorePerformanceTest.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCorePerformanceTest
{
    public class Repository : DbContext
    {
        private readonly string _connectionString;
        public Repository(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ExampleTable> ExampleTable { get; set; }
    }
}
