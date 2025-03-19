using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Domain.Common;
using NetBanking.Core.Domain.Entities;

namespace NetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<SavingsAccount> SavingsAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = "DefaultAppUser";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = "DefaultAppUser";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region "Table Names"

            modelBuilder.Entity<Client>()
                .ToTable("Clients");

            modelBuilder.Entity<Beneficiary>()
                .ToTable("Beneficiaries");

            modelBuilder.Entity<CreditCard>()
                .ToTable("CreditCards");

            modelBuilder.Entity<Loan>()
                .ToTable("Loans");

            modelBuilder.Entity<SavingsAccount>()
                .ToTable("SavingsAccounts");

            modelBuilder.Entity<Transaction>()
                .ToTable("Transactions");

            modelBuilder.Entity<TransactionType>()
                .ToTable("TransactionTypes");

            #endregion

            #region "Primary Keys"
            modelBuilder.Entity<Client>()
                .HasKey(client => client.Id);

            modelBuilder.Entity<Beneficiary>()
                .HasKey(bf => bf.Id);

            modelBuilder.Entity<CreditCard>()
                .HasKey(cd => cd.Id);

            modelBuilder.Entity<Loan>()
                .HasKey(loan => loan.Id);

            modelBuilder.Entity<SavingsAccount>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<Transaction>()
                .HasKey(trans => trans.Id);

            modelBuilder.Entity<TransactionType>()
                .HasKey(transt => transt.Id);

            #endregion

            #region "Relationships"

            #region Beneficiary

            // not directly
            #endregion

            #region Client
            modelBuilder.Entity<Client>()
                .HasMany(c => c.Loans)
                .WithOne(l => l.Client)
                .HasForeignKey(l => l.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.SavingsAccounts)
                .WithOne(l => l.Client)
                .HasForeignKey(l => l.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Beneficiaries)
                .WithOne(l => l.Client)
                .HasForeignKey(l => l.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.CreditCards)
                .WithOne(l => l.Client)
                .HasForeignKey(l => l.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region CreditCard
            // not directly

            #endregion

            #region Loan
            // not directly

            #endregion

            #region SavingsAccounts
            modelBuilder.Entity<SavingsAccount>()
                .HasMany(c => c.Beneficiaries)
                .WithOne(l => l.SavingsAccount)
                .HasForeignKey(l => l.SavingsAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #region Transactions


            #endregion

            #region TransactionType
            modelBuilder.Entity<TransactionType>()
                .HasMany(c => c.Transactions)
                .WithOne(l => l.TransactionType)
                .HasForeignKey(l => l.TransactionTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion

            #endregion

            #region "Property configurations"

            #region Beneficiary

            modelBuilder.Entity<Beneficiary>().
                Property(cc => cc.ClientId)
                .IsRequired();

            modelBuilder.Entity<Beneficiary>().
                Property(cc => cc.SavingsAccountId)
                .IsRequired();

            #endregion

            #region Client
            modelBuilder.Entity<Client>().
                Property(client => client.UserId)
                .IsRequired();

            #endregion

            #region CreditCard
            modelBuilder.Entity<CreditCard>().
                Property(cc => cc.Id)
                .IsRequired();

            modelBuilder.Entity<CreditCard>().
                Property(cc => cc.ClientId)
                .IsRequired();

            modelBuilder.Entity<CreditCard>().
                Property(cc => cc.Limit)
                .IsRequired();

            modelBuilder.Entity<CreditCard>().
                Property(cc => cc.Balance)
                .IsRequired();

            modelBuilder.Entity<CreditCard>().
                Property(cc => cc.Debit)
                .IsRequired();
            #endregion

            #region Loan
            modelBuilder.Entity<Loan>().
                Property(cc => cc.Id)
                .IsRequired();

            modelBuilder.Entity<Loan>().
                Property(cc => cc.ClientId)
                .IsRequired();

            modelBuilder.Entity<Loan>().
                Property(cc => cc.Amount)
                .IsRequired();

            modelBuilder.Entity<Loan>().
                Property(cc => cc.Balance)
                .IsRequired();
            #endregion

            #region SavingsAccount
            modelBuilder.Entity<SavingsAccount>().
                Property(sa => sa.Id)
                .IsRequired();

            modelBuilder.Entity<SavingsAccount>().
                Property(sa => sa.ClientId)
                .IsRequired();

            modelBuilder.Entity<SavingsAccount>().
                Property(sa => sa.Balance)
                .IsRequired();

            modelBuilder.Entity<SavingsAccount>().
                Property(sa => sa.IsMainAccount)
                .IsRequired();
            #endregion

            #region Transaction
            modelBuilder.Entity<Transaction>().
                Property(ts => ts.Origin)
                .IsRequired();

            modelBuilder.Entity<Transaction>().
                Property(ts => ts.Destination)
                .IsRequired();

            modelBuilder.Entity<Transaction>().
                Property(ts => ts.TransactionTypeId)
                .IsRequired();

            modelBuilder.Entity<Transaction>().
                Property(ts => ts.Amount)
                .IsRequired();

            modelBuilder.Entity<Transaction>().
                Property(ts => ts.Concept)
                .HasMaxLength(50);
            #endregion

            #region TransactionType
            modelBuilder.Entity<TransactionType>().
                Property(ts => ts.Name)
                .IsRequired();

            #endregion

            #endregion

            #region "Default Data"
            modelBuilder.Entity<TransactionType>().HasData(
                new TransactionType { Id = 1, Name = "Account Transfer" },
                new TransactionType { Id = 2, Name = "Loan Payment" },
                new TransactionType { Id = 3, Name = "Express Payment" },
                new TransactionType { Id = 4, Name = "Credit Card Payment" },
                new TransactionType { Id = 5, Name = "Cash Advance" },
                new TransactionType { Id = 6, Name = "Beneficiary Payment" }
            );

            #endregion

        }
    }
}
