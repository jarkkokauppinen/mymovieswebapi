using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace mymovieswebapi
{
  public class Quote
  {
    public string quote { get; set; }
    public string movie { get; set; }
    public string year { get; set; }

    internal Database Db { get; set; }

    internal Quote(Database db)
    {
      Db = db;
    }

    public async Task<Quote> GetQuote()
    {
      using var cmd = Db.Connection.CreateCommand();
      cmd.CommandText = @"select quote, movie, year
      from quote order by random() limit 1";
      return await Return(await cmd.ExecuteReaderAsync());
    }

    private async Task<Quote> Return(DbDataReader reader)
    {
      var quote = new Quote(Db);
      using (reader)
      {
        while (await reader.ReadAsync())
        {
          quote = new Quote(Db)
          {
            quote = reader.GetString(0),
            movie = reader.GetString(1),
            year = reader.GetString(2)
          };
        }
      }
      return quote;
    }
  }
}