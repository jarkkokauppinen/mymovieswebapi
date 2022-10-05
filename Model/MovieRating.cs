using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class MovieRating
  {

    public int idmovie { get; set; }
    public int idrating {get; set; }

    public MovieRating() {}

    internal Database Db { get; set; }

    internal MovieRating(Database db)
    {
      Db = db;
    }

    public async Task<string> SaveRatings()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into movierating
      (idmovie, idrating) values (@idmovie, @idrating)";
      BindParams(cmd);
      try
      {
        await cmd.ExecuteNonQueryAsync();
        return "Success";
      }
      catch (System.Exception)
      {   
        return "Something went wrong";
      } 
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("idmovie", idmovie);
      cmd.Parameters.AddWithValue("idrating", idrating);
    }
  }
}