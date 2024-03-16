using System;
using System.Collections.Generic;
using JobDone.Models;
using JobDone.Models.Admin;
using JobDone.Models.Customer;
using JobDone.Models.SecurityQuestions;
using Microsoft.EntityFrameworkCore;

namespace JobDone.Data;

public partial class JobDoneContext : DbContext
{


    public JobDoneContext(DbContextOptions<JobDoneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminModel> AdminModels { get; set; }

    public virtual DbSet<AdminWalletModel> AdminWalletModels { get; set; }

    public virtual DbSet<BannerModel> BannerModels { get; set; }

    public virtual DbSet<CategoryModel> CategoryModels { get; set; }

    public virtual DbSet<CustomerModel> CustomerModels { get; set; }

    public virtual DbSet<MessageModel> MessageModels { get; set; }

    public virtual DbSet<OrderByCustomerModel> OrderByCustomerModels { get; set; }

    public virtual DbSet<OrderModel> OrderModels { get; set; }

    public virtual DbSet<SecurityQuestionModel> SecurityQuestionModels { get; set; }

    public virtual DbSet<SellerAcceptRequestModel> SellerAcceptRequestModels { get; set; }

    public virtual DbSet<SellerModel> SellerModels { get; set; }

    public virtual DbSet<SellerOldWorkModel> SellerOldWorkModels { get; set; }

    public virtual DbSet<ServiceModel> ServiceModels { get; set; }

    public virtual DbSet<WithdrawModel> WithdrawModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=HP-LAB\\MSSQLSERVER02;initial catalog=JobDone; database=JobDone; trusted_connection=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminModel>(entity =>
        {
            entity.HasOne(d => d.WalletIdFkNavigation).WithMany(p => p.AdminModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminModel_AdminWalletModel");
        });

        modelBuilder.Entity<CustomerModel>(entity =>
        {
            entity.HasOne(d => d.SecurityQuestionIdFkNavigation).WithMany(p => p.CustomerModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerModel_SecurityQuestionModel");
        });

        modelBuilder.Entity<MessageModel>(entity =>
        {
            entity.HasOne(d => d.OrderIdFkNavigation).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MessageModel_OrderModel");
        });

        modelBuilder.Entity<OrderByCustomerModel>(entity =>
        {
            entity.HasOne(d => d.CategoryIdKfNavigation).WithMany(p => p.OrderByCustomerModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderByCustomerModel_CategoryModel");

            entity.HasOne(d => d.CustomerIdFkNavigation).WithMany(p => p.OrderByCustomerModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderByCustomerModel_CustomerModel");
        });

        modelBuilder.Entity<OrderModel>(entity =>
        {
            entity.HasOne(d => d.CategoryIdKfNavigation).WithMany(p => p.OrderModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderModel_CategoryModel");

            entity.HasOne(d => d.CustomerIdFkNavigation).WithMany(p => p.OrderModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderModel_CustomerModel");

            entity.HasOne(d => d.SellerIdFkNavigation).WithMany(p => p.OrderModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderModel_SellerModel");
        });

        modelBuilder.Entity<SellerAcceptRequestModel>(entity =>
        {
            entity.HasOne(d => d.OrderByCustomerIdFkNavigation).WithMany(p => p.SellerAcceptRequestModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerAcceptRequestModel_OrderByCustomerModel1");

            entity.HasOne(d => d.SellerIdFkNavigation).WithMany(p => p.SellerAcceptRequestModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerAcceptRequestModel_SellerModel");
        });

        modelBuilder.Entity<SellerModel>(entity =>
        {
            entity.HasOne(d => d.CategoryIdFkNavigation).WithMany(p => p.SellerModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerModel_CategoryModel");

            entity.HasOne(d => d.SecurityQuestionIdFkNavigation).WithMany(p => p.SellerModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerModel_SecurityQuestionModel");
        });

        modelBuilder.Entity<SellerOldWorkModel>(entity =>
        {
            entity.HasOne(d => d.SellerIdFkNavigation).WithMany(p => p.SellerOldWorkModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SellerOldWorkModel_SellerModel");
        });

        modelBuilder.Entity<ServiceModel>(entity =>
        {
            entity.HasOne(d => d.SellerIdFkNavigation).WithMany(p => p.ServiceModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceModel_SellerModel");
        });

        modelBuilder.Entity<WithdrawModel>(entity =>
        {
            entity.HasOne(d => d.SellerIdFkNavigation).WithMany(p => p.WithdrawModels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WithdrawModel_SellerModel");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
