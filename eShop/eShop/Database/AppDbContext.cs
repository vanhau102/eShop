using eShop.Database.Configs;
using eShop.Database.Entities;
using Microsoft.EntityFrameworkCore;
using eShop.Areas.Admin.ViewModels.Category;

namespace eShop.Database
{
	public class AppDbContext : DbContext
	{
		public DbSet<ProductCategory> ProductCategories{ get; set; }
		public DbSet<Product> Products { get; set; }

		public AppDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// gọi class để tạo cấu hình
			modelBuilder.ApplyConfiguration(new ProductCategoryConfig());
			modelBuilder.ApplyConfiguration(new ProductConfig());
		}

		public DbSet<eShop.Areas.Admin.ViewModels.Category.AddOrUpdateCategoryVM>? AddOrUpdateCategoryVM { get; set; }
	}
}
