using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); //Controllers ekledik ve filtre ekledik.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});//Api'de varsayýlan olarak kullanýlan filtreyi yeniden yapýlandýrdýk.Kendi exception mesajlarýmýzý dönmek için. 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(NotFoundFilter<>)); //NotFoundFilter'ý program cs'e bildirdik.

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); //DI
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>)); //DI
builder.Services.AddScoped(typeof(IService<>),typeof(Service<>)); //DI
builder.Services.AddAutoMapper(typeof(MapProfile)); //DI
builder.Services.AddScoped<IProductService, ProductService>(); //DI
builder.Services.AddScoped<IProductRepository,ProductRepository>(); //DI
builder.Services.AddScoped<ICategoryService, CategoryService>(); //DI
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //DI

builder.Services.AddDbContext<AppDbContext>(x=>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"),option=>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
}); //Bildiri

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UserCustomExeption(); //Bildiri middleware 

app.UseAuthorization();

app.MapControllers();

app.Run();
