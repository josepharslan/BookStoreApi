using BookStore.BusinessLayer.Abstract;
using BookStore.BusinessLayer.Concrete;
using BookStore.BusinessLayer.Services;
using BookStore.DataAccessLayer.Abstract;
using BookStore.DataAccessLayer.Context;
using BookStore.DataAccessLayer.EntityFramework;
using BookStore.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var allowedOrigins = new[] {
    "https://localhost:7191"
};

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("FrontCors", p => p
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

// Add services to the container.
builder.Services.AddDbContext<BookStoreContext>();

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<IProductDal, EfProductDal>();
builder.Services.AddScoped<IQuoteDal, EfQuoteDal>();
builder.Services.AddScoped<ISubscriberDal, EfSubscriberDal>();

builder.Services.AddScoped<IDashboardService, DashboardManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IQuoteService, QuoteManager>();
builder.Services.AddScoped<INewsletterService, NewsletterManager>();

builder.Services.AddSingleton<IEmailQueue>(sp => new InMemoryEmailQueue(capacity: 2000));
builder.Services.AddSingleton<IEmailService, SmtpEmailService>();
builder.Services.AddHostedService<EmailBackgroundService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("FrontCors");

app.UseAuthorization();

app.MapControllers();

app.Run();
