using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class MovieActorController : ControllerBase
{
  public Database Db { get; set; }
  
  public MovieActorController(Database db) {
    Db = db;
  }

  [HttpGet()]
  public async Task<IActionResult> Get(string idmovie, string idactor)
  {
    await Db.Connection.OpenAsync();
    var query = new MovieActor(Db);
    return new OkObjectResult(await query.GetRow(idmovie, idactor));
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] MovieActor body)
  {
    await Db.Connection.OpenAsync();
    var query = new MovieActor(Db);
    var row = await query.GetRow(body.idmovie, body.idactor);

    if (row.idmovieactor == 0)
    {
      body.Db = Db;
      string result = await body.SaveActors();
      return new OkObjectResult(result);
    }

    return new OkObjectResult("This row already exist");
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(string id)
  {
    await Db.Connection.OpenAsync();
    var query = new MovieActor(Db);
    return new OkObjectResult(await query.DeleteRows(id));
  }
}
