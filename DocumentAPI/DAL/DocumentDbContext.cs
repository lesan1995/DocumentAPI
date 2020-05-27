using DocumentAPI.DAL.EntityConfigurations;
using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.DAL
{
    public class DocumentDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentEntityTypeConfiguration());
        }

        public DocumentDbContext()
        {

        }

        public DocumentDbContext(DbContextOptions<DocumentDbContext> options) : base(options)
        {

        }
    }
}
