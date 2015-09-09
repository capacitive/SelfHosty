using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Collections.Generic;
using System.Web.Http.Cors;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json.Schema;
using SelfHosty.repo;

namespace SelfHosty
{
    [EnableCors(origins: "http://localhost:7006", headers: "*", methods: "*")]
    public class MoviesController : ApiController
    {
        readonly List<Movie> _movieList = new List<Movie>();
        readonly IMovieRepository _repo = new MovieRepository();
        private bool _seed;

        public MoviesController()
        {
            bool.TryParse(ConfigurationManager.AppSettings["seed"], out _seed);
            if (!_seed) return;

            _movieList.Add(new Movie { id = 23, title = "Star Trek Beyond", releaseYear = 2016 });
            _movieList.Add(new Movie { id = 1, title = "Star Wars IV: A New Hope", releaseYear = 1977 });
            _movieList.Add(new Movie { id = 11, title = "Star Trek: The Motion Picture", releaseYear = 1979 });
            _movieList.Add(new Movie { id = 2, title = "Star Wars V: The Empire Strikes Back", releaseYear = 1981 });
            _movieList.Add(new Movie { id = 3, title = "Star Wars VI: Return of the Jedi", releaseYear = 1983 });
            _movieList.Add(new Movie { id = 12, title = "Star Trek II: The Wrath of Khan", releaseYear = 1982 });
            _movieList.Add(new Movie { id = 13, title = "Star Trek III: The Search for Spock", releaseYear = 1984 });
            _movieList.Add(new Movie { id = 14, title = "Star Trek IV: The Voyage Home", releaseYear = 1986 });
            _movieList.Add(new Movie { id = 15, title = "Star Trek V: The Final Frontier", releaseYear = 1989 });
            _movieList.Add(new Movie { id = 16, title = "Star Trek III: The Undiscovered Country", releaseYear = 1991 });
            _movieList.Add(new Movie { id = 17, title = "Star Trek: Generations", releaseYear = 1994 });
            _movieList.Add(new Movie { id = 18, title = "Star Trek: First Contact", releaseYear = 1996 });
            _movieList.Add(new Movie { id = 19, title = "Star Trek: Insurrection", releaseYear = 1998 });
            _movieList.Add(new Movie { id = 20, title = "Star Trek: Nemesis", releaseYear = 2002 });
            _movieList.Add(new Movie { id = 21, title = "Star Trek", releaseYear = 2009 });
            _movieList.Add(new Movie { id = 22, title = "Star Trek Into Darkness", releaseYear = 2013 });
            foreach (var movie in _movieList)
            {
                _repo.AddMovie(movie);
            }
        }

        public IEnumerable<Movie> Get()
        {        
            if(_seed) return _movieList.OrderBy(m => m.title);
            return _repo.GetAllMovies().Result;
        }

        public Movie Get(int id)
        {
            Thread.Sleep(1000);
            return _seed ? _movieList.SingleOrDefault(m => m.id == id) : _repo.GetMovie(id).Result;
        }

        [HttpPut]
        public Movie Post([FromBody]Movie movie)
        {
            var result = _repo.UpdateMovie(movie.id, movie).Result;
            var message = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
            {
                Content = new StringContent(string.Format("Id {0} not found", movie.id)),
                ReasonPhrase = "Movie Id not found"
            };

            if (!result.Item1 && result.Item2.Length == 0)
            {           
                throw new HttpResponseException(message);
            }
            if (result.Item2.Length > 0)
            {
                message.Content = new StringContent(string.Format("Movie invalid: {0}", result.Item2));
                message.ReasonPhrase = "Movie invalid";
                throw new HttpResponseException(message);
            }
            return movie;
        }
    }

    public class Movie
    {
        [BsonId]
        public int id { get; set; }
        public string title { get; set; }
        public int releaseYear { get; set; }
        public IList<Actor> Actors { get; set; }

        public Movie()
        {
            Actors = new List<Actor>();
        }
    }

    public class Actor
    {
        [BsonId]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string roleName { get; set; }
    }
}
