using TMS.Domain.DTOs;

namespace TMS.Application.Interfaces.HubClients;

public interface IMapHubClient
{
    event Action<IEnumerable<MapLinesDTO>>? MapLinesLoaded;
}