using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Npgsql;

namespace mymovieswebapi
{
  public class Rating
  {
    public string idrating { get; set; }
    public float average { get; set; }
    public int sum { get; set; }
    public int raters { get; set; }
    public int rating { get; set; }
    public int iduser { get; set; }

    internal Database Db { get; set; }

    public Rating() {}

    internal Rating(Database db)
    {
      Db = db;
    }

    public async Task<Rating> GetRatings(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select round(avg(rating), 1), sum(rating), count(rating)
      from rating where idrating in (select idrating from
      movierating where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      return await Return(await cmd.ExecuteReaderAsync());
    }

    public async Task<string> SaveRatings()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"insert into rating
      (idrating, rating, iduser) values (@idrating, @rating, @iduser)";
      cmd.Parameters.AddWithValue("idrating", idrating);
      cmd.Parameters.AddWithValue("rating", rating);
      cmd.Parameters.AddWithValue("iduser", iduser);
      await cmd.ExecuteNonQueryAsync();
      return "Success";
    }

    public async Task<string> DeleteRatings(string id)
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"delete from rating where idrating in
      (select idrating from movierating where idmovie = @id)";
      cmd.Parameters.AddWithValue("id", id);
      await cmd.ExecuteNonQueryAsync();
      return "Deleted";
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
            sum = 0,
            raters = 0
          };

          if (!reader.IsDBNull(0)) {
            ratings.average = reader.GetFloat(0);
          }

          if (!reader.IsDBNull(1)) {
            ratings.sum = reader.GetInt32(1);
          }

          if (!reader.IsDBNull(2)) {
            ratings.raters = reader.GetInt32(2);
          }
        }
      }
      return ratings;
    }
  }
}