using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace mymovieswebapi
{
  public class Movie
  {
    public int idmovie { get; set; }
    public string title { get; set; }
    public string year { get; set; }
    public string description { get; set; }
    public string image_url { get; set; }
    public string user { get; set; }

    internal Database Db { get; set; }

    internal Movie(Database db)
    {
      Db = db;
    }

    public async Task<List<Movie>> GetSearched(string search)
    {
      search = search.ToLower();
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select idmovie, title from movie where
      lower(title) like '%" + search + "%' order by title limit 10";
      cmd.Parameters.AddWithValue("search", search);
      return await ReturnSearched(await cmd.ExecuteReaderAsync());
    }

    public async Task<Movie> GetById(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select title, year, description, image_url,
      concat(firstname, ' ', lastname) as user from movie inner join app_user on
      movie.iduser = app_user.iduser where idmovie = @id";
      cmd.Parameters.AddWithValue("id", id);
      return await ReturnMovie(await cmd.ExecuteReaderAsync());
    }

    private async Task<List<Movie>> ReturnSearched(DbDataReader reader)
    {
      var list = new List<Movie>();
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          var movie = new Movie(Db)
          {
            idmovie = reader.GetInt32(0),
            title = reader.GetString(1),
          };
        list.Add(movie);
        }
      }
      return list;
    }

    private async Task<Movie> ReturnMovie(DbDataReader reader)
    {
      var movie = new Movie(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          movie = new Movie(Db)
          {
            title = reader.GetString(0),
            year = (reader["year"] as string) ?? "year unknown",
            description = (reader["description"] as string) ?? "no description",
            image_url = (reader["image_url"] as string) ?? "no image",
            user = reader.GetString(4)
          };
        }
      }
      return movie;
    }
  }
}