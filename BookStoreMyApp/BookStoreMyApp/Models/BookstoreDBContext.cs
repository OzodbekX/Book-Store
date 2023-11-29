using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStoreMyApp.Models
{
    public partial class BookstoreDBContext : DbContext
    {
        public BookstoreDBContext()
        {
        }

        public BookstoreDBContext(DbContextOptions<BookstoreDBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Shipping> Shippings { get; set; } = null!;
        public virtual DbSet<Picture> Pictures { get; set; } = null!;
        public virtual DbSet<BookAuthor> BookAuthors { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Sale> Sales { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<UserResult> UserResults { get; set; } = null!;
        public virtual DbSet<UserTask> UserTasks { get; set; } = null!;
        public virtual DbSet<TaskFile> TaskFiles { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.ToTable("Picture");
                entity.Property(e => e.Id).HasColumnName(@"id");
                entity.Property(e => e.Size).HasColumnName("size");
                entity.Property(e => e.Bytes).HasColumnName("bytes");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.FileExtension).HasColumnName("fileExtension");

            }); 

            modelBuilder.Entity<TaskFile>(entity =>
            {
                entity.ToTable("TaskFile");
                entity.Property(e => e.FileId).HasColumnName(@"file_id");
                entity.Property(e => e.Size).HasColumnName("size");
                entity.Property(e => e.Bytes).HasColumnName("bytes");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.FileExtension).HasColumnName("fileExtension");
                entity.Property(e => e.FileExtension).HasColumnName("fileExtension");

            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email_address");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .HasColumnName("phone")
                    .HasDefaultValueSql("('UNKNOWN')");

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("state");

                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Option1)
                    .HasColumnName("option1");
                entity.Property(e => e.Option2)
                    .HasColumnName("option2");
                entity.Property(e => e.Option3)
                    .HasColumnName("option3");
                entity.Property(e => e.TaskAddress).HasColumnName("task_address");

                entity.Property(e => e.QuestionText)
                    .IsUnicode(false)
                    .HasColumnName("question_text");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer");

                entity.Property(e => e.Quizball)
                    .HasColumnName("quis_ball");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.Advance)
                    .HasColumnType("money")
                    .HasColumnName("advance");

                entity.Property(e => e.Notes)
                    .IsUnicode(false)
                    .HasColumnName("notes");

                entity.Property(e => e.Rating)
                    .HasColumnName("rating").HasDefaultValue(1);

                entity.Property(e => e.Price)
                    .HasColumnType("money")
                    .HasColumnName("price");

                entity.Property(e => e.Discount)
                    .HasColumnName("discount")
                    .HasDefaultValueSql(null);
                entity.Property(e => e.Vat)
                    .HasColumnName("vat")
                    .HasDefaultValueSql(null);

                entity.Property(e => e.PubId).HasColumnName("pub_id");

                entity.Property(e => e.PublishedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("published_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Royalty).HasColumnName("royalty");

                entity.Property(e => e.Title)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Type)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("type")
                    .HasDefaultValueSql("('UNDECIDED')");

                entity.Property(e => e.YtdSales).HasColumnName("ytd_sales");

                entity.HasOne(d => d.Pub)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PubId)
                    .HasConstraintName("FK__Book__pub_id__3D5E1FD2");
            });

            modelBuilder.Entity<UserResult>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.UserTaskId });
                entity.ToTable("UserResult");
                entity.Property(e => e.TaskType).HasColumnName("task_taype");
                entity.Property(e => e.Result).HasColumnName("result");
                entity.HasOne(d=>d.User).
                     WithMany(p => p.UserResults)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__user_id__book___3F466844");
            });    
            modelBuilder.Entity<UserTask>(entity =>
            {
                entity.HasKey(e => new { e.UserTaskId});

                entity.ToTable("UserTask");

                entity.Property(e => e.UserTaskTittle).HasColumnName("user_task_tittle");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.TaskAddress).HasColumnName("task_address");

            });


            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(e => new { e.AuthorId, e.BookId });

                entity.ToTable("BookAuthor");

                entity.Property(e => e.AuthorId).HasColumnName("author_id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.AuthorOrder).HasColumnName("author_order");

                entity.Property(e => e.RoyaltyPercentage).HasColumnName("royalty_percentage");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__BookAutho__autho__3E52440B");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookAuthors)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__BookAutho__book___3F466844");

            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job");

                entity.Property(e => e.JobId).HasColumnName("job_id");

                entity.Property(e => e.JobDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("job_desc")
                    .HasDefaultValueSql("('New Position - title not formalized yet')");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.PubId)
                    .HasName("PK__Publishe__2515F2220B4D3120");

                entity.ToTable("Publisher");

                entity.Property(e => e.PubId).HasColumnName("pub_id");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country")
                    .HasDefaultValueSql("('USA')");

                entity.Property(e => e.PublisherName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("publisher_name");

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("state");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.TokenId);

                entity.ToTable("RefreshToken");

                entity.Property(e => e.TokenId).HasColumnName("token_id");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expiry_date");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("admin_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__RefreshTo__admin___403A8C7D");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role_name")
                    .HasDefaultValueSql("('New Position - title not formalized yet')");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");

                entity.Property(e => e.SaleId).HasColumnName("sale_id");

                entity.Property(e => e.BookId).HasColumnName("book_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date");

                entity.Property(e => e.OrderNum)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("order_num");

                entity.Property(e => e.SpecialDiscount)
                    .HasMaxLength(20)
                    .HasDefaultValue(0)
                    .IsUnicode(false)
                    .HasColumnName("special_discounts");

                entity.Property(e => e.PayTerms)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("pay_terms");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.StoreId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("store_id");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Sale__book_id__412EB0B6");

                entity.HasOne(d => d.Shipping)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.ShippingId)
                    .HasConstraintName("FK__Sale__shipping_id__412EB0B6");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.StoreId)
                    .HasConstraintName("FK__Sale__store_id__4222D4EF");
            });
            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.ToTable("Shipping");

                entity.Property(e => e.ShippingId).HasColumnName("shipping_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .HasColumnName("address");

                entity.Property(e => e.ShippingState)
                    .IsUnicode(false)
                    .HasColumnName("shipping_state");
                entity.Property(e => e.PostalCode)
                    .IsUnicode(false)
                     .HasMaxLength(6)
                    .HasColumnName("postal_code");
            });


            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.StoreId)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("store_id");

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.State)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("state");

                entity.Property(e => e.StoreAddress)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("store_address");

                entity.Property(e => e.StoreName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("store_name");

                entity.Property(e => e.Zip)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_user_id_2")
                    .IsClustered(false);

                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email_address");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                //entity.Property(e => e.HireDate)
                //    .HasColumnType("datetime")
                //    .HasColumnName("hire_date")
                //    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("last_name")
                    .HasDefaultValueSql("('UNKNOWN')");

                entity.Property(e => e.AvailableAddresses)
                                  .HasColumnName("available_addresses");


                entity.Property(e => e.MiddleName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("middle_name").HasDefaultValueSql("('UNKNOWN')");


                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                //entity.Property(e => e.PubId)
                //    .HasColumnName("pub_id")
                //    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasDefaultValueSql("((1))");

                //entity.Property(e => e.Source)
                //    .HasMaxLength(100)
                //    .IsUnicode(false)
                //    .HasColumnName("source");

                //entity.HasOne(d => d.Pub)
                //    .WithMany(p => p.Users)
                //    .HasForeignKey(d => d.PubId)
                //    .HasConstraintName("FK__User__pub_id__440B1D61");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__User__role_id__4316F928");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
