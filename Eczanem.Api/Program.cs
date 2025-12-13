using Eczanem.Api.Data; // DbContext'imizin bulunduðu yer
using Eczanem.Api.Interfaces; 
using Eczanem.Api.Repositories;
using Eczanem.Api.Services;
using Microsoft.EntityFrameworkCore; // EF Core kütüphanesi
var builder = WebApplication.CreateBuilder(args);

//  appsettings.json'dan baðlantý dizesini oku.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  DbContext'i servislere kaydet ve SQL Server kullanacaðýný belirt.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
// Pharmacy Service'i Dependency Injection'a kaydetme
builder.Services.AddScoped<IPharmacyService, PharmacyService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
// Program.cs dosyasýnda CORS kýsmýný þöyle deðiþtir:

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.AllowAnyOrigin()   // <-- DÝKKAT: Port kýsýtlamasýný kaldýrdýk! Herkes gelebilir.
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
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

//app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
