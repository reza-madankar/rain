using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Rain.Context.Repository;
using Rain.Context.Services;
using Rain.Context;
using Microsoft.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rain API", Version = "v1" });
    c.OperationFilter<AddRequiredHeaderParameter>();
});


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var connectionString = builder.Configuration.GetConnectionString("RainDB");
builder.Services.AddDbContext<RainContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRain, RainRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

// Disable HTTPS redirection in development
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RainContext>();
    var retries = 10;
    var delay = 10000;
    while (retries > 0)
    {
        try
        {
            Console.WriteLine("Attempting to connect to SQL Server...");
            var conn = db.Database.GetDbConnection();
            var dbName = conn.Database;

            var masterConnectionString = new SqlConnectionStringBuilder(conn.ConnectionString)
            {
                InitialCatalog = "master"
            }.ConnectionString;

            using (var masterConnection = new SqlConnection(masterConnectionString))
            {
                masterConnection.Open();
                Console.WriteLine("Connected to master database.");
                using (var cmd = masterConnection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT database_id FROM sys.databases WHERE Name = '{dbName}'";
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine($"Database {dbName} does not exist. Creating and applying migrations...");
                        db.Database.Migrate();
                        Console.WriteLine($"Database {dbName} created and migrations applied successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Database {dbName} exists. Ensuring migrations are up to date...");
                        db.Database.Migrate();
                        Console.WriteLine($"Migrations for {dbName} are up to date.");
                    }
                }
            }
            // Verify table existence
            using (var _conn = new SqlConnection(connectionString))
            {
                _conn.Open();
                using (var cmd = _conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM sys.tables WHERE name = 'RainEntities'";
                    var tableCount = (int)cmd.ExecuteScalar();
                    if (tableCount == 0)
                    {
                        Console.WriteLine("Table 'RainEntities' does not exist. Ensure migrations are correctly defined.");
                        throw new InvalidOperationException("Table 'RainEntities' is missing in the database.");
                    }
                    else
                    {
                        Console.WriteLine("Table 'RainEntities' exists in the database.");
                    }
                }
            }
            break; // Exit loop on success
        }
        catch (SqlException ex) when (ex.Number == 18456)
        {
            Console.WriteLine($"Login failed (Error 18456): {ex.Message}. Retries left: {retries}");
            retries--;
            if (retries == 0)
            {
                Console.WriteLine("Max retries reached. Unable to connect to database.");
                throw;
            }
            Thread.Sleep(delay);
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Server error (Error {ex.Number}): {ex.Message}. Retries left: {retries}");
            retries--;
            if (retries == 0)
            {
                Console.WriteLine("Max retries reached. Unable to connect to database.");
                throw;
            }
            Thread.Sleep(delay);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during database setup: {ex.Message}");
            throw;
        }
    }
}

app.Run();
