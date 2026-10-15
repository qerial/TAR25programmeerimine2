using Numbridle.Models;

namespace Numbridle.Services;

public interface IGameService
{
    Task<GuessResult> RollAsync(int playerId);

    Task<GuessResult> GuessAsync(int playerId, string combination);

    Task<List<Player>> GetLeaderboardAsync();

    Task<List<NumberDiscovery>> GetDiscoveriesAsync(int playerId);
}