using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Actor : Person
  {
    public string idactor { get; set; }
    public string actor { get; set; }

    internal Database Db { get; set; }

    public Actor() {}

    internal Actor(Database db)
    {
      Db = db;
    }

    public async Task<List<Actor>> GetActors(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select concat(firstname, ' ', lastname) from actor where
      idactor in (select idactor from movieactor where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await Return(await cmd.ExecuteReaderAsync());
    }

    public async Task<Actor> GetActorByName(string firstname, string lastname)
    {
      firstname = firstname.ToLower();
      lastname = lastname.ToLower();
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select idactor, firstname, lastname from actor
      where lower(firstname) = @firstname and lower(lastname) = @lastname";
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
      return await ReturnOne(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveActor(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into actor
      (idactor, firstname, lastname) values (@idactor, @firstname, @lastname)";
      cmd.Parameters.AddWithValue("idactor", idactor);
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
      await cmd.ExecuteNonQueryAsync();
      return id;
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

    private async Task<Actor> ReturnOne(DbDataReader reader)
    {
      var actor = new Actor();
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          actor = new Actor(Db)
          {
            idactor = reader.GetString(0),
            firstname = reader.GetString(1),
            lastname = reader.GetString(2)
          };
        }
      }
      return actor;
    }
  }
}