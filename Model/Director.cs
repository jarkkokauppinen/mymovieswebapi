using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Director : Person
  {
    public string iddirector { get; set; }

    internal Database Db { get; set; }

    public Director() {}

    internal Director(Database db)
    {
      Db = db;
    }

    public async Task<Director> GetDirector(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select iddirector, firstname, lastname
      from director where iddirector =
      (select iddirector from movie where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await ReturnDirector(await cmd.ExecuteReaderAsync());
    }

    public async Task<Director> GetOneDirector(string firstname, string lastname)
    {
      firstname = firstname.ToLower();
      lastname = lastname.ToLower();
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select iddirector, firstname, lastname from director
      where lower(firstname) = @firstname and lower(lastname) = @lastname";
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
      return await ReturnDirector(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveDirector(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into director
      (iddirector, firstname, lastname) values (@iddirector, @firstname, @lastname)";
      cmd.Parameters.AddWithValue("iddirector", iddirector);
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
      await cmd.ExecuteNonQueryAsync();
      return id;
    }

    private async Task<Director> ReturnDirector(DbDataReader reader)
    {
      var director = new Director(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          director = new Director(Db)
          {
            iddirector = reader.GetString(0),
            firstname = reader.GetString(1),
            lastname = reader.GetString(2)
          };
        }
      }
      return director;
    }
  }
}