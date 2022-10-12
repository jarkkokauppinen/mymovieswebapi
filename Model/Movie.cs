using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Movie
  {
    public string idmovie { get; set; }
    public string title { get; set; }
    public string year { get; set; }
    public string description { get; set; }
    public string image { get; set; }
    public string iddirector { get; set; }
    public int idgenre { get; set; }
    public int iduser { get; set; }
    public string user { get; set; }
    public string director_firstname { get; set; }
    public string director_lastname { get; set; }
    public string genre { get; set; }

    internal Database Db { get; set; }

    public Movie() {}

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

    public async Task<Movie> GetById(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select idmovie, title, year, description, image,
      movie.iduser, concat(app_user.firstname, ' ', app_user.lastname) as user,
      movie.iddirector, director.firstname, director.lastname, genre from movie
      inner join director on director.iddirector = movie.iddirector
      inner join genre on genre.idgenre = movie.idgenre
      inner join app_user on app_user.iduser = movie.iduser where idmovie = @id";
      cmd.Parameters.AddWithValue("id", id);
      return await ReturnMovie(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> PostMovie()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into movie
      (idmovie, title, year, description, image, iddirector, idgenre, iduser)
      values (@idmovie, @title, @year, @description, @image, @iddirector,
      @idgenre, @iduser)";
      BindParams(cmd);
      try
      {
        await cmd.ExecuteNonQueryAsync();
        return "Movie added";
      }
      catch (System.Exception)
      {   
        return "A movie with that title already exists";
      } 
    }

    public async Task UpdateMovie()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"update movie set idmovie = @idmovie, title = @title,
      year = @year, description = @description, image = @image,
      iddirector = @iddirector, idgenre = @idgenre, iduser = @iduser
      where idmovie = @idmovie";
      BindParams(cmd);
      await cmd.ExecuteNonQueryAsync();
    }

    public async Task<string> DeleteMovie(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"delete from movie where idmovie = @id";
      cmd.Parameters.AddWithValue("id", id);
      await cmd.ExecuteNonQueryAsync();
      return "Deleted";
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
            idmovie = reader.GetString(0),
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
            idmovie = reader.GetString(0),
            title = reader.GetString(1),
            year = reader.GetString(2),
            description = reader.GetString(3),
            image = reader.GetString(4),
            iduser = reader.GetInt32(5),
            user = reader.GetString(6),
            iddirector = reader.GetString(7),
            director_firstname = reader.GetString(8),
            director_lastname = reader.GetString(9),
            genre = reader.GetString(10)
          };
        }
      }
      return movie;
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("idmovie", idmovie);
      cmd.Parameters.AddWithValue("title", title);
      cmd.Parameters.AddWithValue("year", year);
      cmd.Parameters.AddWithValue("description", description);
      cmd.Parameters.AddWithValue("image", image);
      cmd.Parameters.AddWithValue("iddirector", iddirector);
      cmd.Parameters.AddWithValue("idgenre", idgenre);
      cmd.Parameters.AddWithValue("iduser", iduser);
    }
  }
}