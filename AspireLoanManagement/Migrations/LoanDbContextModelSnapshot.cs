﻿// <auto-generated />
using System;
using AspireLoanManagement.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspireLoanManagement.Migrations
{
    [DbContext(typeof(LoanDbContext))]
    partial class LoanDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AspireLoanManagement.Repository.LoanModelDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("LoanAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("SettledDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Term")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("AspireLoanManagement.Repository.RepaymentModelDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("ActualRepaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpectedRepaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("LoanID")
                        .HasColumnType("int");

                    b.Property<decimal>("RepaymentAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LoanID");

                    b.ToTable("Repayments");
                });

            modelBuilder.Entity("AspireLoanManagement.Repository.RepaymentModelDTO", b =>
                {
                    b.HasOne("AspireLoanManagement.Repository.LoanModelDTO", "LoanModel")
                        .WithMany("Repayments")
                        .HasForeignKey("LoanID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoanModel");
                });

            modelBuilder.Entity("AspireLoanManagement.Repository.LoanModelDTO", b =>
                {
                    b.Navigation("Repayments");
                });
#pragma warning restore 612, 618
        }
    }
}
