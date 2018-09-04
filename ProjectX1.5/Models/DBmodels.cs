namespace ProjectX1._5.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBmodels : DbContext
    {
        public DBmodels()
            : base("name=DBmodels")
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<AccessoryType> AccessoryTypes { get; set; }
        public virtual DbSet<AlcoholCategory> AlcoholCategory { get; set; }
        public virtual DbSet<AlcoholProduct> AlcoholProduct { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Bundle> Bundles { get; set; }
        public virtual DbSet<BundleInfo> BundleInfos { get; set; }
        public virtual DbSet<BuyingHistory> BuyingHistory { get; set; }
        public virtual DbSet<BuyingHistoryItem> BuyingHistoryItems { get; set; }
        public virtual DbSet<CigarBrand> CigarBrands { get; set; }
        public virtual DbSet<CigaresProduct> CigaresProducts { get; set; }
        public virtual DbSet<CigaresWrapper> CigaresWrapper { get; set; }
        public virtual DbSet<CigarStrength> CigarStrengths { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<WatchList> WatchList { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<AccessoryType>()
                .Property(e => e.typeName)
                .IsUnicode(false);

            modelBuilder.Entity<AccessoryType>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<AccessoryType>()
                .HasMany(e => e.Accessories)
                .WithRequired(e => e.AccessoryType1)
                .HasForeignKey(e => e.accessoryType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AlcoholCategory>()
                .Property(e => e.categoryName)
                .IsUnicode(false);

            modelBuilder.Entity<AlcoholCategory>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<AlcoholCategory>()
                .HasMany(e => e.AlcoholProducts)
                .WithOptional(e => e.AlcoholCategory)
                .HasForeignKey(e => e.categoryID);

            modelBuilder.Entity<AlcoholProduct>()
                .Property(e => e.origin)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasOptional(e => e.Customer)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Bundle>()
                .Property(e => e.bundleName)
                .IsUnicode(false);

            modelBuilder.Entity<Bundle>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<Bundle>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<Bundle>()
                .HasMany(e => e.BuyingHistoryItems)
                .WithRequired(e => e.Bundles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.buyingDate)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.shippmentCountry)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.creditCardID)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.shippmentCity)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.shippmentAddress)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.phoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .Property(e => e.postalCode)
                .IsUnicode(false);

            modelBuilder.Entity<BuyingHistory>()
                .HasMany(e => e.BuyingHistoryItems)
                .WithRequired(e => e.BuyingHistory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CigarBrand>()
                .Property(e => e.brandName)
                .IsUnicode(false);

            modelBuilder.Entity<CigarBrand>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<CigarBrand>()
                .HasMany(e => e.CigaresProducts)
                .WithRequired(e => e.CigarBrand1)
                .HasForeignKey(e => e.cigarBrand);

            modelBuilder.Entity<CigaresProduct>()
                .Property(e => e.origin)
                .IsUnicode(false);

            modelBuilder.Entity<CigaresProduct>()
                .Property(e => e.wrapperType)
                .IsUnicode(false);

            modelBuilder.Entity<CigaresWrapper>()
                .Property(e => e.wrapperName)
                .IsUnicode(false);

            modelBuilder.Entity<CigaresWrapper>()
                .Property(e => e.color)
                .IsUnicode(false);

            modelBuilder.Entity<CigaresWrapper>()
                .HasMany(e => e.CigaresProducts)
                .WithOptional(e => e.CigaresWrapper)
                .HasForeignKey(e => e.wrapperType)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CigarStrength>()
                .Property(e => e.strengthName)
                .IsUnicode(false);

            modelBuilder.Entity<CigarStrength>()
                .HasMany(e => e.CigaresProducts)
                .WithRequired(e => e.CigarStrength)
                .HasForeignKey(e => e.strengthID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.countryName)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.AlcoholProducts)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.origin);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.CigaresProducts)
                .WithOptional(e => e.Country)
                .HasForeignKey(e => e.origin);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.Country)
                .HasForeignKey(e => e.customerCountry)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.creditCardID)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.creditType)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.ownerName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.expirationMonth)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.expirationYear)
                .IsUnicode(false);

            modelBuilder.Entity<CreditCard>()
                .Property(e => e.securityDigits)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customerName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customerLastName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customerphoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customerCountry)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customerCity)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.BuyingHistories)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.userId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.CreditCards)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.userId);

            modelBuilder.Entity<Product>()
                .Property(e => e.productName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.pic)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.Accessory)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.AlcoholProduct)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.CigaresProduct)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Product>()
                .HasOptional(e => e.Sale)
                .WithRequired(e => e.Product);

            modelBuilder.Entity<Sale>()
                .Property(e => e.saleName)
                .IsUnicode(false);

            modelBuilder.Entity<Sale>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<WatchList>()
                .Property(e => e.note)
                .IsUnicode(false);
        }
    }
}
