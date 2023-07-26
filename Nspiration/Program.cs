using FluentMigrator.Runner;
using Nspiration.BusinessLogic.IBusinessLogic;
using Nspiration.BusinessLogic;
using Nspiration.BusinessRepository.IBusinessRepository;
using Nspiration.BusinessRepository;
using Nspiration.Migrations;
using Microsoft.EntityFrameworkCore;
using Nspiration.NspirationDBContext;

var builder = WebApplication.CreateBuilder(args);

//Cors Policy
builder.Services.AddCors(options =>
                options.AddPolicy(
                    "CorsPolicy",                                      
                    builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
                    ));

// Adding Repository 

builder.Services.AddTransient<IColorBl, ColorBl>();
builder.Services.AddTransient<IColorBr, ColorBr>();
builder.Services.AddTransient<IFolderBl, FolderBl>();
builder.Services.AddTransient<IFolderBr, FolderBr>();
builder.Services.AddTransient<IProjectBl, ProjectBl>();
builder.Services.AddTransient<IProjectBr, ProjectBr>();
builder.Services.AddTransient<IUserBl, UserBl>();
builder.Services.AddTransient<IUserBr, UserBr>();

// Add services to the container.

builder.Services.AddDbContext<NspirationDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("NspirationDB")));
builder.Services.AddLogging(builder => builder.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(builder => builder.AddPostgres()
                .WithGlobalConnectionString("NspirationDB")
                .ScanIn(typeof(AddColorTable_20230704483398).Assembly).For.Migrations().For.EmbeddedResources());
builder.Services.AddDbContext<NspirationPortalOldDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("NspirationPortalOldDB")));
//builder.Services.ConfigureRunner(builder => builder.AddSqlServer()
//                .WithGlobalConnectionString("NspirationPortalOldDB"));


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
using (var scope = app.Services.CreateScope())
{
    {
        var db = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        db.MigrateUp();
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();

