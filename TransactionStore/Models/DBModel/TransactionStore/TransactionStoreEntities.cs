using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TransactionStore.Models.DBModel.TransactionStore
{
    public partial class TransactionStoreEntities : DbContext
    {
        public TransactionStoreEntities()
        {
        }

        public TransactionStoreEntities(DbContextOptions<TransactionStoreEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction");

                entity.HasIndex(e => e.TransactionId, "TransactionID_UNIQ")
                    .IsUnique();

                entity.Property(e => e.Amount).HasColumnType("decimal(19, 4)");

                entity.Property(e => e.CurrencyCode).HasMaxLength(3);

                entity.Property(e => e.Status).HasMaxLength(10);

                entity.Property(e => e.StatusRaw).HasMaxLength(50);

                entity.Property(e => e.TransactionId).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
