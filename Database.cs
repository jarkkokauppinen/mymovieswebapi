using System;
using Npgsql;

namespace mymovieswebapi
{
  public class Database : IDisposable
  {
    public NpgsqlConnection Connection { get; }

    public Database(string connectionString)
    {
      Connection = new NpgsqlConnection(System.Environment.GetEnvironmentVariable("MY_DATABASE"));
    }

    public void Dispose() => Connection.Dispose();
  }
}