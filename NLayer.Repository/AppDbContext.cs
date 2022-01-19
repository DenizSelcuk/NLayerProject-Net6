using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLayer.Core;

namespace NLayer.Repository
{
    public class AppDbContext : DbContext
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {  
        } //options for startup.cs
        public DbSet<Category> Categories {get; set;} 
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductFeature> MyProperty { get; set; }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //implements the configuration that come from all assmbly that use IEntityTypeConfiguration interface. 
            modelBuilder.Entity<ProductFeature>().HasData(new ProductFeature(){Id=1,Color= "Red", Height=12,Width=10,ProductId=1}); //For example, could come from the assembly.
            base.OnModelCreating(modelBuilder);
        } 
    }
}