using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class MovieRating
  {

    public string idmovie { get; set; }
    public string idrating { get; set; }

    internal Database Db { get; set; }

    public MovieRating() {}

    internal MovieRating(Database db)
    {
      Db = db;
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
  }
}