using Application.Features.OrderEvents.Commands.Add;

namespace Application.Common.Interfaces
{
    public interface IOrderEventHubService
    {
        Task AddedAsync(OrderEventHubDto orderEvent, CancellationToken cancellationToken);
    }
}
