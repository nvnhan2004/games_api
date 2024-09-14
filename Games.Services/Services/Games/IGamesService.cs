using Games.Services.Common;
using Games.Services.Services.Games.DTO;

namespace Games.Services.Services.Games
{
    public interface IGamesService : IGenericService<Data.Models.ChucNang.Games>
    {
        Task<string> CrawlGames(RequestCrawlGamesDTO requestCrawl);
        List<GamesPublicDTO> GetGamesNew();
        List<GamesPublicDTO> GetGamesByCategories(string cate);
        GamesDTO GetDetail(string slug);
    }
}
