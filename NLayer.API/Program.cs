using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
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
});//Api'de varsay�lan olarak kullan�lan filtreyi yeniden yap�land�rd�k.Kendi exception mesajlar�m�z� d�nmek i�in. 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(NotFoundFilter<>)); //NotFoundFilter'� program cs'e bildirdik. //Filterlar i�in de bir mod�le olu�turulmal�

builder.Services.AddAutoMapper(typeof(MapProfile)); //Built-in DI Conteiner

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

/*
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); //Built-in DI Conteiner herhangi bir class�n constructorunda gerekli olan interface ve bu interfaceye kar��l�k gelen clas� belirtiriz. Bunlar i�in AutoFac kullanaca��z.
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>)); //Built-in DI Conteiner
builder.Services.AddScoped(typeof(IService<>),typeof(Service<>)); //Built-in DI Conteiner

builder.Services.AddScoped<IProductService, ProductService>(); //Built-in DI Conteiner
builder.Services.AddScoped<IProductRepository,ProductRepository>(); //Built-in DI Conteiner
builder.Services.AddScoped<ICategoryService, CategoryService>(); //Built-in DI Conteiner 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); //Built-in DI Conteiner
*/

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); //Built-in DI Conteiner yerine AutoFac kullanaca��z.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder=>containerBuilder.RegisterModule(new RepoServiceModule())); //RepoServiceModule register edilir. filter modul i�inde eklenmeli



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UserCustomExeption(); //middleware 

app.UseAuthorization();

app.MapControllers();

app.Run();
