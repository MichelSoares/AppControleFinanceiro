using ControleFinanceiroAPI.Context;
using Microsoft.EntityFrameworkCore;
using ControleFinanceiroAPI.Util;

var builder = WebApplication.CreateBuilder(args);

switch (Environment.OSVersion.Platform)
{
    case PlatformID.Unix:
        UtilHelper.SO = 1;
        string textoIniUnix = "\t\tIniciando Controle Financeiro API - UNIX\t\t<======";
        Console.WriteLine(textoIniUnix);
        UtilHelper.myLogTxtSimple(textoIniUnix, builder.Configuration);
        break;
    default:
        UtilHelper.SO = 0;
        string textoIniWindows = "\t\tIniciando Controle Financeiro API - WINDOWS\t\t<======";
        Console.WriteLine(textoIniWindows);
        UtilHelper.myLogTxtSimple(textoIniWindows, builder.Configuration);
        break;
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var postgresSqlConn = builder.Configuration.GetConnectionString("DefaultString");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(postgresSqlConn));

var app = builder.Build();

//app.UseCors(opt => opt.WithOrigins("https://www.apirequest.io").WithMethods("GET"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Run("http://*:8443");
app.Run();
