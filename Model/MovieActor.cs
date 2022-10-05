using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class MovieActor
  {

    public int idmovie { get; set; }
    public int idactor {get; set; }

    public MovieActor() {}

    internal Database Db { get; set; }

    internal MovieActor(Database db)
    {
      Db = db;
    }

    public async Task<string> SaveActors()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into movieactor
      (idmovie, idactor) values (@idmovie, @idactor)";
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
      cmd.Parameters.AddWithValue("idactor", idactor);
    }
  }
}