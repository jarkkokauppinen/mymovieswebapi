using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Director : Person
  {
    internal Database Db { get; set; }

    public Director() {}

    internal Director(Database db)
    {
      Db = db;
    }

    public async Task<Director> GetDirector(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select firstname, lastname
      from director where iddirector =
      (select iddirector from movie where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await ReturnDirector(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveDirector()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into director
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

    private async Task<Director> ReturnDirector(DbDataReader reader)
    {
      var director = new Director(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          director = new Director(Db)
          {
            firstname = reader.GetString(0),
            lastname = reader.GetString(1)
          };
        }
      }
      return director;
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
    }
  }
}