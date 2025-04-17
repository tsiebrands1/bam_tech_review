namespace Stargate.Data.Entities;

using Microsoft.EntityFrameworkCore;

public class StargateContext(DbContextOptions<StargateContext> options)
	: DbContext(options)
{
	public DbSet<Person> People { get; set; } = null!;
	public DbSet<AstronautDetail> AstronautDetails { get; set; } = null!;
	public DbSet<AstronautDuty> AstronautDuties { get; set; } = null!;
}
