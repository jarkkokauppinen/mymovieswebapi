using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace mymovieswebapi.Controllers;

[ApiController]
[Route("[controller]")]

public class MovieActorController : ControllerBase
{
  public MovieActorController(Database db) {
    Db = db;
  }

  [HttpPost()]
  public async Task<IActionResult> Post([FromBody] MovieActor body)
  {
    await Db.Connection.OpenAsync();
    body.Db = Db;
    string result = await body.SaveActors();
    return new OkObjectResult(result);
  }

  public Database Db { get; set; }
}
