using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class MovieActor
  {

    public string idmovie { get; set; }
    public string idactor {get; set; }

    internal Database Db { get; set; }

    public MovieActor() {}

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
      await cmd.ExecuteNonQueryAsync();
      return "Success";
    }

    public async Task<string> DeleteRows(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"delete from movieactor where idmovie = @id";
      cmd.Parameters.AddWithValue("id", id);
      await cmd.ExecuteNonQueryAsync();
      return "Deleted";
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("idmovie", idmovie);
      cmd.Parameters.AddWithValue("idactor", idactor);
    }
  }
}