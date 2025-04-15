namespace Stargate.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using System.ComponentModel.DataAnnotations.Schema;

[Table("AstronautDuty")]
public class AstronautDuty : EntityBase, IAstronautDuty
{
	private IPerson? person = null!;

	public int? PersonId { get; set; } = null!;
	public string Rank { get; set; } = string.Empty;
	public string DutyTitle { get; set; } = string.Empty;
	public DateTime DutyStartDate { get; set; } = DateTime.MinValue;
	public DateTime? DutyEndDate { get; set; } = null;

	public virtual IPerson? Person => this.Person1 = null!;

	public IPerson? Person1 { get => this.person; set => this.person = value; }

	public void SetPerson(IPerson? person)
	{
		this.Person1 = person;
	}
}

public class AstronautDutyConfiguration : IEntityTypeConfiguration<AstronautDuty>
{
	public void Configure(EntityTypeBuilder<AstronautDuty> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.Property(x => x.CreatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.DeletedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.IsDeleted).HasDefaultValueSql(SQLFALSE);
	}
}
