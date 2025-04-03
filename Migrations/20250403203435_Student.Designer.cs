﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace projektBanka.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250403203435_Student")]
    partial class Student
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<double>("Money")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Account");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("LogMsg", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Command")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Logs", (string)null);
                });

            modelBuilder.Entity("Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<Guid>("DestinationAccountId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PayerAccountId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DestinationAccountId");

                    b.HasIndex("PayerAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Access")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Phone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CommonAccount", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CommonAccounts", (string)null);
                });

            modelBuilder.Entity("CreditAccount", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<double>("Ceiling")
                        .HasColumnType("REAL");

                    b.Property<double>("ComputedMinus")
                        .HasColumnType("REAL");

                    b.Property<double>("Interest")
                        .HasColumnType("REAL");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CreditAccounts", (string)null);
                });

            modelBuilder.Entity("SavingsAccount", b =>
                {
                    b.HasBaseType("Account");

                    b.Property<double>("ComputedBonus")
                        .HasColumnType("REAL");

                    b.Property<double>("Interest")
                        .HasColumnType("REAL");

                    b.Property<bool>("IsStudent")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("SavingsAccounts", (string)null);
                });

            modelBuilder.Entity("Transaction", b =>
                {
                    b.HasOne("Account", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("DestinationAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Account", "PayerAccount")
                        .WithMany()
                        .HasForeignKey("PayerAccountId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("DestinationAccount");

                    b.Navigation("PayerAccount");
                });

            modelBuilder.Entity("CommonAccount", b =>
                {
                    b.HasOne("Account", null)
                        .WithOne()
                        .HasForeignKey("CommonAccount", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "Owner")
                        .WithOne("CommonAccount")
                        .HasForeignKey("CommonAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("CreditAccount", b =>
                {
                    b.HasOne("Account", null)
                        .WithOne()
                        .HasForeignKey("CreditAccount", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "Owner")
                        .WithOne("CreditAccount")
                        .HasForeignKey("CreditAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SavingsAccount", b =>
                {
                    b.HasOne("Account", null)
                        .WithOne()
                        .HasForeignKey("SavingsAccount", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("User", "Owner")
                        .WithOne("SavingsAccount")
                        .HasForeignKey("SavingsAccount", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("CommonAccount");

                    b.Navigation("CreditAccount");

                    b.Navigation("SavingsAccount");
                });
#pragma warning restore 612, 618
        }
    }
}
