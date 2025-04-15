namespace Stargate.Core.V1;
public interface IEntity
{
	int Id { get; set; }
	DateTime? CreatedAt { get; }
	DateTime? UpdatedAt { get; }
	DateTime? DeletedAt { get; }
	bool IsDeleted { get; set; }
}
