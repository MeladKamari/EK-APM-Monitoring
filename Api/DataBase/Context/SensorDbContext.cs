using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.DataBase.Context;

public class SensorDbContext(DbContextOptions<SensorDbContext> options) : DbContext(options)
{
    public DbSet<SensorData>  SensorData { get; set; }
}