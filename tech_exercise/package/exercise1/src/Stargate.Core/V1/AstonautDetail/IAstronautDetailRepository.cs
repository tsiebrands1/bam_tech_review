namespace Stargate.Core.V1.AstonautDetail;

public interface IAstronautDetailRepository
{
	ValueTask<IEnumerable<IAstronautDetail>> GetAllAsync(CancellationToken cancellationToken);
	ValueTask<IAstronautDetail?> GetAsync(int id, CancellationToken cancellationToken);
	ValueTask<IAstronautDetail?> GetByPersonIdWithRawSqlAsync(int personId, CancellationToken cancellationToken);
}
