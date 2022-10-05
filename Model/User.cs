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

    public async Task<User> GetInfo(string username)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select iduser, password
      from app_user where username = @username";
      cmd.Parameters.AddWithValue("username", username);
      return await ReturnUser(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> CreateAccount()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into app_user
      (username, password, firstname, lastname)
      values (@username, @password, @firstname, @lastname)";
      BindParams(cmd);
      try
      {
        await cmd.ExecuteNonQueryAsync();
        return "A new account created";
      }
      catch (System.Exception)
      {   
        return "The username you entered is already connected to an account";
      } 
    }

    private async Task<User> ReturnUser(DbDataReader reader)
    {
      var user = new User(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          user = new User(Db)
          {
            iduser = reader.GetInt32(0),
            password = reader.GetString(1)
          };
        }
      }
      return user;
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