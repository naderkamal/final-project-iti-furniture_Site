using AdminDashboardMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDashboardMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductOrder> ProductOrders { get; set; }
        public virtual DbSet<ProductUser> ProductUsers { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Shipment> Shipments { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).UseIdentityColumn()
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .HasColumnName("ID");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("Order_Date");

                entity.Property(e => e.Statue)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Total_Price");
                entity.Property(e => e.ShipmentID).HasColumnName("Shipment_ID");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Shipment)
                  .WithMany(p => p.Orders)
                  .HasForeignKey(p => p.ShipmentID)
                  .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.UserID).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                 .WithMany(p => p.Orders)
                 .HasForeignKey(p => p.UserID)
                 .OnDelete(DeleteBehavior.Cascade);



            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .HasColumnName("ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");
                entity.Property(e => e.CategoryID).HasColumnName("Category_ID");

                entity.HasOne(d => d.Category)
                 .WithMany(p => p.Products)
                 .HasForeignKey(p => p.CategoryID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductOrder>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PK__Product___48672C22AA1C68C5");

                entity.ToTable("Product_Order");

                entity.Property(e => e.OrderId).HasColumnName("Order_ID");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.ItemPrice)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Item_Price");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Product_O__Order__4316F928");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductOrders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Product_O__Produ__440B1D61");
            });

            modelBuilder.Entity<ProductUser>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.ProductId })
                    .HasName("PK__Product___99EEDE29E989E9BC");

                entity.ToTable("Product_User");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.RatingStar).HasColumnName("Rating_Star");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductUsers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Product_U__Produ__4E88ABD4");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProductUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Product_U__User___4D94879B");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .HasColumnName("ID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");
                entity.Property(e => e.UserID).HasColumnName("User_ID");

                entity.HasOne(d => d.Users)
                 .WithMany(p => p.Reviews)
                 .HasForeignKey(p => p.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
            });





            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("Shipment");

                entity.Property(e => e.Id)
                    .UseIdentityColumn()
                    .HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IsCompleted).HasColumnName("Is_Completed");
                entity.Property(e => e.moreDetails)
                    .HasMaxLength(200)
                    .IsUnicode(false);


            });

            modelBuilder.Entity<User>(entity =>
            {
               

                entity.Property(e => e.Street)
                            .HasMaxLength(100)
                            .IsUnicode(false);
                entity.Property(e => e.City)
                            .HasMaxLength(100)
                            .IsUnicode(false);
                entity.Property(e => e.Governorate)
                            .HasMaxLength(100)
                            .IsUnicode(false);

                entity.Property(e => e.FirstName)
                            .HasMaxLength(50)
                            .IsUnicode(false)
                            .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Last_Name");

               
                  



                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false);



                entity.Property(e => e.Ssn)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SSN");

            });

        }
    }
}
