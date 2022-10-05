using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Rating
  {
    public float average { get; set; }
    public int raters { get; set; }
    public int rating { get; set; }
    public int iduser { get; set; }

    public Rating() {}

    internal Database Db { get; set; }

    internal Rating(Database db)
    {
      Db = db;
    }

    public async Task<Rating> GetRatings(int id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select round(avg(rating), 1), count(rating)
      from rating where idrating in (select idrating from
      movierating where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await Return(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveRatings()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into rating
      (rating, iduser) values (@rating, @iduser)";
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

    private async Task<Rating> Return(DbDataReader reader)
    {
      var ratings = new Rating(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          ratings = new Rating(Db)
          {
            average = 0,
            raters = 0
          };

          if (!reader.IsDBNull(0)) {
            ratings.average = reader.GetFloat(0);
          }

          if (!reader.IsDBNull(1)) {
            ratings.raters = reader.GetInt32(1);
          }
        }
      }
      return ratings;
    }

    private void BindParams(NpgsqlCommand cmd)
    {
      cmd.Parameters.AddWithValue("rating", rating);
      cmd.Parameters.AddWithValue("iduser", iduser);
    }
  }
}