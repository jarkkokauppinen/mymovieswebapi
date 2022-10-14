using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class MovieRating
  {
    public int idmovierating { get; set; }
    public string idmovie { get; set; }
    public string idrating { get; set; }

    internal Database Db { get; set; }

    public MovieRating() {}

    internal MovieRating(Database db)
    {
      Db = db;
    }

    public async Task<MovieRating> GetRow(string idmovie, int iduser)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select idmovierating from movierating where
      idmovie = @idmovie and idrating in (select idrating from rating where
      iduser = @iduser)";
      cmd.Parameters.AddWithValue("idmovie", idmovie);
      cmd.Parameters.AddWithValue("iduser", iduser);
      return await ReturnRow(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveRatings()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into movierating
      (idmovie, idrating) values (@idmovie, @idrating)";
      cmd.Parameters.AddWithValue("idmovie", idmovie);
      cmd.Parameters.AddWithValue("idrating", idrating);
      await cmd.ExecuteNonQueryAsync();
      return "Success";
    }

    public async Task<string> DeleteRatings(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"delete from movierating where idmovie = @id";
      cmd.Parameters.AddWithValue("id", id);
      await cmd.ExecuteNonQueryAsync();
      await cmd.ExecuteNonQueryAsync();
      return "Deleted";
    }

    private async Task<MovieRating> ReturnRow(DbDataReader reader)
    {
      var row = new MovieRating(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          row = new MovieRating(Db)
          {
            idmovierating = reader.GetInt32(0)
          };
        }
      }
      return row;
    }
  }
}