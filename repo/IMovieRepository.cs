using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHosty.repo
{
    public interface IMovieRepository
    {
        Task<List<Movie>> GetAllMovies();
        Task<Movie> GetMovie(int id);
        void AddMovie(Movie movie);
        bool DeleteMovie(int id);
        Task<Tuple<bool, string>> UpdateMovie(int id, Movie movie);
    }
}
