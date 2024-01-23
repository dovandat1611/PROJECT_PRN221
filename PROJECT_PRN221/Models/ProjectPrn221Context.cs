using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PROJECT_PRN221.Models;

public partial class ProjectPrn221Context : DbContext
{
    public ProjectPrn221Context()
    {
    }

    public ProjectPrn221Context(DbContextOptions<ProjectPrn221Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsGroup> NewsGroups { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Warranty> Warranties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MyCnn"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Gmail)
                .HasMaxLength(250)
                .HasColumnName("gmail");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(250)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.BrandName)
                .HasMaxLength(250)
                .HasColumnName("brand_name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(250)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentContent)
                .HasColumnType("text")
                .HasColumnName("comment_content");
            entity.Property(e => e.CommentDate)
                .HasColumnType("date")
                .HasColumnName("comment_date");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Comment_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Comment_Product");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.ContactContent)
                .HasColumnType("text")
                .HasColumnName("contact_content");
            entity.Property(e => e.ContactDate)
                .HasColumnType("date")
                .HasColumnName("contact_date");
            entity.Property(e => e.Gmail)
                .HasMaxLength(250)
                .HasColumnName("gmail");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .HasColumnName("address");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(250)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Username)
                .HasMaxLength(250)
                .HasColumnName("username");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.Property(e => e.NewsId).HasColumnName("news_id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("date")
                .HasColumnName("created_date");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.NewsgroupId).HasColumnName("newsgroup_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .HasColumnName("title");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.News)
                .HasForeignKey(d => d.Createdby)
                .HasConstraintName("FK_News_Admin");

            entity.HasOne(d => d.Newsgroup).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsgroupId)
                .HasConstraintName("FK_News_News_group");
        });

        modelBuilder.Entity<NewsGroup>(entity =>
        {
            entity.ToTable("News_group");

            entity.Property(e => e.NewsgroupId).HasColumnName("newsgroup_id");
            entity.Property(e => e.NewsgroupName)
                .HasMaxLength(250)
                .HasColumnName("newsgroup_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.AddressReceiver)
                .HasMaxLength(250)
                .HasColumnName("address_receiver");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.NameReceiver)
                .HasMaxLength(250)
                .HasColumnName("name_receiver");
            entity.Property(e => e.OderDate)
                .HasColumnType("date")
                .HasColumnName("oder_date");
            entity.Property(e => e.Payment)
                .HasMaxLength(250)
                .HasColumnName("payment");
            entity.Property(e => e.PhoneReceiver)
                .HasMaxLength(250)
                .HasColumnName("phone_receiver");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("Order_detail");

            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.ListPrice).HasColumnName("list_price");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QuantityOrder).HasColumnName("quantity_order");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Order_detail_Order");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.ListPrice).HasColumnName("list_price");
            entity.Property(e => e.ProductName)
                .HasMaxLength(250)
                .HasColumnName("product_name");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.SubDescription)
                .HasColumnType("text")
                .HasColumnName("sub_description");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Product_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Warranty>(entity =>
        {
            entity.ToTable("Warranty");

            entity.Property(e => e.WarrantyId).HasColumnName("warranty_id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ImageProduct)
                .HasMaxLength(250)
                .HasColumnName("image_product");
            entity.Property(e => e.ImageProductAdmin)
                .HasMaxLength(250)
                .HasColumnName("image_product_admin");
            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductStatus)
                .HasMaxLength(50)
                .HasColumnName("product_status");
            entity.Property(e => e.ProductStatusAdmin)
                .HasMaxLength(250)
                .HasColumnName("product_status_admin");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.WarrantyDate)
                .HasColumnType("date")
                .HasColumnName("warranty_date");
            entity.Property(e => e.WarrantyDateAdmin)
                .HasMaxLength(250)
                .HasColumnName("warranty_date_admin");
            entity.Property(e => e.WarrantyQuantity).HasColumnName("warranty_quantity");
            entity.Property(e => e.WarrantyStatus)
                .HasMaxLength(50)
                .HasColumnName("warranty_status");

            entity.HasOne(d => d.Customer).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Warranty_Customer");

            entity.HasOne(d => d.Orderdetail).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.OrderdetailId)
                .HasConstraintName("FK_Warranty_Order_detail");

            entity.HasOne(d => d.Product).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Warranty_Product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
