using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options)
        {

        }
        public DbSet<DNSEntry> Entries { get; set; }

        //Add some data to the database on startup
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DNSEntry>()
                .HasData(
                    new DNSEntry { Id = 1, HostName = "www.google.com", TTL = 43200, Type = "A", Value = "123.123.123.123"}
                );
        }
    }

    
}
