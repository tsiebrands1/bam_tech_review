namespace Stargate.Data.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stargate.Core.V1.AstonautDetail;
using Stargate.Core.V1.AstronautDuty;
using Stargate.Core.V1.Person;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Person")]
public class Person : EntityBase, IPerson
{
	public Person()
	{
		this.astronautDetail = new AstronautDetail();
		this.astronautDuties = [];
	}

	public string Name { get; set; } = string.Empty;

	private IAstronautDetail astronautDetail = new AstronautDetail();
	private IEnumerable<IAstronautDuty> astronautDuties = [];

	public virtual IAstronautDetail AstronautDetail {
		get { return this.astronautDetail; }
		set { this.astronautDetail = value; }
	}

	public virtual IEnumerable<IAstronautDuty> AstronautDuties {
		get { return this.astronautDuties; }
		set { this.astronautDuties = value; }
	}
}

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
	public void Configure(EntityTypeBuilder<Person> builder)
	{
		builder.HasKey(x => x.Id);
		builder.Property(x => x.Id).ValueGeneratedOnAdd();
		builder.HasOne(z => z.AstronautDetail)
			.WithOne(z => (Person)z.Person!)
			.HasForeignKey<AstronautDetail>(z => z.PersonId);
		builder.HasMany(z => z.AstronautDuties)
			.WithOne(z => (Person)z.Person!)
			.HasForeignKey(z => z.PersonId);
		builder.Property(x => x.CreatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.UpdatedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.DeletedAt).HasDefaultValueSql(SQLGETDATE);
		builder.Property(x => x.IsDeleted).HasDefaultValueSql(SQLFALSE);
	}
}
