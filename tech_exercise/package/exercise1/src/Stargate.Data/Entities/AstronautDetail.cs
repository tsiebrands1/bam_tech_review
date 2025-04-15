namespace Stargate.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.Person;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AstronautDetail")]
public class AstronautDetail : EntityBase, IAstronautDetail
{
	private IPerson? person = null!;

	public int? PersonId { get; set; } = 0;
	public string CurrentRank { get; set; } = string.Empty;
	public string CurrentDutyTitle { get; set; } = string.Empty;
	public DateTime CareerStartDate { get; set; }
	public DateTime? CareerEndDate { get; set; }
	public virtual IPerson? Person { get => this.person; }

	public void SetPerson(IPerson person)
	{
		this.person = person;
	}
}

public class AstronautDetailConfiguration : IEntityTypeConfiguration<AstronautDetail>
{
	public void Configure(EntityTypeBuilder<AstronautDetail> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.HasOne(z => z.Person)
			.WithOne(static z => (AstronautDetail)z.AstronautDetail)
			.HasForeignKey<AstronautDetail>(z => z.PersonId);
		builder.Property(x => x.CareerStartDate).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.CareerEndDate).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.CurrentDutyTitle).HasDefaultValue(string.Empty);
		builder.Property(x => x.CurrentRank).HasDefaultValue(string.Empty);
		builder.Property(x => x.CreatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.DeletedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.IsDeleted).HasDefaultValueSql(SQLFALSE);
	}
}
