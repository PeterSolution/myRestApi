using ServerApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;
using AutoMapper;
using ServerApi.Mapper;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ServerApi.Functions;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);



string instanceName = "(localdb)\\MSSQLLocalDB"; 
var connectionString = builder.Configuration.GetConnectionString("StrToConnectToDB");

if (!DoesSqlServerInstanceExist(connectionString))
{
    try
    {
        Console.WriteLine($"Tworzenie nowej instancji SQL Server: {instanceName}");
        DbControlClass.CreateSqlServerInstance(instanceName);
        Console.WriteLine("Instancja SQL Server zosta³a utworzona pomyœlnie.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Wyst¹pi³ b³¹d podczas tworzenia instancji SQL Server: {ex.Message}");
        // return;
    }
}
else
{
    Console.WriteLine($"Instancja SQL Server {instanceName} ju¿ istnieje.");
}



var specificIpAddress = IPAddress.Parse("192.168.56.1"); 

/*builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(specificIpAddress, 5248); // HTTP
    *//*options.Listen(specificIpAddress, 7117, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });*//*
    options.Listen(specificIpAddress, 5249, listenOptions =>
    {
        var certPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Certificate:Path"]);

        var certPassword = builder.Configuration["Certificate:Password"];

        if (File.Exists(certPath))
        {
            listenOptions.UseHttps(new X509Certificate2(certPath, certPassword));
        }
        else
        {
            throw new FileNotFoundException("Certificate file not found.", certPath);
        }
    });
});
*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(ServerApi.Mapper.Mapper));

builder.Services.AddDbContext<ContextFile>(opt =>
{
    opt.UseSqlServer(connectionString);
});

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();






using (var dbcreate = app.Services.CreateScope())
{
    var con = dbcreate.ServiceProvider.GetRequiredService<ContextFile>();
    con.Database.Migrate();
}

app.Run();


static bool DoesSqlServerInstanceExist(string connectionString)
{
    try
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return true;
        }
    }
    catch
    {
        return false;
    }
}