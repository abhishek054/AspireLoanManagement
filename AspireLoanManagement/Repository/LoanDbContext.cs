﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AspireLoanManagement.Repository
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options)
        {
        }

        public DbSet<LoanModelDTO> Loans { get; set; }
        public DbSet<RepaymentModelDTO> Repayments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoanModelDTO>()
            .HasMany(l => l.Repayments)
            .WithOne(r => r.LoanModelDTO)
            .HasForeignKey(r => r.LoanID);
        }
    }
}
