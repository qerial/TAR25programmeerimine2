using Microsoft.AspNetCore.Mvc.RazorPages;
using Numbridle.Models;
using Numbridle.Services;

namespace Numbridle.Pages;

public class LeaderboardModel(IGameService gameService) : PageModel
{
    public List<Player> Players { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Players = await gameService.GetLeaderboardAsync();
    }
}