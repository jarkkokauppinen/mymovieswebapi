using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace mymovieswebapi
{
  public class Genre
  {
    public string genre { get; set; }

    internal Database Db { get; set; }

    internal Genre(Database db)
    {
      Db = db;
    }

    public async Task<Genre> GetGenre(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select genre from genre where idgenre =
      (select idgenre from movie where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await Return(await cmd.ExecuteReaderAsync());
    }

    private async Task<Genre> Return(DbDataReader reader)
    {
      var genre = new Genre(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          genre = new Genre(Db)
          {
            genre = reader.GetString(0)
          };
        }
      }
      return genre;
    }
  }
}