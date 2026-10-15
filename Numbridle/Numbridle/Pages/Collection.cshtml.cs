using Microsoft.AspNetCore.Mvc.RazorPages;
using Numbridle.Models;
using Numbridle.Services;

namespace Numbridle.Pages;

public class CollectionModel(IGameService gameService) : PageModel
{
    public List<NumberDiscovery> Discoveries { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Discoveries = await gameService.GetDiscoveriesAsync(1);
    }
}