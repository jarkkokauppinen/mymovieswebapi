using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class User : Person
  {
    public int iduser { get; set; }
    public string username { get; set; }
    public string password { get; set; }

    internal Database Db { get; set; }

    public User() {}

    internal User(Database db)
    {
      Db = db;
    }

    public async Task<string> CreateAccount()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into app_user (username, password, firstname, lastname)
      values (@username, @password, @firstname, @lastname)";
      BindParams(cmd);
      try
      {
        await cmd.ExecuteNonQueryAsync();
        return "OK";
      }
      catch (System.Exception)
      {   
        return "not ok";
      } 
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("username", username);
      cmd.Parameters.AddWithValue("password", password);
      cmd.Parameters.AddWithValue("firstname", firstname);
      cmd.Parameters.AddWithValue("lastname", lastname);
    }
  }
}