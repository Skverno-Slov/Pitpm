namespace TestApi
{
    public class AnimeService
    {
        private readonly List<Anime> _animeList = [
            new() { Id = 1, Title = "Attack on Titan", Genre = "Экшен", Year = 2013, Rating = 9.1 },
            new() { Id = 2, Title = "Death Note", Genre = "Детектив", Year = 2006, Rating = 9.0 },
            new() { Id = 3, Title = "Naruto", Genre = "Сёнен", Year = 2002, Rating = 8.5 }
        ];

        public IEnumerable<Anime> GetAnimes()
        {
            return _animeList;
        }

        public Anime GetAnime(int id)
        {
            var anime = _animeList.FirstOrDefault(a => a.Id == id);
            if (anime != null)
            {
                return anime;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void AddAnime(Anime anime)
        {
            _animeList.Add(anime);
        }

        public void RemoveAnime(int id)
        {
            var animeToDelete = _animeList.FirstOrDefault(anime => anime.Id == id);
            if (animeToDelete != null)
            {
                _animeList.Remove(animeToDelete);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }

    public class Anime
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Genre { get; set; } = "";
        public int Year { get; set; }
        public double Rating { get; set; }
    }
}
