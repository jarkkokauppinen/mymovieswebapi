using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Actor : Person
  {
    public string actor { get; set; }

    public Actor() {}

    internal Database Db { get; set; }

    internal Actor(Database db)
    {
      Db = db;
    }

    public async Task<List<Actor>> GetActors(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select concat(firstname, ' ', lastname) from actor where
      idactor in (select idactor from movieactor where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await Return(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveActor()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into actor
      (firstname, lastname) values (@firstname, @lastname)";
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

    private async Task<List<Actor>> Return(DbDataReader reader)
    {
      var list = new List<Actor>();
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          var actor = new Actor(Db)
          {
            actor = reader.GetString(0)
          };
          list.Add(actor);
        }
      }
      return list;
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
    }
  }
}