using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Numbridle.Data;
using Numbridle.Models;
using Numbridle.Services;

namespace Numbridle.Pages;

public class IndexModel(
    IGameService gameService,
    GameDbContext db) : PageModel
{
    private const int DemoPlayerId = 1;

    [BindProperty]
    public string Combination { get; set; } = "";

    public GuessResult? Result { get; private set; }

    public long TotalScore { get; private set; }

    public int DiscoveryCount { get; private set; }

    public async Task OnGetAsync()
    {
        await EnsurePlayerExistsAsync();
        await LoadStatsAsync();
    }

    public async Task<IActionResult> OnPostGuessAsync()
    {
        await EnsurePlayerExistsAsync();

        Result = await gameService.GuessAsync(
            DemoPlayerId, Combination);

        await LoadStatsAsync();

        return Page();
    }

    private async Task EnsurePlayerExistsAsync()
    {
        var player = await db.Players
            .FirstOrDefaultAsync(p => p.Id == DemoPlayerId);

        if (player is null)
        {
            db.Players.Add(new Player
            {
                Username = "Player",
                TotalScore = 0,
                CreatedAtUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();
        }
    }

    private async Task LoadStatsAsync()
    {
        var player = await db.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == DemoPlayerId);

        TotalScore = player?.TotalScore ?? 0;

        DiscoveryCount = await db.NumberDiscoveries
            .CountAsync(d => d.PlayerId == DemoPlayerId);
    }
    public async Task<IActionResult> OnPostRollAsync()
    {
        await EnsurePlayerExistsAsync();

        Result = await gameService.RollAsync(DemoPlayerId);

        await LoadStatsAsync();

        return Page();
    }
}