namespace Stargate.Data.Entities;
using Stargate.Core.V1;
using System;

public abstract class EntityBase : IEntity
{
	public int Id { get; set; } = 0;
	public DateTime? CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public DateTime? DeletedAt { get; set; }
	public bool IsDeleted { get; set; } = false;
}

//public class EntityBaseConfiguration : IEntityTypeConfiguration<EntityBase>
//{
//	public void Configure(EntityTypeBuilder<EntityBase> builder)
//	{
//		builder.HasKey(e => e.Id);
//		builder.Property(e => e.CreatedAt).HasDefaultValueSql(SQLGETDATE);
//		builder.Property(e => e.UpdatedAt).HasDefaultValueSql(SQLGETDATE);
//		builder.Property(e => e.DeletedAt).HasDefaultValueSql(SQLGETDATE);
//		builder.Property(e => e.IsDeleted).HasDefaultValueSql(SQLFALSE);
//	}
//}