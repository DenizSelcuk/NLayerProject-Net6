using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;

namespace NLayer.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product{Id=1, Name="Pencil 1", CategoryId=1, Price=1, stock=20,CreateDate=DateTime.Now},
                new Product{Id=2, Name="Pencil 2", CategoryId=1, Price=2, stock=35,CreateDate=DateTime.Now},
                new Product{Id=3, Name="Pencil 3", CategoryId=1, Price=3, stock=2,CreateDate=DateTime.Now},
                new Product{Id=1, Name="Book 1", CategoryId=2, Price=15, stock=50,CreateDate=DateTime.Now},
                new Product{Id=2, Name="Book 2", CategoryId=2, Price=25, stock=75,CreateDate=DateTime.Now});
        }
    }
}