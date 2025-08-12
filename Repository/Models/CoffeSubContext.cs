using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository.Models;

public partial class CoffeSubContext : DbContext
{
    public CoffeSubContext()
    {
    }

    public CoffeSubContext(DbContextOptions<CoffeSubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CoffeeItem> CoffeeItems { get; set; }

    public virtual DbSet<CoffeeRedemption> CoffeeRedemptions { get; set; }

    public virtual DbSet<DailyCupTracking> DailyCupTrackings { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<PlanCoffeeOption> PlanCoffeeOptions { get; set; }

    public virtual DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

    public virtual DbSet<SubscriptionTimeWindow> SubscriptionTimeWindows { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=MSI\\MSSQL2022EXPRESS;Uid=sa;Pwd=12345;Database=coffe_sub;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__D54EE9B4621D2CF2");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
        });

        modelBuilder.Entity<CoffeeItem>(entity =>
        {
            entity.HasKey(e => e.CoffeeId).HasName("PK__coffee_i__FE8F721D3792562C");

            entity.ToTable("coffee_items");

            entity.HasIndex(e => e.Code, "UQ__coffee_i__357D4CF9420C6565").IsUnique();

            entity.Property(e => e.CoffeeId).HasColumnName("coffee_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CoffeeName)
                .HasMaxLength(100)
                .HasColumnName("coffee_name");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");

            entity.HasOne(d => d.Category).WithMany(p => p.CoffeeItems)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__coffee_it__categ__403A8C7D");
        });

        modelBuilder.Entity<CoffeeRedemption>(entity =>
        {
            entity.HasKey(e => e.RedemptionId).HasName("PK__coffee_r__B17E4334DE4B0AE5");

            entity.ToTable("coffee_redemptions");

            entity.HasIndex(e => e.SubscriptionId, "idx_coffee_redemptions_subscription");

            entity.Property(e => e.RedemptionId).HasColumnName("redemption_id");
            entity.Property(e => e.CodeUsed)
                .HasMaxLength(20)
                .HasColumnName("code_used");
            entity.Property(e => e.CoffeeId).HasColumnName("coffee_id");
            entity.Property(e => e.FailureReason)
                .HasMaxLength(255)
                .HasColumnName("failure_reason");
            entity.Property(e => e.IsSuccessful).HasColumnName("is_successful");
            entity.Property(e => e.RedemptionTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("redemption_time");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");

            entity.HasOne(d => d.Coffee).WithMany(p => p.CoffeeRedemptions)
                .HasForeignKey(d => d.CoffeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__coffee_re__coffe__59FA5E80");

            entity.HasOne(d => d.Subscription).WithMany(p => p.CoffeeRedemptions)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__coffee_re__subsc__59063A47");
        });

        modelBuilder.Entity<DailyCupTracking>(entity =>
        {
            entity.HasKey(e => e.TrackingId).HasName("PK__daily_cu__7AC3E9AE183DA381");

            entity.ToTable("daily_cup_tracking");

            entity.HasIndex(e => new { e.SubscriptionId, e.Date }, "idx_daily_cup_tracking_subscription_date");

            entity.HasIndex(e => new { e.SubscriptionId, e.Date }, "unique_subscription_date").IsUnique();

            entity.Property(e => e.TrackingId).HasColumnName("tracking_id");
            entity.Property(e => e.CupsTaken).HasColumnName("cups_taken");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");

            entity.HasOne(d => d.Subscription).WithMany(p => p.DailyCupTrackings)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__daily_cup__subsc__5535A963");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__payment___85C600AFE1B116C9");

            entity.ToTable("payment_transactions");

            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BankCode)
                .HasMaxLength(20)
                .HasColumnName("bank_code");
            entity.Property(e => e.CardType)
                .HasMaxLength(20)
                .HasColumnName("card_type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("VND")
                .HasColumnName("currency");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("ip_address");
            entity.Property(e => e.OrderId)
                .HasMaxLength(50)
                .HasColumnName("order_id");
            entity.Property(e => e.PaymentTime)
                .HasMaxLength(50)
                .HasColumnName("payment_time");
            entity.Property(e => e.TransactionNo)
                .HasMaxLength(50)
                .HasColumnName("transaction_no");
            entity.Property(e => e.TransactionStatus)
                .HasMaxLength(20)
                .HasColumnName("transaction_status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PaymentTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payment_t__user___5DCAEF64");
        });

        modelBuilder.Entity<PlanCoffeeOption>(entity =>
        {
            entity.HasKey(e => e.OptionId).HasName("PK__plan_cof__F4EACE1B081F50B8");

            entity.ToTable("plan_coffee_options");

            entity.Property(e => e.OptionId).HasColumnName("option_id");
            entity.Property(e => e.CoffeeId).HasColumnName("coffee_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");

            entity.HasOne(d => d.Coffee).WithMany(p => p.PlanCoffeeOptions)
                .HasForeignKey(d => d.CoffeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plan_coff__coffe__4AB81AF0");

            entity.HasOne(d => d.Plan).WithMany(p => p.PlanCoffeeOptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__plan_coff__plan___49C3F6B7");
        });

        modelBuilder.Entity<SubscriptionPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PK__subscrip__BE9F8F1D4F6F5B76");

            entity.ToTable("subscription_plans");

            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.DailyCupLimit).HasColumnName("daily_cup_limit");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DurationDays).HasColumnName("duration_days");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlanName)
                .HasMaxLength(100)
                .HasColumnName("plan_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.TotalCups).HasColumnName("total_cups");
        });

        modelBuilder.Entity<SubscriptionTimeWindow>(entity =>
        {
            entity.HasKey(e => e.WindowId).HasName("PK__subscrip__4F5F203AA5A04A82");

            entity.ToTable("subscription_time_windows");

            entity.Property(e => e.WindowId).HasColumnName("window_id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.StartTime).HasColumnName("start_time");

            entity.HasOne(d => d.Plan).WithMany(p => p.SubscriptionTimeWindows)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__subscript__plan___46E78A0C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F6D878BEE");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E616439E4FBCD").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC57217C1A9C4").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__user_sub__863A7EC1AC43A5B5");

            entity.ToTable("user_subscriptions");

            entity.HasIndex(e => e.IsActive, "idx_user_subscriptions_active");

            entity.HasIndex(e => e.UserId, "idx_user_subscriptions_user");

            entity.Property(e => e.SubscriptionId).HasColumnName("subscription_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PaymentAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("payment_amount");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentReference)
                .HasMaxLength(100)
                .HasColumnName("payment_reference");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.RemainingCups).HasColumnName("remaining_cups");
            entity.Property(e => e.StartDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Plan).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_subs__plan___4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_subs__user___4E88ABD4");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
