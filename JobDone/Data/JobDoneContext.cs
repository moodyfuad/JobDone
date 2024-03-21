using System;
using System.Collections.Generic;
using JobDone.Models;
using JobDone.Models.Admin;
using JobDone.Models.Category;
using JobDone.Models.Customer;
using JobDone.Models.Order;
using JobDone.Models.OrderByCustomer;
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

}
