using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Servers;

namespace SelfHosty.repo
{
    public class MovieRepository: IMovieRepository
    {
        MongoClient _client;
        private IMongoDatabase _database;
        private IMongoCollection<Movie> _collection;

        public MovieRepository()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            
            _client = new MongoClient("mongodb://localhost:27017");
            _database = _client.GetDatabase("dev");
            _collection = _database.GetCollection<Movie>("movies");
        }

        public async Task<List<Movie>> GetAllMovies()
        {
            var movies = await _database.GetCollection<Movie>("movies")
                .Find(x => x.title.Length > 0)
                .ToListAsync();

            return movies;
        }

        public async Task<Movie> GetMovie(int id)
        {
            var filter = Builders<Movie>.Filter.Eq("id", id);
            var movie = await _collection.Find(filter).FirstAsync();
            return movie;
        }

        public void AddMovie(Movie movie)
        {
            _collection.InsertOneAsync(movie);
        }

        public bool DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<bool, string>> UpdateMovie(int id, Movie movie)
        {
            var builder = new StringBuilder(20);

            if (movie.title.Length == 0 ) return Tuple.Create(false, builder.Append("title cannot be 0; ").ToString());
            if (movie.releaseYear == 0) return Tuple.Create(false, builder.Append("releaseYear cannot be 0").ToString());

            var filter = Builders<Movie>.Filter.Eq("id", movie.id);
            //var update = Builders<Movie>.Update.Set("title", movie.title);
            //update.AddToSet("releaseYear", movie.releaseYear);
            var result = await _collection.ReplaceOneAsync(filter, movie);

            if (result.ModifiedCount > 0)
            {
                return Tuple.Create(true, "");
            }
            return Tuple.Create(false, "");
        }
    }
}
